﻿using UnityEngine;
using System.Collections;

public class TowerHandler : MonoBehaviour {
    public int player;
    public int Lives;
    public Point Position;
    private AudioHandler handle;
	// Use this for initialization
	void Start () {
        transform.position = StartGrid.GridIndexToPosition( Position.x,Position.y);
        handle = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioHandler>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        UnitController obj = other.gameObject.GetComponent<UnitController>();
        if (obj.player != player)
        {
            handle.Request(4);
            Lives--;
            Destroy(other.gameObject);
            if (Lives <= 0)
            { }
            if (player == 0)
                Application.LoadLevel("RedEnd");
            else
                Application.LoadLevel("GameEnd");
        }
    }
}
