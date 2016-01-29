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
        P2Selected.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.3f);
        P1Selected.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0.3f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
