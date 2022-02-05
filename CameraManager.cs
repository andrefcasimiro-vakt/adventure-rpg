using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF { 
    public class CameraManager : MonoBehaviour
    {
        public GameObject currentCamera;
        public GameObject previousCamera;

        public static CameraManager instance;

        private void Awake()
        {
            instance = this;
        }

        public void SwitchCamera(GameObject newCamera)
        {
            this.previousCamera = this.currentCamera;
            this.currentCamera = newCamera;

            this.previousCamera.SetActive(false);
            this.currentCamera.SetActive(true);
        }

        public void SwitchToPreviousCamera()
        {
            GameObject targetCamera = this.previousCamera;
            this.previousCamera = this.currentCamera;
            this.currentCamera = targetCamera;

            this.previousCamera.SetActive(false);
            this.currentCamera.SetActive(true);
        }


    }
}