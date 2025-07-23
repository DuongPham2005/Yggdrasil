using System;
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
    public int maxJumps = 2;
    
    //run
    private int runCount = 0;
    public int maxRun = 1;

    // Tham chiếu tới UIheart
    public UIheart uiHeart;
    public int damageAmount = 1;

    // (Tùy chọn) Thời gian miễn nhiễm
    public float invincibleTime = 1f;
    private float lastHitTime = -999f;

    //mana
    public UIMana uiMana;
    public float manaUsePerJump = 20f;
    public float manaUserPerRun = 3f;
    
    //PlayerHealth
    public PlayerHealth Playerhealth;



    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, controller.height / 2 + groundCheckDistance);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpCount = 0;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps && uiMana.currentMana >= manaUsePerJump)
        {
            
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;
            uiMana.UseMana(manaUsePerJump);

        }
        //if(Input.GetButton("Run") && runCount < maxRun && uiMana.currentMana => manaUserPerRun)
        //{
        //    velocity.y = MathF.Sqrt(x * x + z * z);
        //}

        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    }
    


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))

        {
            Playerhealth.TakeDamage(20);
            uiHeart.TakeDamage(1);
        }
    }
}


