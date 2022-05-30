using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    public class FootstepListener : MonoBehaviour
    {

        public AudioSource footstepAudioSource;

        Player player;

        private void Awake()
        {
            TryGetComponent<Player>(out player);
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void PlayFootstep()
        {
            string groundTag = GetGroundTag();
            if (groundTag == null)
            {
                return;
            }

            if (player != null)
            {
                if (player.isAttacking)
                {
                    return;
                }
            }

            AudioClip clip = FootstepSystem.instance.GetFootstepClip(groundTag);
            Utils.PlaySfx(footstepAudioSource, clip);
        }

        private string GetGroundTag()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit))
            {
                if (hit.transform != null)
                {
                    return hit.transform.gameObject.tag;
                }
            }

            return null;
        }
    }
}
