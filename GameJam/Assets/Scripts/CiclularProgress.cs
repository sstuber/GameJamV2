using UnityEngine;
using System.Collections;

public class CiclularProgress : MonoBehaviour {
	
	public int ManaRegen = 20;
    public float i = 0;
	
	// Use this for initialization
	void Start () {
		//Use this to Start progress
        i = 1;
        gameObject.GetComponent<Renderer>().material.SetFloat("_Progress", i);
	}

    void Update()
    {
        StartCoroutine(RadialProgress(ManaRegen));
       // Debug.Log(i);
        if (Input.GetKeyDown(KeyCode.F))
        {
            i -= 0.2f;
            Debug.Log("Decrease Mana");
            
        }

        if (i >= 1) i = 1;
    }

	IEnumerator RadialProgress(float Regen)
	{
		float rate = 1 / Regen;

        if (i < 1)
        {
            i += Time.deltaTime * rate;
            gameObject.GetComponent<Renderer>().material.SetFloat("_Progress", i);



            yield return 0;
        }


	}
}