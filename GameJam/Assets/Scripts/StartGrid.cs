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
    public GameObject _tile;
    List<Vector2> pastVectors;
    TileHandler tile;
    public int Height, Width;
    public GameObject[,] Grid;
    public bool IsRaining = false;
    public float amountOfTimeItIsRaining;
    float rainTimer = 0;
    float UpdateTime = 0.25f;
    float UpdateTimer = 0.25f;
    public static float tileScale;


    public GameObject[,] prevGrid;
    bool bollie;

    // Use this for initialization
    void Start()
    {
        
        tile = _tile.GetComponent<TileHandler>();
        Grid = new GameObject[Width, Height];
        prevGrid = new GameObject[Width, Height];
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            { Grid[x, y]= Instantiate(_tile);
                Grid[x, y].transform.position = new Vector3(x, y)* tile.scale + transform.position;
      //          Grid[x, y].GetComponent<TileHandler>().fireSmall = true;
                prevGrid[x, y] = Grid[x, y];
            }
        tileScale = tile.scale;
        GenerateForest(3);
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
    }

    // Update is called once per frame
    public static Point PositionToGridIndex(Vector3 pos)
    {
        int x = (int)(pos.x / tileScale);
        int y = (int)(pos.x / tileScale);
        return new Point(x, y);
    }
    void Update () {
        if (IsRaining)// zolang regent kijk of lava naar steen moet?
        {
            rainTimer -= Time.deltaTime;
            if (rainTimer < 0)
                IsRaining = false;
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
    void GenerateForest(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            //Vector2[] Points;
            //int[] Chance;
            Vector2 startPoint = new Vector2((int)Random.Range(0, (Width - 1)), (int)Random.Range(0, (Height - 1)));
            print(startPoint);
            Grid[(int)startPoint.x, (int)startPoint.y].GetComponent<TileHandler>().ChangeBasicTileType(BTT.bos);
            pastVectors = new List<Vector2>();
            SpreadTiles(startPoint, BTT.bos, 0.8f);
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
                    Grid[(int)startPoint.x, (int)startPoint.y].GetComponent<TileHandler>().ChangeSpecialProperty(abilityType, true);
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
                                Grid[x,y].GetComponent<TileHandler>().ChangeSpecialProperty(1,true);
                                Grid[x,y].GetComponent<TileHandler>().ChangeSpecialProperty(5, true);
                            }
                            else if (x == startPoint.x - 1 || x == startPoint.x + 1 || y == startPoint.y - 1 || y == startPoint.y + 1)
                            {
                                Grid[x,y].GetComponent<TileHandler>().ChangeSpecialProperty(0,  true);
                            }
                            else
                            {
                                Grid[(int)startPoint.x, (int)startPoint.y].GetComponent<TileHandler>().ChangeSpecialProperty(5, true);
                            }
                        }
                    break;
                }
            case 5:
                {
                    Grid[(int)startPoint.x, (int)startPoint.y].GetComponent<TileHandler>().ChangeSpecialProperty(abilityType,true);
                    break;
                }
            case 6: // Change area to forest
                {   // startpoint is the tile in the middle
                    for (int x = (int)startPoint.x - 1; x <= startPoint.x + 1; x++)
                        for (int y = (int)startPoint.y - 1; y <= startPoint.y + 1; y++)
                        {
                            if (x < 0 || x >= Width || y < 0 || y >= Height)
                                continue;
                            Grid[x, y].GetComponent<TileHandler>().ChangeBasicTileType(BTT.bos);
                            print("test");
                        }
                            break;
                }
            case 7:
                {
                    IsRaining = true;
                    rainTimer = amountOfTimeItIsRaining;
                    break;
                }
            case 8:
                {
                    bool found = false;
                    //Vector2 loc;
                    List<Vector2> tempList = new List<Vector2>();
                    for (int z = 0; found==false&&z<6; z++)
                    for (int x = (int)startPoint.x - 1; x <= startPoint.x + 1; x++)
                        for (int y = (int)startPoint.y - 1; y <= startPoint.y + 1; y++)
                        {
                            if (x < 0 || x >= Width || y < 0 || y >= Height)
                                continue;
                                if (z==0&&Grid[x,y].GetComponent<TileHandler>().TileType==BTT.plateau)//+units
                                {
                                    found = true;
                                    tempList.Add(new Vector2(x,y));
                                }
                                else if (z == 1 && Grid[x, y].GetComponent<TileHandler>().TileType == BTT.plateau)
                                {
                                    found = true;
                                    tempList.Add(new Vector2(x, y));
                                }
                                else if (z == 2 && Grid[x, y].GetComponent<TileHandler>().SpecialProps[0]==true)//is a rock
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
                                    found = true;
                                    tempList.Add(new Vector2(x, y));
                                }
                                else if (z == 5 && Grid[x, y].GetComponent<TileHandler>().TileType == BTT.flat)
                                {
                                    found = true;
                                    tempList.Add(new Vector2(x, y));
                                }
                                //TileHandler tempHandler = Grid[(int)startPoint.x, (int)startPoint.y].GetComponent<TileHandler>();

                                Vector2[] tempArray = tempList.ToArray();
                                int k = (Random.Range(0, tempArray.Length));

                                //tempArray[k]; doe dingen
                                // moet nog gedaan worden wat er moet gebeuren.

                                /*Lighting bolt   3x3 Kiest het ''hoogste'' doelwit
    ie  plateau met unit -- plateau -- steen -- bos -- vechtunits op platland -- platland
   effecten:  unit dood  -- niks    -- niks  -- bos tile krijgt vuur lv 1 -- unit dood -- niks*/
                            }
                            break;
                }


        }

    }
    

}
