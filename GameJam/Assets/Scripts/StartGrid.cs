using UnityEngine;
using System.Collections;

public class StartGrid : MonoBehaviour {
    public GameObject tile;
    GameObject[,] Grid = new GameObject[16, 16];
    // Use this for initialization
    void Start()
    {
        Grid = new GameObject[16, 16];
        for (int i = 0; i < 16; i++)
            for (int j = 0; j < 16; j++)
            { Grid[i, j] = Instantiate(tile);
                Grid[i, j].transform.position = new Vector3(i, j) + tile.transform.position;
            }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
