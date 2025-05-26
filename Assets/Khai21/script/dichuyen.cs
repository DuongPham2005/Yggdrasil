using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class dichuyen : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -25f;
    public float groundCheckDistance = 0.2f;

    private CharacterController controller;
    private Vector3 velocity;

    private bool isGrounded;
    private int jumpCount = 0;
    public int maxJumps = 2; // 1 nhảy thường + 1 nhảy giữa không

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Kiểm tra chạm đất bằng raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, controller.height / 2 + groundCheckDistance);

        // Nếu chạm đất, reset số lần nhảy và rơi nhẹ
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpCount = 0; // Reset double jump
        }

        // Nhận input WASD
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Xử lý nhảy: chỉ được nhảy nếu còn lượt
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
        }

        // Áp dụng trọng lực
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
