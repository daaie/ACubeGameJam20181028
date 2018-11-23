using UnityEngine;
using System.Collections;

 public class CameraFollow : MonoBehaviour
    {
        public Transform target;            // The position that that camera will be following.
        public float smoothing = 5f;        // The speed with which the camera will be following.


        public Vector3 offset = new Vector3(0,16,-16);

        void Start ()
        {
            if (target == null) return;
            // Calculate the initial offset.
            offset = transform.position - target.position;
            Debug.Log(offset);
            
        }


        void FixedUpdate()
        {
            if (target == null) return;
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }
