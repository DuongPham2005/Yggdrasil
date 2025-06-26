using UnityEngine;

namespace Retro.ThirdPersonCharacter
{
    public class Orbit : MonoBehaviour
    {
        public Transform target;
        public float distance = 3f;
        public float rotationSpeed = 3f;
        public Vector3 targetOffcet;
        
        private Vector3 TargetPostion => target.position + targetOffcet;
        private bool isCursorLocked = true;

        private void Start()
        {
            LockCursor(true); // Khóa chuột khi vào game
        }

        private void Update()
        {
            // Nhấn giữ Alt để mở chuột
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                LockCursor(false);

            }
            else if (Input.GetKeyUp(KeyCode.LeftAlt))
            { 
                LockCursor(true);
        
            }
        }

        private void LateUpdate()
        {
            if (isCursorLocked)
            {
                float xAngle = Input.GetAxis("Mouse X") * rotationSpeed;
                float yAngle = -Input.GetAxis("Mouse Y") * rotationSpeed;

                Quaternion xRotation = Quaternion.AngleAxis(xAngle, target.up);
                Quaternion yRotation = Quaternion.AngleAxis(yAngle, target.right);
                transform.rotation *= xRotation * yRotation;
            }

            transform.position = transform.forward * (-distance) + TargetPostion;
            transform.LookAt(TargetPostion);
        }

        private void LockCursor(bool shouldLock)
        {
            isCursorLocked = shouldLock;
            Cursor.visible = !shouldLock;
            Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
