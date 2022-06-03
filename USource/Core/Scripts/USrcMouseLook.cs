using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USource
{
    public class USrcMouseLook : MonoBehaviour
    {
        float rotSpeed = 250;
        [SerializeField] Transform body;
        float vertRot = 0;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update()
        {
            float mouseY = Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
            float mouseX = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;

            vertRot -= mouseY;
            vertRot = Mathf.Clamp(vertRot, -90, 90);

            transform.localRotation = Quaternion.Euler(vertRot, 0, 0);
            body.Rotate(Vector3.up * mouseX);
        }
    }
}