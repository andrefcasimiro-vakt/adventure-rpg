using UnityEngine;

namespace AF { 
    public class ChangeCameras : MonoBehaviour
    {
        public GameObject cameraToTransitionTo;

        private void Start()
        {
            GetComponent<MeshRenderer>().enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                CameraManager.instance.SwitchCamera(cameraToTransitionTo);
            }
        }
    }
}