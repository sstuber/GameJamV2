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
    public bool usingRandom;
    public GameObject overlaySprite;
    public Sprite[] OverlayArray;
    public GameObject fireOverlaySprite;
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
        fireOverlaySprite = Instantiate(overlaySprite);
        fireOverlaySprite.transform.localScale *= scale;
        fireOverlaySprite.transform.position = new Vector3(transform.position.x, transform.position.y, -0.7f);
        overlaySprite = Instantiate(overlaySprite);
        overlaySprite.transform.localScale *= scale;
        overlaySprite.transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);

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
             
                    RenderSpecialProperty(-2, false);
                    fireBig = false;
                
            }
        }
        if (TileType == BTT.bos)
        {
            
            if (fireSmall)
            {
                RenderSpecialProperty(6, true);

                //fireBig = true;
            }

            if (lava)
            {
               if (!usingRandom || Random.Range(0, 100) < 80)
                {
                    RenderSpecialProperty(5, true);
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
            river = false;
            mud = false;
            steen = false;
            fireSmall = false;
            fireBig = false;

            if (dent)
            {
                RenderSpecialProperty(0, true);
                //steen = true;
                dent = false;
                ChangeBasicTileType(BTT.flat);
            }
        }


        #endregion
        #region compressed logic

        if (rain||mud)
        {
            if (fireBig || fireSmall)
                RenderSpecialProperty(-1,false);
            fireSmall = false;
            fireBig = false;
            if (rain && !lava && dent&&TileType!=BTT.plateau)
            {
                RenderSpecialProperty(2, true);
                //river = true;
            }
            if (TileType != BTT.plateau)
            {
                RenderSpecialProperty(3, true);
               // mud = true;
            }
        }
        if (fireBig && fireSmall)
        {
            fireSmall = false;
        }

        if (river && !dent)
        {
            RenderSpecialProperty(-2,false);
            river = false;
        }
        if (river && steen&&TileType!=BTT.plateau)
        {
            
            RenderSpecialProperty(3, true);
            //mud = true;
        }
        if (lava && river)
        {
            RenderSpecialProperty(-2, false);
            lava = false;
            river = false;
            if (dent)
            {
                dent = false;
                RenderSpecialProperty(-2, false);
            }
            else
            {
                RenderSpecialProperty(0, true);
                //steen = true;
            }
        }

        if (lava && mud)
        {
            RenderSpecialProperty(-2, false);
            mud = false;
        }
        
        if (dent && steen && !river)
        {
            RenderSpecialProperty(-1, false);
            dent = false;
            steen = false;
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
                RenderSpecialProperty(3, true);
                //this.mud = true;
            }

            else if (this.mud)
            {
                RenderSpecialProperty(-2, false);
                this.mud = false;
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
            RenderSpecialProperty(2, true);
            //this.river = true;

        }
        else if (this.river && !rain)
        {
            if (dent && steen)
            {
                dent = false;
                steen = false;
                RenderSpecialProperty(-1, true);
            }
            else if (dent)
            {
                //RenderSpecialProperty(-2, true);
                RenderSpecialProperty(4, true);
            }
            else if (steen)
            {
                //RenderSpecialProperty(-2, true);
                RenderSpecialProperty(0, true);
            }
            else
                RenderSpecialProperty(-2, true);
            this.river = false;
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
            RenderSpecialProperty(1, true);
            // this.lava = true;
        }

        if (TileType == BTT.bos && (!usingRandom || Random.Range(0, 100) < 80)&&((above.lava || above.fireBig) || (leftb && (left.lava || left.fireBig)) || (rightb && (right.lava || right.fireBig)) || (belowb && (below.lava || below.fireBig))))
        {
            RenderSpecialProperty(5, true);
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
    public void RenderSpecialProperty(int i, bool newValue)
    {
        
        SpriteRenderer renderer = fireOverlaySprite.GetComponent<SpriteRenderer>();
        SpriteRenderer renderer2 = overlaySprite.GetComponent<SpriteRenderer>();
        
        if(i == -1)
        {
            renderer.sprite = null;
            return;
        }
        else if(i==-2)
        {
            renderer2.sprite =null;
            return;
        }
        else if (i == 3)
        {
            renderer2.sprite = OverlayArray[3];
            mud = true;
            return;
        }
        else if (i == 5)
        {
            renderer2.sprite = OverlayArray[5];
            fireSmall = true;
            return;

        }
        else if (i == 6)
        {

            renderer2.sprite = OverlayArray[6];
            fireBig = true;
            return;
        }
        else if (i == 7)
        {
            renderer.sprite = OverlayArray[i];
            ChangeSpecialProperty(5, newValue);
        }
        renderer.sprite = OverlayArray[i];
        ChangeSpecialProperty(i, newValue);
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
