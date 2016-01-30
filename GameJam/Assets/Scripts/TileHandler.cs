using UnityEngine;
using System.Collections;

public enum BTT
{
    flat,
    bos,
    plateau
}
public enum LN 
{ 
    fireSmall,
    fireBig,
    river,
    mud,
    lava,
    rock,
    pitfall,
    volcano
}

public class TileHandler : MonoBehaviour {
    public float scale;
    public bool usingRandom;
    public GameObject overlaySprite;
    public Sprite[] OverlayArray;
    public GameObject[] overlays;
    public Sprite[] SpriteArray;
    //public StartGrid scene;
    Sprite sprite;
    public BTT TileType;
    // Use this for initialization
    public bool[] SpecialProps;
    public bool steen = false;        //   0
    public bool lava = false;         //     1
    public bool river = false;         //    2
    public bool mud = false;         //    3
    public bool dent = false;   //   5
    public bool fireBig = false;
    public bool fireSmall = false; //  6
    
   


    void Start () {
        this.transform.localScale *= scale;
        sprite = GetComponent<SpriteRenderer>().sprite;
        overlays = new GameObject[8];
        for (int i = 0; i < 7; i++)
        {
            overlays[i] = Instantiate(overlaySprite);
            overlaySprite.transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f - (i / 10));
            overlaySprite.transform.localScale *= scale;
            overlaySprite.GetComponent<SpriteRenderer>().sprite = OverlayArray[i];
            overlaySprite.GetComponent<SpriteRenderer>().enabled = false;
        }

	}
	
    void ChoseBasicType(BTT type)
    {
        switch (type)
        {
            case BTT.bos:
                {
                    TileType = BTT.bos;
                    GetComponent<SpriteRenderer>().sprite = SpriteArray[1];
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

    public void checkAllValues(Point GridPosition,  GameObject[,] grid, bool rain )
    {

        #region ForestTile
        if (TileType == BTT.flat)
        {
            if (fireBig) 
            {
               RenderSpecialProperty(LN.fireBig, false);
                   // fireBig = false; 
            }
        }
        if (TileType == BTT.bos)
        {
            
            if (fireSmall)
            {
                RenderSpecialProperty(LN.fireSmall, true);

                //fireBig = true;
            }

            if (lava)
            {
               if (!usingRandom || Random.Range(0, 100) < 80)
                {
                    RenderSpecialProperty(LN.lava, true);
                    //fireSmall = true;
                }
            }

           
            if (fireBig)
            {
                ChangeBasicTileType(BTT.flat);

            }

        }

        #endregion

        #region Plateau
        if (TileType == BTT.plateau)
        {
            RenderSpecialProperty(LN.river, false);
            RenderSpecialProperty(LN.mud, false);
            RenderSpecialProperty(LN.rock, false);
            RenderSpecialProperty(LN.fireSmall, false);
            RenderSpecialProperty(LN.fireBig, false);

           /* river = false;
            mud = false;
            steen = false;
            fireSmall = false;
            fireBig = false;*/

            if (dent)
            {
                RenderSpecialProperty(LN.rock, true);
                //steen = true;
                RenderSpecialProperty(LN.pitfall, false);
                //dent = false;
                ChangeBasicTileType(BTT.flat);
            }
        }


        #endregion
        #region compressed logic

        if (rain||mud)
        {
            
            //fireSmall = false;
            RenderSpecialProperty(LN.fireSmall, false);
            RenderSpecialProperty(LN.fireBig, false);
            if (rain && !lava && dent&&TileType!=BTT.plateau)
            {
                RenderSpecialProperty(LN.river, true);
                //river = true;
            }
            if (TileType != BTT.plateau)
            {
                RenderSpecialProperty(LN.mud, true);
               // mud = true;
            }
        }
        if (fireBig && fireSmall)
        {
            //fireSmall = false;
            RenderSpecialProperty(LN.fireSmall,false);
        }

        if (river && !dent)
        {
            RenderSpecialProperty(LN.river,false);
            //river = false;
        }
        if (river && steen&&TileType!=BTT.plateau)
        {
            
            RenderSpecialProperty(LN.mud, true);
            //mud = true;
        }
        if (lava && river)
        {
            RenderSpecialProperty(LN.lava, false);
            RenderSpecialProperty(LN.river, false);
            //lava = false;
            //river = false;
            if (dent)
            {
                //dent = false;
                RenderSpecialProperty(LN.pitfall, false);
            }
            else
            {
                RenderSpecialProperty(LN.rock, true);
                //steen = true;
            }
        }

        if (lava && mud)
        {
            RenderSpecialProperty(LN.mud, false);
            //mud = false;
        }
        
        if (dent && steen && !river)
        {
            RenderSpecialProperty(LN.pitfall, false);
            RenderSpecialProperty(LN.rock, false);
           // dent = false;
           // steen = false;
        }

        if (GridPosition.y < grid.GetUpperBound(1))
        {
            TileHandler above = grid[GridPosition.x, GridPosition.y + 1].GetComponent<TileHandler>();
            boundHandle(above, GridPosition, grid, rain);
        }
        else
        {
           TileHandler above=grid[GridPosition.x,GridPosition.y].GetComponent<TileHandler>();
           boundHandle(above, GridPosition, grid, rain);
        }
        
        #endregion
    }

    public void boundHandle(TileHandler above, Point GridPosition, GameObject[,] grid, bool rain) 
    {
        if (!rain)
        {
            if (above.river && above.steen && !lava)
            {
                RenderSpecialProperty(LN.mud, true);
                //this.mud = true;
            }

            else if (mud)
            {
                RenderSpecialProperty(LN.mud, false);
                //mud = false;
            }
        }
        TileHandler topleft = above;
        bool topleftb = false;
        int y = 0;
        if (grid[GridPosition.x, GridPosition.y] == above)
            y = GridPosition.y;
        else
            y = GridPosition.y + 1;
        if (GridPosition.x > 0)
        { topleft = grid[GridPosition.x - 1, y-1].GetComponent<TileHandler>(); }

        TileHandler topright = above;
        bool toprightb = false;
        if (GridPosition.x < grid.GetUpperBound(0) - 1)
        { topright = grid[GridPosition.x + 1, y-1].GetComponent<TileHandler>(); }
        if (this.dent && ((above.river && !above.steen) || (topleftb && topleft.river && !topleft.steen) || (toprightb && topright.river&&!topright.steen) || GridPosition.y == grid.GetUpperBound(1)))
        {
            RenderSpecialProperty(LN.river, true);
            //this.river = true;

        }
        else if (this.river && !rain)
        {
            if (dent && steen)
            {
                RenderSpecialProperty(LN.pitfall, false);
                RenderSpecialProperty(LN.rock, false);
            }
            else if (dent)
            {
                //RenderSpecialProperty(-2, true);
                RenderSpecialProperty(LN.pitfall, true);
            }
            else if (steen)
            {
                //RenderSpecialProperty(-2, true);
                RenderSpecialProperty(LN.rock, true);
                RenderSpecialProperty(LN.river, false);
            }
            else
                RenderSpecialProperty(LN.river, false);
            //this.river = false;
        }
        TileHandler left = grid[GridPosition.x, GridPosition.y].GetComponent<TileHandler>();
        bool leftb = false;
        if (GridPosition.x != 0)
        {
            left = grid[GridPosition.x - 1, GridPosition.y].GetComponent<TileHandler>();
            leftb = true;
        }
        TileHandler right = grid[GridPosition.x, GridPosition.y].GetComponent<TileHandler>();
        bool rightb = false;
        if (GridPosition.x != grid.GetUpperBound(0))
        {
            right = grid[GridPosition.x + 1, GridPosition.y].GetComponent<TileHandler>();
            rightb = true;
        }
        TileHandler below = grid[GridPosition.x, GridPosition.y].GetComponent<TileHandler>();
        bool belowb = false;
        if (GridPosition.y != 0)
        {
            below = grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>();
            belowb = true;
        }
        if (this.dent && ((above.lava && !above.steen) || (leftb && left.lava && !left.steen) || (rightb && right.lava && !right.steen) || (belowb && below.lava && !below.steen)))
        {
            RenderSpecialProperty(LN.lava, true);
            // this.lava = true;
        }

        if (TileType == BTT.bos && (!usingRandom || Random.Range(0, 100) < 80)&&((above.lava || above.fireBig) || (leftb && (left.lava || left.fireBig)) || (rightb && (right.lava || right.fireBig)) || (belowb && (below.lava || below.fireBig))))
        {
            RenderSpecialProperty(LN.fireSmall, true);
            //this.fireSmall = true;
        }
         
    }

    // Update is called once per frame
    void Update () {
	    
	}

    public void ChangeBasicTileType(BTT type)
    {
        ChoseBasicType(type);

    }
    public void RenderSpecialProperty(LN overlay, bool newValue)
    {

        switch (overlay)
        {
            case LN.rock:
                {
                    overlays[0].GetComponent<SpriteRenderer>().enabled = newValue;
                    steen = newValue;
                    break; 
                }
            case LN.lava:
                {
                    overlays[1].GetComponent<SpriteRenderer>().enabled = newValue;
                    lava = newValue;
                    break;
                }
            case LN.river:
                {
                    overlays[2].GetComponent<SpriteRenderer>().enabled = newValue;
                    river = newValue;
                    break;
                }
            case LN.mud:
                {
                    overlays[3].GetComponent<SpriteRenderer>().enabled = newValue;
                    mud = newValue;
                    break;
                }
            case LN.pitfall:
                {
                    overlays[4].GetComponent<SpriteRenderer>().enabled = newValue;
                    dent = newValue;
                    break;
                }
            case LN.fireSmall:
                {
                    overlays[5].GetComponent<SpriteRenderer>().enabled = newValue;
                    fireSmall = newValue;
                    break;
                }
            case LN.fireBig:
                {
                    overlays[6].GetComponent<SpriteRenderer>().enabled = newValue;
                    fireBig = newValue;
                    break;
                }
            case LN.volcano:
                {
                    overlays[7].GetComponent<SpriteRenderer>().enabled = newValue;
                    steen = newValue;
                    break;
                }
        }
    }

    public void ChangeSpecialProperty(int i, bool newValue)
    {
        switch(i)
        {
            case 0:
                {
                    steen = newValue;
                    break;
                }
            case 1:
        {
                    lava = newValue;
                    break;
        }
            case 2:
                {
                    river = newValue;
                    break;
                }
            case 3:
                {
                    mud = newValue;
                    break;
                }
            case 4:
                {
                    dent = newValue;
                    break;
                }
            case 5:
                {
                    fireSmall = newValue;
                    break;
                }
            case 6:
                {
                    fireBig = newValue;
                    break;
                }
        }
    }

    public bool GetSpecialProp(int i)
    {
        switch (i)
        {
            case 0:
                {
                    return steen;
                }
            case 1:
                {
                    return lava;

                }
            case 2:
                {
                    return river;
      
                }
            case 3:
                {
                    return mud;
        
                }
            case 4:
                {
                    return dent;
                }
            case 5:
                {
                    return fireSmall;

                }
            default:
                {
                    return fireBig;

                }
        }
    }
}
