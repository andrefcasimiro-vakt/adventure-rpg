using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    public class FootstepListener : MonoBehaviour
    {

        public AudioSource footstepAudioSource;

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

            AudioClip clip = FootstepSystem.instance.GetFootstepClip(groundTag);
            PlaySfx(clip);
        }

        private void PlaySfx(AudioClip clip)
        {
            float pitch = Random.Range(0.95f, 1.05f);
            footstepAudioSource.pitch = pitch;

            footstepAudioSource.PlayOneShot(clip);
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
