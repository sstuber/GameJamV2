using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {

	// Use this for initialization
    public InfluenceController influenceController;
    public Vector3 target;
    float speed = 0.01f;
	void Start () {
        target = transform.position + new Vector3(10, 10);
	}
	
	// Update is called once per frame
	void Update () {
        //target = influenceController.GetBestTile();
        Point currentTile = StartGrid.PositionToGridIndex(transform.position);
        target = influenceController.GetBestTile(currentTile.x, currentTile.y);
        print(currentTile.x + " " + currentTile.y + " " + target);
        float addStuff = StartGrid.tileScale / 2f;
        target += new Vector3(addStuff, addStuff);
        float distance = (target - transform.position).magnitude;
        if (distance >= speed) {
            transform.position += (target - transform.position).normalized * speed;
        }
	}
}
