using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF { 
    public class CameraManager : MonoBehaviour, ISaveable
    {
        public GameObject currentCamera;
        public GameObject previousCamera;

        public static CameraManager instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }

            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            SaveSystem.instance.OnGameLoad += OnGameLoaded;
        }

        private void Update()
        {
            if (currentCamera == null)
            {
                GameObject playerCamera = GameObject.Find("Player Camera");
                if (playerCamera != null)
                {
                    Cinemachine.CinemachineVirtualCamera virtualCamera = playerCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>();

                    if (virtualCamera != null)
                    {
                        GameObject player = FindObjectOfType<Player>(true).gameObject;

                        if (player != null)
                        {
                            virtualCamera.m_Follow = player.transform;
                            virtualCamera.m_LookAt = player.transform;

                            SwitchCamera(playerCamera);
                        }
                    }

                }
            }
        }

        public void SwitchCamera(GameObject newCamera)
        {
            if (this.currentCamera != null)
            {
                this.previousCamera = this.currentCamera;
            }

            this.currentCamera = newCamera;

            if (this.previousCamera != null)
            {
                this.previousCamera.SetActive(false);
            }

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


        public void OnGameLoaded(GameData gameData)
        {
            foreach (Transform childCameraTransform in transform)
            {
                SwitchCamera(currentCamera);

                if (childCameraTransform.gameObject.name == gameData.activeCameraName)
                {
                    SwitchCamera(childCameraTransform.gameObject);
                }
            }
        }


    }
}