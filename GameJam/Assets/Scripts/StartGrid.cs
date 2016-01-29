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
}
