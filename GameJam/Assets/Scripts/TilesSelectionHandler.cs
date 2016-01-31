using UnityEngine;
using System.Collections;

public class TilesSelectionHandler : MonoBehaviour {
    public TileHandler Tile;
    public InputHandler InputHandler;
    public GameObject selectionPrefab;
    private selectedTileController p1, p2;

    // Use this for initialization
    void Start()
    {
        var ts = StartGrid.tileScale;
        GameObject tmp1 = (GameObject)Instantiate(selectionPrefab);
        GameObject tmp2 = (GameObject)Instantiate(selectionPrefab);
        p1 = tmp1.GetComponent<selectedTileController>();
        p2 = tmp2.GetComponent<selectedTileController>();

        p1.c = Color.red;
        p2.c = Color.blue;
    }

    // Update is called once per frame
    void Update () {
        p1.gameObject.transform.position = (Vector3)InputHandler.TileP2*Tile.scale+(Vector3)transform.position + new Vector3(0,0,-1);
        p2.gameObject.transform.position = (Vector3)InputHandler.TileP1*Tile.scale+(Vector3)transform.position + new Vector3(0, 0, -1);
	}
}
