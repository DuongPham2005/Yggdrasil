using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public GameObject pressFUI; // Kéo UI "Nhấn F để mở cửa" vào đây
    public float openDistance = 2f; // Khoảng cách để hiện UI
    public float openSpeed = 2f; // Tốc độ mở cửa
    private bool isPlayerNear = false;
    private bool isOpen = false;
    private Vector3 closedPosition;
    public Vector3 openOffset = new Vector3(0, -3, 0); // Kéo xuống 3 đơn vị

    // Thêm biến AudioSource
    public AudioSource audioSource;
    public AudioClip openSound; // Kéo file âm thanh vào đây trong Inspector

    void Start()
    {
        closedPosition = transform.position;
        if (pressFUI != null)
            pressFUI.SetActive(false);

        // Nếu chưa gán AudioSource, tự động lấy trên GameObject
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPlayerNear && !isOpen)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isOpen = true;
                if (pressFUI != null)
                    pressFUI.SetActive(false);

                // Phát âm thanh mở cửa
                if (audioSource != null && openSound != null)
                    audioSource.PlayOneShot(openSound);
            }
        }

        if (isOpen)
        {
            // Kéo cửa xuống
            transform.position = Vector3.MoveTowards(transform.position, closedPosition + openOffset, openSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            if (pressFUI != null && !isOpen)
                pressFUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (pressFUI != null)
                pressFUI.SetActive(false);
        }
    }
}
