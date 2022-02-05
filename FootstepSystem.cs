using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AF
{

    [System.Serializable]
    public class FootstepLayer
    {
        public string tagName;
        public AudioClip[] clips;
    }

    public class FootstepSystem : MonoBehaviour
    {

        public static FootstepSystem instance;

        public List<FootstepLayer> footstepLayers = new List<FootstepLayer>();

        Dictionary<string, FootstepLayer> layers = new Dictionary<string, FootstepLayer>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            foreach (FootstepLayer footstepLayer in footstepLayers)
            {
                layers.Add(footstepLayer.tagName, footstepLayer);
            }
        }

        public AudioClip GetFootstepClip(string groundTag)
        {
            AudioClip[] clips = layers[groundTag].clips;
            int clipDice = Random.Range(0, clips.Length);

            return clips[clipDice];
        }

    }

}