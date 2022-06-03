using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USource
{
    public class USrcFPSController : MonoBehaviour
    {
        CharacterController cc;
        float speed = 3;
        [SerializeField] Transform cam;
        float negY = 0;
        [SerializeField] bool headBobbing = true;
        float startTime;
        float lastSine;
        bool isGrounded = true;
        [SerializeField] LayerMask groundLayer;
        bool crouched;
        bool crouchedLastFrame;

        private void Start()
        {
            cc = gameObject.GetComponent<CharacterController>();
            startTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            float checkSpherePos = cc.height == 2 ? -0.6f : -0.1f;
            isGrounded = Physics.CheckSphere(transform.position + new Vector3(0, checkSpherePos), 0.5f, groundLayer);
            crouched = Input.GetKeyDown(KeyCode.LeftControl)
                && (!crouched ||
                (crouched && !Physics.CheckSphere(transform.position + new Vector3(0, 1), 0.0001f, groundLayer)))
                ? !crouched : crouched;
            if (crouched != crouchedLastFrame)
            {
                if (crouched)
                    Teleport(transform.position - new Vector3(0, 0.5f, 0), transform.rotation);
                else
                    Teleport(transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
            }
            
            speed = Input.GetKey(KeyCode.LeftShift) ? 6 : 3;
            cc.height = crouched ? 1 : 2;

            float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float z = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            if (isGrounded) negY = 0;

            if (Mathf.Sin((Time.time - startTime) * speed * 4) <= -0.99 && lastSine > -0.99)
            {
                if ((x != 0 || z != 0) && cc.enabled && isGrounded)
                {
                    Footstep();
                }
            }
            lastSine = Mathf.Sin((Time.time - startTime) * speed * 4);

            if ((x != 0 || z != 0) && cc.enabled)
            {
                if (headBobbing && isGrounded) cam.localPosition = new Vector3(0, CamBaseHeight() + (Mathf.Sin((Time.time - startTime) * speed * 4) / 10), 0);
                else cam.localPosition = new Vector3(0, CamBaseHeight(), 0);
            }
            else cam.localPosition = new Vector3(0, CamBaseHeight(), 0);

            if (cc.enabled && isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                negY = -6;
            }

            if (cc.enabled)
            {
                cc.Move(transform.right * x + -transform.up * negY * Time.deltaTime + transform.forward * z);
            }

            crouchedLastFrame = crouched;
        }
        private void FixedUpdate()
        {
            if (!isGrounded) negY += 0.3f;
        }

        public void Teleport(Vector3 newPos, Quaternion newRot)
        {
            cc.enabled = false;
            transform.position = newPos;
            transform.rotation = newRot;
            cc.enabled = true;
        }

        public virtual void Footstep() { }

        float CamBaseHeight()
        {
            return cc.height == 2 ? 0.575f : 0f;
        }
    }
}