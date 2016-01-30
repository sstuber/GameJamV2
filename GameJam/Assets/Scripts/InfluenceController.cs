using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfluenceController : MonoBehaviour {

	// Use this for initialization
    public List<InfluenceMap> influenceMaps;
    public GameObject tile;
    private GameObject[,] tiles;
    public Point targetTile;
    StartGrid sg;
    public int drawInfluenceMapIndex = 0;
    public bool drawInfluenceMap = true;
    private bool prevDraw;
	void Start () {
        prevDraw = drawInfluenceMap;
        sg = transform.parent.gameObject.GetComponent<StartGrid>();
        tiles = new GameObject[sg.Width, sg.Height];
        influenceMaps = new List<InfluenceMap>();
        AddMap("goTopRight");

        for(int y = 0; y < sg.Height; ++y){
            for (int x = 0; x < sg.Width; ++x) {
                GameObject newTile = (GameObject)Instantiate(tile, sg.Grid[x, y].transform.position + new Vector3(0,0,-1), Quaternion.identity);
                tiles[x, y] = newTile;
                influenceMaps[0].influences[x, y] = 0;//(1 + x) * (1 + y);
                tiles[x, y].transform.localScale *= StartGrid.tileScale;
            }
        }

        //AddInfluence("goTopRight", 100f, 0.75f, 8, 8, 4);
        //AddInfluence("goTopRight", 16, 5, 1000, 20);
        EnableDrawing(true);
	}

    void EnableDrawing(bool enable = true) {
        foreach (GameObject tile in tiles) {
            SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
            sr.enabled = enable;
        }
    }

    void ColourInfluence(int index, Color color) {
        for (int y = 0; y < sg.Height; ++y) {
            for (int x = 0; x < sg.Width; ++x) {
                SpriteRenderer sr = tiles[x,y].GetComponent<SpriteRenderer>();
                float intensity = 4 * influenceMaps[index].influences[x, y] / 128f;
                if (influenceMaps[index].influences[x, y] < 0) {
                    intensity *= -1f;
                    sr.color = color * intensity;
                    //sr.color = new Color(intensity + 0.33f, 0.33f, 0.33f, 0.75f);
                }
                else
                    sr.color = color * intensity;

                    //sr.color = new Color(0.33f, 0.33f, intensity + 0.33f, 0.75f);
            }
        }
    }
	// Update is called once per frame
	void Update () {
        FloodFromTile(12, 5);

        if (prevDraw != drawInfluenceMap)
            EnableDrawing(drawInfluenceMap);
        if (drawInfluenceMap)
            ColourInfluence(drawInfluenceMapIndex, Color.yellow);

        prevDraw = drawInfluenceMap;
	}

    class Tuple<T, K> {
        public Tuple(T x, K y){
            this.x = x;
            this.y = y;
        }
        public T x;
        public K y;
    }

    class PriorityQueue<T>{
        LinkedList<Tuple<T, float>> queue;

        public PriorityQueue() {
            queue = new LinkedList<Tuple<T, float>>();
        }
        public void Add(T elem, float importance) {

            Tuple<T, float> newElem = new Tuple<T, float>(elem, importance);

            if (queue.Count == 0) {
                queue.AddFirst(newElem);
                return;
            }

            var curElem = queue.First;
            while (curElem != queue.Last) {
                if (importance < curElem.Value.y) {
                    queue.AddBefore(curElem, newElem);
                    return;
                }
                curElem = curElem.Next;
            }

            queue.AddLast(newElem);
        }

        public T Dequeue() {
            var first = queue.First.Value;
            queue.RemoveFirst();
            return first.x;
        }

        public int Count() {
            return queue.Count;
        }
    }
    void FloodFromTile(int x, int y) {
        PriorityQueue<Tuple<Point, float>> pq = new PriorityQueue<Tuple<Point, float>>();
        HashSet<Point> visited = new HashSet<Point>();

        pq.Add(new Tuple<Point, float>(new Point(x, y), 0), 0);
        visited.Add(new Point(x, y));
        int imp = 0;
        while (pq.Count() > 0) {
            var currentTuple = pq.Dequeue();
            var currentPoint = currentTuple.x;
            var currentDepth = currentTuple.y;

            influenceMaps[0].influences[currentPoint.x, currentPoint.y] = -currentDepth;

            for (int iy = -1; iy < 2; ++iy) {
                for (int ix = -1; ix < 2; ++ix) {
                    if (ix == 0 && iy == 0)
                        continue;

                    int currentX = currentPoint.x + ix;
                    int currentY = currentPoint.y + iy;

                    if (currentX < 0 || currentX >= sg.Width || currentY < 0 || currentY >= sg.Height)
                        continue;

                    Point neighorPoint = new Point(currentX, currentY);
                    if (visited.Contains(neighorPoint))
                        continue;

                    float neighborDepth = currentDepth + 1;

                    float importance = neighborDepth;
                    var ding = sg.Grid[currentX, currentY];
                    TileHandler dong = (TileHandler)ding.GetComponent<TileHandler>();
                    //hoeveel extra blokjes omlopen vind het waard om dit te vermijden?
                    if (dong.TileType == BTT.bos) {
                        importance += 1;
                    }
                    if (dong.TileType == BTT.plateau)
                        importance += 4;

                    if (Mathf.Abs(ix) + Mathf.Abs(iy) == 2)
                        importance *= 1.4f;

                    pq.Add(new Tuple<Point, float>(neighorPoint, importance), importance);
                    visited.Add(neighorPoint);
                }
            }
        }
        
    }

    public Point GetBestTile(int x, int y){
        int bestX = x, bestY = y;
        float bestValue = float.MinValue;// influenceMaps[0].influences[x, y];
        for (int iy = -1; iy < 2; ++iy) {
            for (int ix = -1; ix < 2; ++ix) {
                int currentX = x + ix;
                if(currentX < 0 || currentX >= sg.Width)
                    continue;
                int currentY = y + iy;
                if (currentY < 0 || currentY >= sg.Height)
                    continue;

                float currentValue = influenceMaps[0].influences[currentX, currentY];
                if ((currentValue * 0.99f) > bestValue) {
                    bestValue = currentValue;
                    bestX = currentX;
                    bestY = currentY;
                }
            }
        }
        return new Point(bestX, bestY);
    }

    private void UpdateMaps() {
        foreach (InfluenceMap map in influenceMaps) {
            map.UpdateMap();
        }
    }

    private void AddMap(string mapName){
        InfluenceMap map = new InfluenceMap(mapName, sg.Width, sg.Height);
        influenceMaps.Add(map);
    }
    private void AddInfluence(string mapName, int x, int y, float power, int radius) {
        InfluenceMap map = findMap(mapName);
        if (map == null)
            return;

        for (int iy = -radius; iy < radius + 1; ++iy) {
            for (int ix = -radius; ix < radius + 1; ++ix) {
                int currentX = x + ix;
                int currentY = y + iy;
                if(currentX < 0 || currentX >= sg.Width
                    || currentY < 0 || currentY >= sg.Height) {
                        continue;
                }

                Vector2 deltaVector = new Vector2(currentX - x, currentY - y);
                float distance = deltaVector.magnitude;
                if (distance > radius)
                    continue;

                map.influences[currentX, currentY] += power / distance;
            }
        }
    }
    /*
    private void AddInfluence(string mapName, float power, float taper, int x, int y, int maxLoops) {
        InfluenceMap map = findMap(mapName);
        if (map == null)
            return;

        AddInfluenceRecursive(map, power, taper, x, y, maxLoops, 0);

    }

    private void AddInfluenceRecursive(InfluenceMap map, float power, float taper, int x, int y, int maxLoops, int loop = 0) {
        if (loop > maxLoops || Mathf.Abs(power) < 1f || Mathf.Abs(power * taper) < 1f)
            return;
        
        else {
            for(int iy = -loop; iy < loop + 1; iy++){
                int currentY = y + iy;
                if (currentY < 0 || currentY >= sg.Height)
                    continue;
                if (iy == -loop || iy == loop) {
                    for (int ix = -loop; ix < loop + 1; ix++) {
                        int currentX = x + ix;
                        if (currentX < 0 || currentX >= sg.Width)
                            continue;

                        map.influences[currentX, currentY] += power;
                    }
                }
                else {
                    int currentX = x - loop;
                    if (currentX < 0 || currentX >= sg.Width)
                        continue;

                    map.influences[currentX, currentY] += power;
                    currentX = x + loop;
                    if (currentX < 0 || currentX >= sg.Width)
                        continue;

                    map.influences[currentX, currentY] += power;

                }
            }
        }

        AddInfluenceRecursive(map, power * taper, taper, x, y, maxLoops, loop + 1);
    }*/

    InfluenceMap findMap(string mapName) {
        foreach(InfluenceMap map in influenceMaps)
            if (map.name == mapName)
                return map;
        return null;
    }
}


public class InfluenceMap{
    public string name;
    public float[,] influences;

    public InfluenceMap(string name, int width, int height) {
        this.name = name;
        this.influences = new float[width, height];
    }

    public void UpdateMap() {

    }
}
