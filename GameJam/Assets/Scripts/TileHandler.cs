using UnityEngine;
using System.Collections;

public enum BTT
{
    flat,
    bos,
    plateau
}


public class TileHandler : MonoBehaviour {
    public float scale;
    public Sprite[] SpriteArray;
    Sprite sprite;
    BTT TileType;
	// Use this for initialization
	void Start () {
        this.transform.localScale *= scale;
        sprite = GetComponent<SpriteRenderer>().sprite;
	}
	
    void ChoseBasicType(BTT type)
    {
        switch (type)
        {
            case BTT.bos:
                {
                    TileType = BTT.bos;
                    GetComponent<SpriteRenderer>().sprite = SpriteArray[1];
                    print("changed to forrest");
                    break;
                }
            case BTT.plateau:
                {
                    TileType = BTT.plateau;
                    GetComponent<SpriteRenderer>().sprite = SpriteArray[2];
                    break;
                }
            default:
                {
                    TileType = BTT.flat;
                    GetComponent<SpriteRenderer>().sprite = SpriteArray[0];
                    break;
                }
        }
    }

	// Update is called once per frame
	void Update () {
	    
	}

    public void ChangeBasicTileType(BTT type)
    {
        ChoseBasicType(type);

    }
}
