using UnityEngine;
using System.Collections.Generic;

public struct Point
{
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public int x, y;
}

public class StartGrid : MonoBehaviour {
    float UpdateTime = 0.25f;
    float UpdateTimer = 0.25f;
    public GameObject[,] prevGrid;
    bool bollie;
    public GameObject lightning;
    public AudioClip rain;
    public AudioClip thunder;
    public AudioClip river;
    public AudioClip forest;
    public AudioClip volcano;
    public AudioClip trap;
    public AudioClip rock;

    public GameObject rainMaker;
    public GameObject _tile;
    public GameObject InflTile;

    List<Vector2> pastVectors;
    TileHandler tile;
    public int Height, Width;
    public GameObject[,] Grid;

    public bool IsRaining = false;
    public float amountOfTimeItIsRaining;
    float rainTimer = 0;
    public static float tileScale;
    public InfluenceController ic;
    private GameObject icgo;
    private AudioSource source;
    
    // Use this for initialization
    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.clip = rain;
        
        
        gameObject.AddComponent<InfluenceController>();
        gameObject.GetComponent<InfluenceController>().sg = this;
        gameObject.GetComponent<InfluenceController>().tile = InflTile;
        tile = _tile.GetComponent<TileHandler>();
        Grid = new GameObject[Width, Height];
        prevGrid = new GameObject[Width, Height];
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            { Grid[x, y]= Instantiate(_tile);
                Grid[x, y].transform.position = new Vector3(x, y)* tile.scale + transform.position;
                prevGrid[x, y] = Grid[x, y];
            }
        tileScale = tile.scale;
        GenerateForest(5);
        GeneratePlateau(3);
       
        bollie = true;
        while (bollie)
        {
            int dingen = 0;
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    prevGrid[x, y] = Grid[x, y];
                    Grid[x, y].GetComponent<TileHandler>().checkAllValues(new Point(x, y), Grid, IsRaining);

                }

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    if (prevGrid[x, y] == Grid[x, y])
                    {
                        dingen++;

                    }

            }
            if(dingen == Width*Height)
            {
                bollie = false;
            }
        
        }
        GenerateRiver(3);
      //  GenerateStones(0.05f);
        // GenerateRiver(4);
    }

    // Update is called once per frame
    public static Point PositionToGridIndex(Vector3 pos)
    {
        int x = (int)(pos.x / tileScale);
        int y = (int)(pos.y / tileScale);
        return new Point(x, y);
    }

    public static Vector3 GridIndexToPosition(int x, int y) {
        float vx = x * tileScale;
        float vy = y * tileScale;
        return new Vector3(vx, vy);
    }
    void Update () {
        if (IsRaining)// zolang regent kijk of lava naar steen moet?
        {
            rainTimer -= Time.deltaTime;
            if (rainTimer < 0)
            {
                IsRaining = false;
                source.Stop();
                rainMaker.GetComponent<ParticleSystem>().Stop();
            }
        }
        UpdateTimer -= Time.deltaTime;
        if (UpdateTimer < 0)
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    Grid[x, y].GetComponent<TileHandler>().checkAllValues(new Point(x, y), Grid, IsRaining);
                }
            UpdateTimer = UpdateTime;
        }
	}
    #region mapgeneration
    void GeneratePlateau(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 startPoint = new Vector2((int)Random.Range(0, (Width - 1)), (int)Random.Range(0, (Height - 1)));
            //print(startPoint);
            Grid[(int)startPoint.x, (int)startPoint.y].GetComponent<TileHandler>().ChangeBasicTileType(BTT.plateau);
            pastVectors = new List<Vector2>();
            SpreadTiles(startPoint, BTT.plateau, 1.3f);
        }
    }
    void GenerateForest(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            //Vector2[] Points;
            //int[] Chance;
            Vector2 startPoint = new Vector2((int)Random.Range(0, (Width - 1)), (int)Random.Range(0, (Height - 1)));
            Grid[(int)startPoint.x, (int)startPoint.y].GetComponent<TileHandler>().ChangeBasicTileType(BTT.bos);
            pastVectors = new List<Vector2>();
            SpreadTiles(startPoint, BTT.bos, 1.1f);
           /* for (int x = (int)startPoint.x -1; x <= startPoint.x +1; x++)
                for (int y = (int)startPoint.y-1; y <= startPoint.y +1; y++)
                {
                    if (Random.Range(0,1) < 0.8)
                    {
                        Grid[x, y].GetComponent<TileHandler>().ChangeBasicTileType(BTT.bos);
                    }
                }*/

        }


    }

    void GenerateRiver(int amount)
    {
        for (int i = 0; i < amount; i++)
        {

            Vector2 startPoint = new Vector2((int)(Random.Range(2, (Width - 3))), (Height - 1));
            Vector2 endPoint = new Vector2(startPoint.x, (int)Random.Range(0, Height/2));

            for (int y = (int)startPoint.y; y > endPoint.y; y--)
            {
                if (Grid[(int)startPoint.x, y].GetComponent<TileHandler>().TileType == BTT.plateau)
                    Grid[(int)startPoint.x, y].GetComponent<TileHandler>().ChangeBasicTileType(BTT.flat);
                RitualsToMap(new Vector2(startPoint.x, y), 5);
                //Grid[(int)startPoint.x, y].GetComponent<TileHandler>().checkAllValues(new Point((int)startPoint.x, y), Grid, IsRaining);
            }
        }

    }
    void GenerateStones(float chance )
    {

            for (int y = 0; y < Height; y++)
                for (int x = 2; x < Width-2; x++)
                {

                    if (Random.Range(0,1)<= chance)
                    {
             /*           if (Grid[x, y].GetComponent<TileHandler>().steen)
                            continue;*/
                        RitualsToMap(new Vector2(x, y), 0);
                   // Grid[x, y].GetComponent<TileHandler>().steen = true;
                    }
                }

    }
    

    void SpreadTiles(Vector2 startPoint, BTT type, float chance)
    {
        //Grid[(int)startPoint.x, (int)startPoint.y].GetComponent<TileHandler>().ChangeBasicTileType(type);
        for (int x = (int)startPoint.x - 1; x <= startPoint.x + 1; x++)
            for (int y = (int)startPoint.y - 1; y <= startPoint.y + 1; y++)
            {
                Vector2 tempVec = new Vector2(x, y);
                if (pastVectors.Contains(tempVec))
                    continue;
                if (x < 0 || x >= Width || y < 0 || y >= Height)
                    continue;
                
                if (Random.Range(0, 100) < chance*100)
                {

                    Grid[x, y].GetComponent<TileHandler>().ChangeBasicTileType(type);
                    pastVectors.Add(tempVec);
                    SpreadTiles(tempVec, type, chance - 0.3f);
                }
            }

    }
    #endregion
    // numbers in action 0,6 
    public void RitualsToMap(Vector2 startPoint,  int abilityType)
    {
        switch (abilityType)
            {
            case 0: // place stone move
                {
                    Grid[(int)startPoint.x, (int)startPoint.y].GetComponent<TileHandler>().RenderSpecialProperty(abilityType, true);
                    source.PlayOneShot(rock);
                    gameObject.AddComponent<Rigidbody2D>();
                    gameObject.AddComponent<CircleCollider2D>();
                    var rb = GetComponent<Rigidbody2D>();
                    rb.mass = 10000000;
                    rb.drag = 10000000;
                    rb.gravityScale = 0;
                    var cc = GetComponent<CircleCollider2D>();
                    cc.radius = 0.35f;
                    cc.offset = new Vector2(startPoint.x * StartGrid.tileScale, startPoint.y * StartGrid.tileScale);     
                    break;    
                }
            case 4:
                {
                    for (int x = (int)startPoint.x - 2; x <= startPoint.x + 2; x++)
                        for (int y = (int)startPoint.y - 2; y <= startPoint.y + 2; y++)
                        {
                            if (x < 0 || x >= Width || y < 0 || y >= Height)
                                continue;
                            if (x == startPoint.x - 2 || x == startPoint.x + 2 || y == startPoint.y - 2 || y == startPoint.y + 2)
                            {
                                Grid[x,y].GetComponent<TileHandler>().RenderSpecialProperty(1,true);
                                Grid[x,y].GetComponent<TileHandler>().ChangeSpecialProperty(4, true);
                                
                            }
                            else if (x == startPoint.x - 1 || x == startPoint.x + 1 || y == startPoint.y - 1 || y == startPoint.y + 1)
                            {
                                source.PlayOneShot(trap);
                                source.PlayOneShot(volcano);
                                Grid[x,y].GetComponent<TileHandler>().ChangeSpecialProperty(0,true);
                            }
                            else
                            {
                                Grid[(int)startPoint.x, (int)startPoint.y].GetComponent<TileHandler>().RenderSpecialProperty(7, true);
                            }
                        }
                    break;
                }
            case 5:
                {
                    Grid[(int)startPoint.x, (int)startPoint.y].GetComponent<TileHandler>().RenderSpecialProperty(4,true);
                    source.PlayOneShot(trap);
                    break;
                }
            case 6: // Change area to forest
                {   // startpoint is the tile in the middle
                    source.PlayOneShot(forest);
                    for (int x = (int)startPoint.x - 1; x <= startPoint.x + 1; x++)
                        for (int y = (int)startPoint.y - 1; y <= startPoint.y + 1; y++)
                        {
                            if (x < 0 || x >= Width || y < 0 || y >= Height)
                                continue;
                            Grid[x, y].GetComponent<TileHandler>().ChangeBasicTileType(BTT.bos);

                        }
                            break;
                }
            case 7:
                {
                    IsRaining = true;
                    source.Play();
                    rainTimer = amountOfTimeItIsRaining;
                    rainMaker.GetComponent<ParticleSystem>().Play();
                    break;
                }
            case 8:
                {
                    bool found = false;
                    //Vector2 loc;
                    List<Vector2> tempList = new List<Vector2>();
                    for (int z = 0; found == false && z < 6; z++) {
                        for (int x = (int)startPoint.x - 1; x <= startPoint.x + 1; x++) {
                            for (int y = (int)startPoint.y - 1; y <= startPoint.y + 1; y++)
                            {
                                if (x < 0 || x >= Width || y < 0 || y >= Height)
                                { continue; }
                                BTT type = Grid[x, y].GetComponent<TileHandler>().TileType;
                                print((type == BTT.plateau) + ":" + (type == BTT.bos) + ":" + (type == BTT.flat));
                                if (z == 0 && Grid[x, y].GetComponent<TileHandler>().TileType == BTT.plateau)//+units
                                {
                                    Vector3 v = StartGrid.GridIndexToPosition(x, y);
                                    print(v);
                                    Vector2 temp = new Vector2(tileScale / 2, tileScale / 2);
                                    Collider2D[] units = Physics2D.OverlapAreaAll((Vector2)v + temp, (Vector2)v - temp);
                                    if (units.Length > 0)
                                    {
                                        found = true;
                                        tempList.Add(new Vector2(x, y));
                                    }
                                }
                                else if (z == 1 && Grid[x, y].GetComponent<TileHandler>().TileType == BTT.plateau)
                                {
                                    found = true;
                                    tempList.Add(new Vector2(x, y));
                                }
                                else if (z == 2 && Grid[x, y].GetComponent<TileHandler>().steen == true)//is a rock
                                {
                                    found = true;
                                    tempList.Add(new Vector2(x, y));
                                }
                                else if (z == 3 && Grid[x, y].GetComponent<TileHandler>().TileType == BTT.bos)
                                {
                                    found = true;
                                    tempList.Add(new Vector2(x, y));
                                }
                                else if (z == 4 && Grid[x, y].GetComponent<TileHandler>().TileType == BTT.flat)//+units
                                {
                                    Vector3 v = StartGrid.GridIndexToPosition(x, y);
                                    print(v);
                                    Vector2 temp = new Vector2(tileScale / 2, tileScale / 2);
                                    Collider2D[] units = Physics2D.OverlapAreaAll((Vector2)v+temp, (Vector2)v-temp);
                                    if (units.Length > 0)
                                    {
                                        print("located units");
                                        found = true;
                                        tempList.Add(new Vector2(x, y));
                                    }
                                }
                                else if (z == 5 && Grid[x, y].GetComponent<TileHandler>().TileType == BTT.flat)
                                {
                                    found = true;
                                    tempList.Add(new Vector2(x, y));
                                }
                            }
                        }

                            }
                    //print(z);
                    //print(found);
                    //TileHandler tempHandler = Grid[(int)startPoint.x, (int)startPoint.y].GetComponent<TileHandler>();

                    //Vector2[] tempArray = tempList.ToArray();
                    int k = (Random.Range(0, tempList.Count));

                    //source2.Stop();
                    source.PlayOneShot(thunder);
                    lightning.transform.position = gameObject.transform.position + (new Vector3(tempList[k].x, tempList[k].y, 0) * tileScale);
                    lightning.GetComponent<ParticleSystem>().Play();
                    Vector3 vec = StartGrid.GridIndexToPosition((int)tempList[k].x, (int)tempList[k].y);
                    //Collider2D[] unitsToHit = Physics2D.OverlapBox(vec, new Vector3(tileScale / 2, tileScale / 2, 999));
                   
              
                    Vector2 temp2 = new Vector2(tileScale / 2, tileScale / 2);
                    Collider2D[] unitsToHit = Physics2D.OverlapAreaAll((Vector2)vec + temp2, (Vector2)vec - temp2);
                    if (unitsToHit.Length > 0) 
                    {
                        foreach (Collider2D unit in unitsToHit) 
                        {
                            if (unit.tag == "Unit")
                            {
                                unit.GetComponent<UnitController>().alive = false;
                            }
                        }
                    }
                    //DO DAMAGE TO TILE
                    //tempArray[k]; doe dingen
                    // moet nog gedaan worden wat er moet gebeuren.

                    /*Lighting bolt   3x3 Kiest het ''hoogste'' doelwit
ie  plateau met unit -- plateau -- steen -- bos -- vechtunits op platland -- platland
effecten:  unit dood  -- niks    -- niks  -- bos tile krijgt vuur lv 1 -- unit dood -- niks*/
                            break;
                }


        }

    }
    
    
}
