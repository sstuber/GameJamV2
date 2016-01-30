using UnityEngine;
using System.Collections;

public class StartGrid : MonoBehaviour {
    public GameObject _tile;
    public 
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
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void GenerateForest(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 startPoint = new Vector2((int)(Random.Range(0, 1) * (Width - 1)), (int)(Random.Range(0, 1) * (Height - 1)));

            for (int x = (int)startPoint.x -1; x <= startPoint.x +1; x++)
                for (int y = (int)startPoint.y-1; y <= startPoint.y +1; y++)
                {
                    if (Random.Range(0,1) < 0.8)
                    {
                        Grid[x, y].GetComponent<TileHandler>().ChangeBasicTileType(BTT.bos);
                    }
                }

        }


    }

    

}
