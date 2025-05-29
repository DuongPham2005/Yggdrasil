using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Reference to NPC Script")]
    public NPCInteraction vachamnayno; // Script điều khiển chuột từ NPC hoặc va chạm

    [Header("Camera Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -4);
    public float sensitivity = 3f;
    public float minY = -35f;
    public float maxY = 60f;

    private float rotX, rotY;

    [Header("UI Panels (con của Canvas)")]
    public GameObject[] uiElements; // Các panel UI cần kiểm tra

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
        // Toggle bằng ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            userWantsMouse = !userWantsMouse;
        }

        // Luôn cập nhật trạng thái chuột mỗi frame
        UpdateCursorState();
    }

    void LateUpdate()
    {
        if (ShouldShowMouse()) return; // Nếu cần hiện chuột thì không điều khiển camera

        // Điều khiển camera góc nhìn thứ 3
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
        if (ShouldShowMouse())
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

    // Điều kiện để hiện chuột
    bool ShouldShowMouse()
    {
        return AnyUIActive() || userWantsMouse || (vachamnayno != null && vachamnayno.vachamnayno);
    }

    // Kiểm tra các panel trong Canvas có đang bật không
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
