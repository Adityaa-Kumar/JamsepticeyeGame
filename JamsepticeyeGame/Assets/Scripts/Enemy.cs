using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;

    public float attackRange = 2f;
    public LayerMask playerLayer;
    bool playerInAttackRange;

    public bool possessed = false;
    public bool canMove = true;

    public float suspicionMeter = 0;

    

    public Vector2 pointA;
    public Vector2 pointB;
    private Vector2 targetPoint;

    void Start()
    {
        player = GameObject.Find("Player").transform;

        targetPoint = pointA;
    }

    void Update()
    {
        playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);

        if (!playerInAttackRange && !possessed)
        {
            Patrolling();
        }

        if (possessed)
        {
            GetComponent<PlayerController>().enabled = true;
            GetComponent<Enemy>().enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Player");

            Collider2D hit = Physics2D.OverlapCircle(transform.position, 5f, LayerMask.GetMask("Enemy"));
            if (hit != null)
            {
                hit.GetComponent<Enemy>().suspicionMeter++;
                StartCoroutine(Surprised());
            }
        }
    }

    void Patrolling()
    {
        if (!canMove) return;

        Vector2 direction = (targetPoint - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        transform.position = Vector2.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint) < 0.2f)
        {
            // Switch target when reaching one point
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }
    }

    IEnumerator Surprised()
    {
        canMove = false;
        yield return new WaitForSeconds(2f);

    }

    void AttackPlayer(){
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw patrol points in scene view
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(pointA, 0.1f);
        Gizmos.DrawSphere(pointB, 0.1f);
    }
}
