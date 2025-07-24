using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip stoneStepLoop;
    public AudioClip groundStepLoop;

    private CharacterController characterController;
    private string currentSurface = "";

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        bool isMoving = characterController != null && characterController.isGrounded && characterController.velocity.magnitude > 0.2f;

        if (isMoving)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
            {
                string surface = "Ground";
                if (hit.collider.CompareTag("Stone"))
                    surface = "Stone";
                else if (hit.collider.CompareTag("Ground"))
                    surface = "Ground";

                // Nếu đổi bề mặt hoặc chưa phát thì đổi clip và phát lại
                if (surface != currentSurface || !audioSource.isPlaying)
                {
                    currentSurface = surface;
                    if (surface == "Stone" && stoneStepLoop != null)
                        audioSource.clip = stoneStepLoop;
                    else if (surface == "Ground" && groundStepLoop != null)
                        audioSource.clip = groundStepLoop;
                    else
                        audioSource.clip = groundStepLoop;

                    audioSource.Play();
                }
            }
        }
        else
        {
            // Nếu player dừng lại thì dừng âm thanh
            if (audioSource.isPlaying)
                audioSource.Stop();
            currentSurface = "";
        }
    }
}
