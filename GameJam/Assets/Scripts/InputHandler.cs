using UnityEngine;
using System;
using System.Collections;

public class InputHandler : MonoBehaviour {
    public GameObject _Map;
    GameObject[,] grid;
    int Width, Height;
    public Vector2 TileP1;
    public Vector2 TileP2;
	// Use this for initialization
	void Start () {
        grid = _Map.GetComponent<StartGrid>().Grid;
        Width = _Map.GetComponent<StartGrid>().Width;
        Height = _Map.GetComponent<StartGrid>().Height;

        TileP1 = new Vector2(0, (int)(Height - 1) / 2);
        TileP2 = new Vector2(Width - 1, (int)(Height - 1) / 2);

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W))
        {

            TileP1.y = Mathf.Min(TileP1.y + 1, Height-1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            TileP1.x = Mathf.Max(TileP1.x - 1, 0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {

            TileP1.y = Mathf.Max(TileP1.y - 1, 0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {

            TileP1.x = Mathf.Min(TileP1.x + 1, Width-1);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            TileP2.y = Mathf.Min(TileP2.y + 1, Height-1);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            TileP2.x = Mathf.Max(TileP2.x - 1, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            TileP2.y = Mathf.Max(TileP2.y - 1, 0);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            TileP2.x = Mathf.Min(TileP2.x + 1, Width-1);
        }
    }
}
