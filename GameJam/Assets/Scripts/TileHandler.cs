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
                      sprite = SpriteArray[1];
                    break;
                }
            case BTT.plateau:
                {
                    TileType = BTT.plateau;
                    sprite = SpriteArray[2];
                    break;
                }
            default:
                {
                    TileType = BTT.flat;
                    sprite = SpriteArray[0];
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
