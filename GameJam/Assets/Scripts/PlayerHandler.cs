using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHandler : MonoBehaviour {
    int maxMana = 500;
    public static int manaCount;
    int manaregain = 30;
    float regainTimer =1;
    List<GameObject>[] UnitCounts;
    public StartGrid startGrid;
    public int Player;
    public GameObject Unit1;
    public GameObject Unit2;
    public GameObject Unit3;

	// Use this for initialization
	void Start () {
        UnitCounts = new List<GameObject>[3];
        for (int i = 0; i < UnitCounts.Length; i++)
            UnitCounts[i] = new List<GameObject>();
        manaCount = maxMana;
        CreateUnits();
	}

    void CreateUnits()
    {
        Vector2 startPosition = new Vector2(2+ Player * (startGrid.Width - 3), (startGrid.Height - 1) / 2);
            Unit1.GetComponent<UnitController>().StartPosition = startPosition;
       //InfluenceController ic1 = 
        for (int i = 0; i < 5; i++)
        {
            //GameObject tempObj = Instantiate(Unit1);
            // tempObj.GetComponent<UnitController>().StartPosition = startPosition;
            print(startPosition);
            var newUnit = (GameObject)Instantiate(Unit1,  startPosition + new Vector2(0, i), Quaternion.identity);
            newUnit.GetComponent<UnitController>().player = Player;
            //newUnit.GetComponent<UnitController>().influenceController = startGrid.GetComponent<InfluenceController>();

            UnitCounts[0].Add(Instantiate(newUnit));
            
        }
       startPosition = new Vector2(2 + Player * (startGrid.Width - 3), (startGrid.Height - 1) / 4);
        for (int i = 0; i < 5; i++)
        {
            GameObject tempObj = Instantiate(Unit2);
            tempObj.GetComponent<UnitController>().StartPosition = startPosition + new Vector2(0, i);
            tempObj.GetComponent<UnitController>().player = Player;
            //tempObj.GetComponent<UnitController>().influenceController = startGrid.GetComponent<InfluenceController>();

            UnitCounts[1].Add(tempObj);

        }

        startPosition = new Vector2(2 + Player * (startGrid.Width - 3), (startGrid.Height - 5)  );
        for (int i = 0; i < 5; i++)
        {
            GameObject tempObj = Instantiate(Unit3);
            tempObj.GetComponent<UnitController>().StartPosition = startPosition + new Vector2(0,i);
            tempObj.GetComponent<UnitController>().player = Player;
          //  tempObj.GetComponent<UnitController>().influenceController = startGrid.GetComponent<InfluenceController>();

            UnitCounts[2].Add(tempObj);

        }

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
