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
    public BTT TileType;
    // Use this for initialization
    public bool[] SpecialProps;
  /*  bool Stone = false;           0
    bool Lava = false;              1
    bool River = false;             2
    bool Swamp = false;             3
    bool Mud = false;               4
    bool valkuil/trap = false;      5
    */
   


    void Start () {
        this.transform.localScale *= scale;
        sprite = GetComponent<SpriteRenderer>().sprite;
        SpecialProps = new bool[6];
        for (int i = 0; i < SpecialProps.Length; i++)
            SpecialProps[i] = false;

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
