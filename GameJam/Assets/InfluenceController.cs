using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfluenceController : MonoBehaviour {

	// Use this for initialization
    public List<InfluenceMap> influenceMaps;
    public GameObject tile;
    private GameObject[,] tiles;
    StartGrid sg;
	void Start () {
        sg = transform.parent.gameObject.GetComponent<StartGrid>();
        tiles = new GameObject[sg.Width, sg.Height];
        influenceMaps = new List<InfluenceMap>();
        AddMap("goTopRight");

        for(int y = 0; y < sg.Height; ++y){
            for (int x = 0; x < sg.Width; ++x) {
                GameObject newTile = (GameObject)Instantiate(tile, sg.Grid[x, y].transform.position * sg.tile.scale, Quaternion.identity);
                tiles[x, y] = newTile;
                influenceMaps[0].influences[x, y] = (1 + x) * (1 + y);
            }
        }

        EnableDrawing(true);
	}

    void EnableDrawing(bool enable = true) {
        foreach (GameObject tile in tiles) {
            SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
            sr.enabled = enable;
        }
    }

    void ColourInfluence() {
        for (int y = 0; y < sg.Height; ++y) {
            for (int x = 0; x < sg.Width; ++x) {
                SpriteRenderer sr = tiles[x,y].GetComponent<SpriteRenderer>();
                sr.color = new Color(influenceMaps[0].influences[x, y] / 256f, 0.5f, 0.5f, 0.5f) ;
            }
        }
    }
	// Update is called once per frame
	void Update () {
        ColourInfluence();
	}

    public Vector3 GetBestTile(int x, int y){
        int bestX = 0, bestY = 0;
        float bestValue = influenceMaps[0].influences[x, y];
        for (int iy = -1; iy < 2; ++iy) {
            for (int ix = -1; ix < 2; ++ix) {
                int currentX = Mathf.Clamp(x + ix, 0, sg.Width - 1);
                int currentY = Mathf.Clamp(y + iy, 0, sg.Height - 1);
                float currentValue = influenceMaps[0].influences[currentX, currentY];
                if (currentValue > bestValue) {
                    bestValue = currentValue;
                    bestX = currentX;
                    bestY = currentY;
                }
            }
        }
        return tiles[bestX, bestY].transform.position;
    }

    private void UpdateMaps() {
        foreach (InfluenceMap map in influenceMaps) {
            map.UpdateMap();
        }
    }

    private void AddMap(string mapName){
        InfluenceMap map = new InfluenceMap(mapName, sg.Width, sg.Height);
        influenceMaps.Add(map);
    }
}


public class InfluenceMap{
    public string name;
    public float[,] influences;

    public InfluenceMap(string name, int width, int height) {
        this.name = name;
        this.influences = new float[width, height];
    }

    public void UpdateMap() {

    }
}
