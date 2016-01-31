using UnityEngine;
using System.Collections;

public class StartUp : MonoBehaviour {
    public float amount = 3;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.realtimeSinceStartup > amount)
            Application.LoadLevel("Menu");
    }
}
