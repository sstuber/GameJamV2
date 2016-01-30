using UnityEngine;
using System.Collections;

public struct Point {
    public Point(int x, int y) {
        this.x = x;
        this.y = y;
    }
    public int x, y;
}

public class StartGrid : MonoBehaviour {
    public GameObject _tile;
    public TileHandler tile;
    public int Height, Width;
    public GameObject[,] Grid;
    public static float tileScale;
    // Use this for initialization

    void Awake() {
        tile = _tile.GetComponent<TileHandler>();
        tileScale = tile.scale;
        Grid = new GameObject[Width, Height];
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++) {
                Grid[x, y] = Instantiate(_tile);
                Grid[x, y].transform.position = new Vector3(x, y) * tile.scale + transform.position + new Vector3(0, 0, 1);
            }
    }


    public static Point PositionToGridIndex(Vector3 pos) {
        int x = (int)(pos.x / tileScale);
        int y = (int)(pos.x / tileScale);
        return new Point(x, y);
    }


    void Start()
    {
 
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
