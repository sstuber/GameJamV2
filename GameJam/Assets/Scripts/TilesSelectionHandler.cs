using UnityEngine;
using System.Collections;

public class TilesSelectionHandler : MonoBehaviour {
    public TileHandler Tile;
    public InputHandler InputHandler;
    public GameObject _SelectionTile;
    public float alpha;
    GameObject P1Selected;
    GameObject P2Selected;

	// Use this for initialization
	void Start () {
        P1Selected = Instantiate(_SelectionTile);
        P2Selected = Instantiate(_SelectionTile);
        P1Selected.transform.localScale *= Tile.scale;
        P2Selected.transform.localScale *= Tile.scale;
        P2Selected.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, alpha);
        P1Selected.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, alpha);
    }
	
	// Update is called once per frame
	void Update () {
        P2Selected.transform.position = (Vector3)InputHandler.TileP2*Tile.scale+(Vector3)transform.position + new Vector3(0,0,-1);
        P1Selected.transform.position = (Vector3)InputHandler.TileP1*Tile.scale+(Vector3)transform.position + new Vector3(0, 0, -1);
	}
}
