using UnityEngine;

namespace Core
{
    public class PlayerLookingDirection : MonoBehaviour
    {
        [SerializeField] private Camera myCamera;

        public float mouseSensitivity = 100f;

        private float xRotation = 0f;

        private void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); 

            myCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}
