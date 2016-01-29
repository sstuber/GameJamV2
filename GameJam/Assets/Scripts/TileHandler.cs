using UnityEngine;
using System.Collections;

public class TileHandler : MonoBehaviour {
    public float scale; 
	// Use this for initialization
	void Start () {
        this.transform.localScale *= scale;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
