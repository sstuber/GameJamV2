using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour
{
    // Use this for initialization
    public InfluenceController influenceController;
    public StartGrid sg;
    public Point currentTile;
    public Vector3 target;
    public int player;
    public PlayerHandler owner, enemy;
    public GameObject chasingEnemy;
    public float chaseRange;
    public float attackRange;
    public float speed;
    public float attackCooldown, maxAttackCooldown;
    public float normalDrag;
    private Quaternion targetDeathRotation;
    private Animator animator;
    public Vector3 dir;
    public float deadBodyTimer = 5f;

    public bool alive = true;
    public enum UnitType
    {
        scout,
        warrior,
        archer
    };

    public UnitType unitType;
    void Start()
    {
        int r = Random.Range(0, 2);
        if (r == 0)
        {
            targetDeathRotation = Quaternion.EulerAngles(0, 0, 90);
        }
        else {
            targetDeathRotation = Quaternion.EulerAngles(0, 0, 270);
        }
        transform.localScale *= StartGrid.tileScale;
        this.influenceController = GameObject.FindGameObjectWithTag("ic").GetComponent<InfluenceController>();
        this.sg = GameObject.FindGameObjectWithTag("sg").GetComponent<StartGrid>();
        var enemyGO = GameObject.FindGameObjectsWithTag("Player");
        if (enemyGO[0].GetComponent<PlayerHandler>().Player != player)
        {
            owner = enemyGO[1].GetComponent<PlayerHandler>();
            enemy = enemyGO[0].GetComponent<PlayerHandler>();
        }
        else {
            owner = enemyGO[0].GetComponent<PlayerHandler>();
            enemy = enemyGO[1].GetComponent<PlayerHandler>();
        }
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();

            if (attackCooldown > 0)
            {
                attackCooldown -= Time.deltaTime;
                rb.velocity *= 0.9f;
            }
            else {

                bool bayestenbool = false;

                GameObject closestEnemy = null;
                float distance = float.MaxValue;
                foreach (GameObject unit in enemy.Units)
                {
                    float currentDistance = (transform.position - unit.transform.position).magnitude;
                    if (currentDistance < distance)
                    {
                        distance = currentDistance;
                        closestEnemy = unit;
                    }
                }
                currentTile = StartGrid.PositionToGridIndex(transform.position);
                Vector3 dir = Vector3.zero;
                if (closestEnemy != null && distance < chaseRange)
                {
                    dir = (closestEnemy.transform.position - transform.position);
                    //ATTACK
                    print(distance);
                    if (distance < attackRange)
                    {
                        if (attackCooldown <= 0)
                        {
                            Attack(closestEnemy);
                        }
                    }
                    //CHARGE
                    else {
                        dir = (closestEnemy.transform.position - transform.position);
                    }
                }
                else {
                    Point targetTile = influenceController.GetBestTile(currentTile.x, currentTile.y, player);
                    target = StartGrid.GridIndexToPosition(targetTile.x, targetTile.y) + new Vector3(0.1f, 0.1f, 0);
                    dir = (target - transform.position);
                    if (currentTile.x == targetTile.x && currentTile.y == targetTile.y)
                    {
                        animator.speed = 0;
                        bayestenbool = true;
                    }
                }

                dir.z = 0;
                dir = dir.normalized;

                rb.AddForce(dir * speed);
              //  print("current tile" + currentTile.x +" "+ currentTile.y);
                switch (sg.Grid[currentTile.x, currentTile.y].GetComponent<TileHandler>().TileType)
                {
                    case BTT.bos:
                        rb.drag = normalDrag * 1.5f;
                        break;
                    case BTT.flat:
                        rb.drag = normalDrag;
                        break;
                    case BTT.plateau:
                        rb.drag = normalDrag *2f;
                        break;
                    default:
                        rb.drag = normalDrag;
                        break;
                }

                if (bayestenbool)
                {
                    rb.velocity *= 0.99f;
                }
                else {

                }

                if (dir.x > 0) animator.SetInteger("Direction", 3);
                if (dir.x < dir.y) animator.SetInteger("Direction", 2);
                if (dir.x < 0 && dir.y < dir.x) animator.SetInteger("Direction", 0);
                else if (dir.x < 0) animator.SetInteger("Direction", 1);
            }
        }
        else {
            deadBodyTimer -= Time.deltaTime;
            if (deadBodyTimer < 0)
                Destroy(this);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetDeathRotation, 0.1f);
        }
    }

    private void Attack(GameObject t)
    {
        attackCooldown = maxAttackCooldown;
        UnitController targetUnit = t.GetComponent<UnitController>();
        if (unitType == UnitType.archer)
        {
            RangedAttack(t);
        }
        else if (unitType == UnitType.scout)
        {
            if (targetUnit.unitType == UnitType.archer)
            {
                targetUnit.Die();
            }
        }
        else {
            if (targetUnit.unitType == UnitType.archer)
            {
                targetUnit.Die();
            }
            else if (targetUnit.unitType == UnitType.scout)
            {
                targetUnit.Die();
            }
        }
    }

    private void RangedAttack(GameObject t)
    {
        UnitController targetUnit = t.GetComponent<UnitController>();
        //knalluh


    }

    public void Die()
    {
        alive = false;
        owner.Units.Remove(gameObject);
        CircleCollider2D cc = transform.GetComponent<CircleCollider2D>();
        cc.enabled = false;
        animator.speed = 0;
    }
}
