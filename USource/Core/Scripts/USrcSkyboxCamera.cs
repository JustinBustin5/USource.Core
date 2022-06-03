using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace USource
{
    public class USrcSkyboxCamera : MonoBehaviour
    {
        [SerializeField] float scale = 16;

        // Update is called once per frame
        void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
            transform.localPosition = Camera.main.transform.position / scale;
        }
    }
}