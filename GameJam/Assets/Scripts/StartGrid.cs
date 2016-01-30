using UnityEngine;
using System.Collections.Generic;

public class StartGrid : MonoBehaviour {
    public GameObject _tile;
    List<Vector2> pastVectors;
    TileHandler tile;
    public int Height, Width;
    public GameObject[,] Grid;
    // Use this for initialization
    void Start()
    {
        
        tile = _tile.GetComponent<TileHandler>();
        Grid = new GameObject[Width, Height];
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            { Grid[x, y]= Instantiate(_tile);
                Grid[x, y].transform.position = new Vector3(x, y)* tile.scale + transform.position;
            }

        GenerateForest(3);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

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
    

}
