using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfluenceController : MonoBehaviour {

	// Use this for initialization
    public List<InfluenceMap> influenceMaps;
    public GameObject tile;
    private GameObject[,] tiles;
    StartGrid sg;
    public bool drawInfluence = true;
    private bool prevDraw;
	void Start () {
        prevDraw = drawInfluence;
        sg = transform.parent.gameObject.GetComponent<StartGrid>();
        tiles = new GameObject[sg.Width, sg.Height];
        influenceMaps = new List<InfluenceMap>();
        AddMap("goTopRight");

        for(int y = 0; y < sg.Height; ++y){
            for (int x = 0; x < sg.Width; ++x) {
                GameObject newTile = (GameObject)Instantiate(tile, sg.Grid[x, y].transform.position, Quaternion.identity);
                tiles[x, y] = newTile;
                influenceMaps[0].influences[x, y] = 0;//(1 + x) * (1 + y);
                tiles[x, y].transform.localScale *= StartGrid.tileScale;
            }
        }

        AddInfluence("goTopRight", 100f, 0.75f, 8, 8, 4);

        EnableDrawing(true);
	}

    void EnableDrawing(bool enable = true) {
        foreach (GameObject tile in tiles) {
            SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
            sr.enabled = enable;
        }
    }

    void ColourInfluence() {
        for (int y = 0; y < sg.Height; ++y) {
            for (int x = 0; x < sg.Width; ++x) {
                SpriteRenderer sr = tiles[x,y].GetComponent<SpriteRenderer>();
                sr.color = new Color(influenceMaps[0].influences[x, y] / 128f, 0.4f, 0.6f, 0.33f) ;
            }
        }
    }
	// Update is called once per frame
	void Update () {
        if (prevDraw != drawInfluence)
            EnableDrawing(drawInfluence);
        if(drawInfluence)
            ColourInfluence();

        prevDraw = drawInfluence;
	}

    public Vector3 GetBestTile(int x, int y){
        int bestX = 0, bestY = 0;
        float bestValue = influenceMaps[0].influences[x, y];
        for (int iy = -1; iy < 2; ++iy) {
            for (int ix = -1; ix < 2; ++ix) {
                int currentX = Mathf.Clamp(x + ix, 0, sg.Width - 1);
                int currentY = Mathf.Clamp(y + iy, 0, sg.Height - 1);
                float currentValue = influenceMaps[0].influences[currentX, currentY];
                if ((currentValue * 0.9f) > bestValue) {
                    bestValue = currentValue;
                    bestX = currentX;
                    bestY = currentY;
                }
            }
        }
        return tiles[bestX, bestY].transform.position;
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
    }

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
