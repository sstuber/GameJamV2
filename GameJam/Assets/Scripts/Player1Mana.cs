using UnityEngine;
using System.Collections;

public class Player1Mana : MonoBehaviour {

    public float max_Mana = 100f;
    public float cur_Mana = 0f;
    public GameObject manaBar;
    public float ManaRegen;

    // Use this for initialization
    void Start()
    {

        cur_Mana = max_Mana;
        InvokeRepeating("addmana", 2f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void addmana()
    {
        if (cur_Mana < max_Mana)
        {
            cur_Mana += ManaRegen;
        }
        float calc_Mana = cur_Mana / max_Mana;
        SetManaBar(calc_Mana);
    }

    public void decreasemana(float manaAmount)
    {
        cur_Mana -= manaAmount;
        float calc_Mana = cur_Mana / max_Mana;
        SetManaBar(calc_Mana);
    }

    public void SetManaBar(float myMana)
    {
        //myHealth getal tussen 0-1
        manaBar.transform.localScale = new Vector3(myMana, manaBar.transform.localScale.y, manaBar.transform.localScale.z);
    }
}