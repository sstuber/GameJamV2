using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour {

    Vector3 previousPos;
    public int player;
    public float yDistance;
	// Use this for initialization
	void Start () {
        previousPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        previousPos = transform.position;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.velocity.y < -10f)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color *= 0.9f;
        }
        if (rb.velocity.y < -15f) Destroy(gameObject);

        Vector3 dir = transform.position + new Vector3(rb.velocity.x, rb.velocity.y, 0) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float xScale = 1f;
        var absVelo = Mathf.Abs(rb.velocity.y);

        if(absVelo < 5f)
        {
            xScale = absVelo / 5f;
        }

        var tmpD = yDistance / 3;

        xScale = tmpD * xScale + (1 - tmpD);

        transform.localScale = new Vector3(Mathf.Min(1, xScale), transform.localScale.y, transform.localScale.z);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.velocity.y > -5f) return;
        var go = other.gameObject;
        if (go.tag != "Unit") return;
        var unit = go.GetComponent<UnitController>();
        if (unit.player != player) {
            Destroy(gameObject);
            unit.Die();
        }

    }
}
