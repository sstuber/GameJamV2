using UnityEngine;
using System.Collections;

public class FloatingGem : MonoBehaviour {

    Vector2 GemMoveSpeed;
    public GameObject Gem;
    public float UpperLimit;
    public float BottomLimit;
    bool MoveGemUp;
    bool MoveGemDown;
    public float Speed;

	// Use this for initialization
	void Start () {

        GemMoveSpeed = new Vector2(0,Speed);
        MoveGemUp = true;
       
      
	}
	
	// Update is called once per frame
	void Update () {

        if (Gem.GetComponent<Rigidbody2D>().position.y >= UpperLimit)
        {
            MoveGemUp = false;
            MoveGemDown = true;
        }

        if (Gem.GetComponent<Rigidbody2D>().position.y <= BottomLimit)
        {
            MoveGemUp = true;
            MoveGemDown = false;
        }


        if (MoveGemUp)
        {
            Gem.GetComponent<Rigidbody2D>().position += GemMoveSpeed;
        }

        if (MoveGemDown)
        {
            Gem.GetComponent<Rigidbody2D>().position -= GemMoveSpeed;
        }

	}

    
}
