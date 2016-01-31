using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandler : MonoBehaviour {
    int maxMana = 500;
    public static int manaCount;
    int manaregain = 30;
    float regainTimer =1;
    public List<GameObject> Units;
    public StartGrid startGrid;
    public int Player;
    public GameObject Unit1;
    public GameObject Unit2;
    public GameObject Unit3;
    public GameObject Tower;

	// Use this for initialization
	void Start () {
        Units = new List<GameObject>();
        manaCount = maxMana;
        CreateUnits();
	}

    void CreateUnits()
    {
        Vector2 startPosition = new Vector2(2+ Player * (startGrid.Width - 5), (startGrid.Height - 6) / 2 -5) * StartGrid.tileScale; ;
          //  Unit1.GetComponent<UnitController>().StartPosition = startPosition;
       //InfluenceController ic1 = 
        for (int i = 0; i < 5; i++)
        {
            //GameObject tempObj = Instantiate(Unit1);
            // tempObj.GetComponent<UnitController>().StartPosition = startPosition;
            var newUnit = (GameObject)Instantiate(Unit1,  startPosition + new Vector2(0, i), Quaternion.identity);
            newUnit.GetComponent<UnitController>().player = Player;
            //newUnit.GetComponent<UnitController>().influenceController = startGrid.GetComponent<InfluenceController>();

            Units.Add(newUnit);
            
        }
       startPosition = new Vector2(2 + Player * (startGrid.Width - 6), (startGrid.Height - 5) / 4 ) * StartGrid.tileScale;
        for (int i = 0; i < 5; i++)
        {
            GameObject tempObj = (GameObject)Instantiate(Unit2, startPosition + new Vector2(0, i), Quaternion.identity);
            tempObj.GetComponent<UnitController>().player = Player;
            //tempObj.GetComponent<UnitController>().influenceController = startGrid.GetComponent<InfluenceController>();

            Units.Add(tempObj);

        }

        startPosition = new Vector2(2 + Player * (startGrid.Width - 6), (startGrid.Height - 10)  ) * StartGrid.tileScale; 
        for (int i = 0; i < 5; i++)
        {
            GameObject tempObj = (GameObject)Instantiate(Unit3, startPosition + new Vector2(0, i), Quaternion.identity);
            tempObj.GetComponent<UnitController>().player = Player;
            //  tempObj.GetComponent<UnitController>().influenceController = startGrid.GetComponent<InfluenceController>();

            Units.Add(tempObj);

        }
        Tower = Instantiate(Tower);
        Tower.GetComponent<TowerHandler>().Position = new Point((int)((Player * (startGrid.Width - 1)*2) * StartGrid.tileScale), (int)(startGrid.Height*StartGrid.tileScale));
        Tower.GetComponent<TowerHandler>().player = Player;

    }

	// Update is called once per frame
	void Update () {
        regainTimer -= Time.deltaTime;
        if (regainTimer< 0 )
        {
            manaCount = Mathf.Min( manaCount + manaregain, maxMana);
            regainTimer = 1;
        }
	}
}
