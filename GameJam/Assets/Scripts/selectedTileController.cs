using UnityEngine;
using System.Collections;

public class selectedTileController : MonoBehaviour {
    public GameObject cornerPrefab;
    public float offset = 1f;
    float localScale;
    public float targetOffset = 1f;
    public float wiggleTimer = 0.5f;
    public float wiggleSize = 1.25f;
    [HideInInspector]
    public float spellSize = 1f;
    public Color c;
    public GameObject[] corners;
    public GameObject middle;

	// Use this for initialization
	void Start () {
        localScale = StartGrid.tileScale * 0.5f;
        middle = (GameObject)Instantiate(middle, transform.position + new Vector3(0,0,-5), Quaternion.identity);
        middle.transform.localScale *= StartGrid.tileScale * 1.25f;
        SpriteRenderer mr = middle.GetComponent<SpriteRenderer>();
        mr.color = c * 0.25f;
        corners = new GameObject[4];
        for (int i = 0; i < 4; ++i) {
            corners[i] = (GameObject)Instantiate(cornerPrefab);
            corners[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, i * 90 + 90));
            SpriteRenderer sr = corners[i].GetComponent<SpriteRenderer>();
            sr.color = c * 0.8f;
            corners[i].transform.localScale *= localScale;
        }
        offset = localScale;
        targetOffset = offset;
        corners[0].transform.position = new Vector3(-offset, -offset, -5);
        corners[1].transform.position = new Vector3(offset, -offset, - 5);
        corners[2].transform.position = new Vector3(offset, offset, -5);
        corners[3].transform.position = new Vector3(-offset, offset, -5); 
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.frameCount % 150 == 0) {
            StartCoroutine(Wiggle());
        }

        offset += (targetOffset - offset) * 0.25f;

        float selectionOffset = offset * spellSize;// *1.4f;
        print(spellSize + " " + offset + " " + selectionOffset);
        corners[0].transform.position = new Vector3(-selectionOffset, -selectionOffset, -5) + transform.position;
        corners[1].transform.position = new Vector3(selectionOffset, -selectionOffset, -5) + transform.position;
        corners[2].transform.position = new Vector3(selectionOffset, selectionOffset, -5) + transform.position;
        corners[3].transform.position = new Vector3(-selectionOffset, selectionOffset, -5) + transform.position;
        middle.transform.position = transform.position + new Vector3(0, 0, -5);
	}

    IEnumerator Wiggle() {
        targetOffset = localScale * 1.25f;
        yield return new WaitForSeconds(0.5f);
        targetOffset = localScale;
    }
}
