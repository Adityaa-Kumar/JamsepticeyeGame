using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public Vector2 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 5f;
    public float moveSpeed = 2f;

    public float attackRange = 2f;
    public LayerMask playerLayer;
    bool playerInAttackRange;

    public bool possessed = false;

    public double suspicionMeter = 0;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);

        if (!playerInAttackRange)
        {
            Patrolling();
        }

        if (possessed)
        {
            GetComponent<PlayerController>().enabled = true;
            GetComponent<Enemy>().enabled = false;
        }
    }

    void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            Vector2 direction = (walkPoint - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

            transform.position = Vector2.MoveTowards(transform.position, walkPoint, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, walkPoint) < 1f)
            {
                walkPointSet = false;
            }
        }
    }

    void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomY = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector2(transform.position.x + randomX, transform.position.y + randomY);
        walkPointSet = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
