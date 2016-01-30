using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {

	// Use this for initialization
    public InfluenceController influenceController;
    public Point currentTile;
    public Vector3 target;
    float speed = 0.01f;
	void Start () {
        target = transform.position + new Vector3(10, 10);
	}
	
	// Update is called once per frame
	void Update () {
        currentTile = StartGrid.PositionToGridIndex(transform.position);
        Point targetTile = influenceController.GetBestTile(currentTile.x, currentTile.y);
        target = StartGrid.GridIndexToPosition(targetTile.x, targetTile.y) + new Vector3(0.1f, 0.1f, 0);
        var dir = (target - transform.position).normalized;
        dir.z = 0;
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
        rb.AddForce(dir * 1000);
        print(currentTile.x + " " + currentTile.y + " " + targetTile.x + " " + targetTile.y);
	}
}
