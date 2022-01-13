using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    public class PlayerCamera : MonoBehaviour
    {

        [Header("Camera Settings")]
        public float smoothFollow = 10f;

        [Header("Collision Settings")]
        public LayerMask layersToDetectCollision;

        //Private variable to store the offset distance between the player and camera
        private Vector3 offset;            
        private Vector3 cameraDesiredPosition;
        private GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindWithTag("Player");
            offset = transform.position - player.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            cameraDesiredPosition = player.transform.position + offset;

            Ray ray = new Ray(transform.position, player.transform.position - cameraDesiredPosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 5, layersToDetectCollision))
            {
                // Give the hit point a bit of offset so the camera doesn't overlap with the hitpoint
                cameraDesiredPosition.z = hit.point.z - 0.05f;
            }
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, cameraDesiredPosition, smoothFollow * Time.fixedDeltaTime);
        }
    }

}
