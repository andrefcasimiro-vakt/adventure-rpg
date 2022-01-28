using UnityEngine;
using System.Collections;
using Cinemachine;

namespace AF
{

    public class ParrySystem : MonoBehaviour
    {
        public CinemachineVirtualCamera mainCamera;
        public CinemachineVirtualCamera parryCamera;

        public static ParrySystem instance;

        public bool parryingOngoing = false;

        Character invoker;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (invoker == null)
            {
                return;
            }

            if (invoker.receivingParryDamage)
            {
                invoker.TakeParryDamage(
                    GameObject.FindWithTag("Player").GetComponent<Player>().weaponGameObject.GetComponent<Hitbox>().weaponCriticalDamage,
                    invoker.parryPositionBloodFx.position
                );

                invoker.animator.SetBool("receivingParryDamage", false);
            }

            parryingOngoing = invoker.animator.GetBool("parryOngoing");

            if (parryingOngoing == false)
            {
                // Stop parry event
                Time.timeScale = 1f;
                mainCamera.gameObject.SetActive(true);
                parryCamera.gameObject.SetActive(false);
                this.invoker = null;
            }
        }

        public void Dispatch(Player player, Character target)
        {
            invoker = target;

            var lookPos = target.transform.position - player.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            player.transform.rotation = rotation;

            player.transform.position = target.parryPosition.position;

            player.GetComponent<Animator>().Play("Parry");
            invoker.animator.Play("Parried");
            invoker.animator.SetBool("parryOngoing", true);

            Time.timeScale = 0.7f;

            mainCamera.gameObject.SetActive(false);
            parryCamera.gameObject.SetActive(true);
        }
    }

}
