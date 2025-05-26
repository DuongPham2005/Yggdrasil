using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public NPCInteraction vachamnayno; // Script khác điều khiển việc bật chuột

    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float sensitivity = 3f;
    public float minY = -35f;
    public float maxY = 60f;

    private float rotX, rotY;

    [Header("UI Elements")]
    public GameObject[] uiElements;

    private bool userWantsMouse = false;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        rotX = angles.y;
        rotY = angles.x;

        UpdateCursorState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            userWantsMouse = !userWantsMouse;
            UpdateCursorState();
        }

        // Luôn cập nhật theo trạng thái từ script khác
        UpdateCursorState();
    }

    void LateUpdate()
    {
        if (AnyUIActive() || userWantsMouse || (vachamnayno != null && vachamnayno)) return;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotX += mouseX;
        rotY -= mouseY;
        rotY = Mathf.Clamp(rotY, minY, maxY);

        transform.rotation = Quaternion.Euler(rotY, rotX, 0);

        if (target)
        {
            transform.position = target.position + transform.rotation * offset;
        }
    }

    void UpdateCursorState()
    {
        if (AnyUIActive() || userWantsMouse || (vachamnayno != null && vachamnayno))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    bool AnyUIActive()
    {
        foreach (GameObject element in uiElements)
        {
            if (element != null && element.activeInHierarchy)
                return true;
        }
        return false;
    }
}
