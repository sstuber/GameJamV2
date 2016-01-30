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
    //public StartGrid scene;
    Sprite sprite;
    public BTT TileType;
    // Use this for initialization
    public bool[] SpecialProps;
    bool steen = false;        //   0
    bool lava = false;         //     1
    bool river = false;         //    2
    bool mud = false;         //    3
    bool dent = false;   //   5
    bool fireBig = false;
    public bool fireSmall = false; //  6
    
   


    void Start () {
        this.transform.localScale *= scale;
        sprite = GetComponent<SpriteRenderer>().sprite;
        SpecialProps = new bool[7];
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

    public void checkAllValues(Point GridPosition,  GameObject[,] grid, bool rain )
    {
        #region flat 
        if (TileType == BTT.flat)
        {
            if (rain)
            {
                fireSmall = false;
                fireBig = false;

                if (!lava && dent)
                {
                    river = true;

                }
                mud = true;

            }
            else
            {
                if (mud && fireSmall)
                {
                    fireSmall = false;
                }

                if (mud && fireBig)
                {
                    fireBig = false;
                }

                if (fireBig && fireSmall)
                {
                    fireSmall = false;
                }


            }
            if (river && !dent)
            {
                river = false;
            }


            if (river && steen)
            {
                mud = true;
            }
            if (lava && river)
            {
                lava = false;
                river = false;
                if (dent)
                {
                    dent = false;
                }
                else
                {
                    steen = true;
                }
            }

            if (lava && mud)
            {
                mud = false;
            }





            if (dent && steen && !river)
            {
                dent = false;
                steen = false;
            }


            // eigenschappen voor flatland als er bepaalde dingen naast liggen

            if (GridPosition.y > 0)
            {
                if (!rain)
                {
                    if (grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().river && grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().steen && !lava)
                    {
                        this.mud = true;
                    }

                    else if (this.mud)
                    {
                        this.mud = false;
                    }
                }
                if (this.dent && grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().river && !grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().steen)
                {
                    this.river = true;

                }
                else if (this.river && !rain)
                {
                    this.river = false;
                }

                if (this.dent && grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().lava && !grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().steen)
                {
                    this.lava = true;

                }

            }

        }
        #endregion

        #region ForestTile
        if (TileType == BTT.bos)
        {
            if (lava)
            {
                fireSmall = true;
            }
            if (fireSmall)
            {
                fireBig = true;
            }

            if (rain)
            {
                fireSmall = false;
                fireBig = false;

                if (!lava && dent)
                {
                    river = true;

                }
                mud = true;

            }
            else
            {
                if (mud && fireSmall)
                {
                    fireSmall = false;
                }

                if (mud && fireBig)
                {
                    fireBig = false;
                }

                if (fireBig && fireSmall)
                {
                    fireSmall = false;
                }


            }
            if (river && !dent)
            {
                river = false;
            }


            if (river && steen)
            {
                mud = true;
            }
            if (lava && river)
            {
                lava = false;
                river = false;
                if (dent)
                {
                    dent = false;
                }
                else
                {
                    steen = true;
                }
            }

            if (lava && mud)
            {
                mud = false;
            }





            if (dent && steen && !river)
            {
                dent = false;
                steen = false;
            }


            if (GridPosition.y > 0)
            {
                if (!rain)
                {
                    if (grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().river && grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().steen && !lava)
                    {
                        this.mud = true;
                    }

                    else if (this.mud)
                    {
                        this.mud = false;
                    }
                }
                if (this.dent && grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().river && !grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().steen)
                {
                    this.river = true;

                }
                else if (this.river && !rain)
                {
                    this.river = false;
                }

                if (this.dent && grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().lava && !grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().steen)
                {
                    this.lava = true;

                }
                if (grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().lava || grid[GridPosition.x, GridPosition.y - 1].GetComponent<TileHandler>().fireBig)
                {
                    this.fireSmall = true;
                }
            }

            if (rain)
            {
                fireSmall = false;
                fireBig = false;
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
                steen = true;
                dent = false;
                ChangeBasicTileType(BTT.flat);
            }
        }


        #endregion
    }


    // Update is called once per frame
    void Update () {
	    
	}

    public void ChangeBasicTileType(BTT type)
    {
        ChoseBasicType(type);

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
