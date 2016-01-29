using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {

	// Use this for initialization
    public InfluenceController influenceController;
    public Vector3 target;
    float speed = 0.1f;
	void Start () {
        target = transform.position + new Vector3(10, 10);
	}
	
	// Update is called once per frame
	void Update () {
        //target = influenceController.GetBestTile();
        float distance = (target - transform.position).magnitude;
        print(target);
        if (distance >= speed) {
            transform.position += (target - transform.position).normalized * speed;
        }
	}
}
