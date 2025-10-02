using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody2D rb;
    public Camera cam;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Vector2 mousePos;
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
    }
    
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        rb.rotation = angle;
    }
}
