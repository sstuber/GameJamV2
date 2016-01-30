using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {
    public Vector2 StartPosition; 
	// Use this for initialization
    public InfluenceController influenceController;
    public Point currentTile;
    public Vector3 target;
    public int player;
    private Animator animator;
    float speed = 0.01f;
	void Start () {
        transform.localScale *= StartGrid.tileScale;
        //target = transform.position;// + new Vector3(10, 10);
        transform.position = StartGrid.GridIndexToPosition((int)StartPosition.x, (int)StartPosition.y);
        this.influenceController = GameObject.FindGameObjectWithTag("ic").GetComponent<InfluenceController>();
        animator = this.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        
        currentTile = StartGrid.PositionToGridIndex(transform.position);
        Point targetTile = influenceController.GetBestTile(currentTile.x, currentTile.y, player);
        target = StartGrid.GridIndexToPosition(targetTile.x, targetTile.y) + new Vector3(0.1f, 0.1f, 0);
        var dir = (target - transform.position).normalized;
        dir.z = 0;
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
        if (currentTile.x == targetTile.x && currentTile.y == targetTile.y) {
            rb.velocity *= 0.99f;
            animator.speed = 0;
        }
        else {
            rb.AddForce(dir * 100);
        }

        // Changing Direction
        if (dir.x > 0) animator.SetInteger("Direction", 3);
        if (dir.x < dir.y) animator.SetInteger("Direction", 2);
        if (dir.x < 0 && dir.y < dir.x) animator.SetInteger("Direction", 0);
        else if (dir.x < 0) animator.SetInteger("Direction", 1);

        
        //transform.position = StartGrid.GridIndexToPosition((int)StartPosition.x, (int)StartPosition.y);

    }
}
