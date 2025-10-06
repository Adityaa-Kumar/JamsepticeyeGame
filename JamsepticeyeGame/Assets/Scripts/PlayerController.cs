using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;
    public Camera cam;
    public Enemy Enemy;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Vector2 mousePos;

    [Header("Possession Settings")]
    private Enemy possessedEnemy;
    private bool isPossessing = false;

    void Awake()
    {
        Enemy = GetComponent<Enemy>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isPossessing)
            {
                TryPossessEnemy();
            }
        }

        if (isPossessing && possessedEnemy != null)
        {
            ControlEnemy();
        }
    }

    void TryPossessEnemy()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 2f, LayerMask.GetMask("Enemy"));
        if (hit != null)
        {
            possessedEnemy = hit.GetComponent<Enemy>();
            hit.GetComponent<Enemy>().possessed = true;
            if (possessedEnemy != null)
            {
                isPossessing = true;
                gameObject.SetActive(false);
            }
        }
    }

    void ControlEnemy()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 moveDir = new Vector2(moveX, moveY).normalized;

        possessedEnemy.transform.position += (Vector3)(moveDir * possessedEnemy.moveSpeed * Time.deltaTime);

        if (moveDir != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            possessedEnemy.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        rb.rotation = angle;
    }
}
