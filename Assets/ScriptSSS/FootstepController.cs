using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip stoneStepLoop;
    public AudioClip groundStepLoop;

    private CharacterController characterController;
    private string currentSurface = "";
    private float stepTimer = 0f;
    public float stepInterval = 0.4f; // Thời gian giữa các bước chân (giây)

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        audioSource.loop = false; // Không lặp lại âm thanh
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        bool isMoving = characterController != null && characterController.isGrounded && characterController.velocity.magnitude > 0.2f;

        if (isMoving)
        {
            stepTimer += Time.deltaTime;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
            {
                string surface = "Ground";
                if (hit.collider.CompareTag("Stone"))
                    surface = "Stone";
                else if (hit.collider.CompareTag("Ground"))
                    surface = "Ground";

                if (surface != currentSurface)
                {
                    currentSurface = surface;
                }

                if (stepTimer >= stepInterval)
                {
                    if (surface == "Stone" && stoneStepLoop != null)
                        audioSource.PlayOneShot(stoneStepLoop);
                    else if (surface == "Ground" && groundStepLoop != null)
                        audioSource.PlayOneShot(groundStepLoop);
                    else
                        audioSource.PlayOneShot(groundStepLoop);
                    stepTimer = 0f;
                }
            }
        }
        else
        {
            stepTimer = stepInterval; // Reset để phát ngay khi di chuyển lại
            currentSurface = "";
            audioSource.Stop(); // Dừng mọi âm thanh đang phát
        }
    }
}
