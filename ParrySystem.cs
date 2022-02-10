using UnityEngine;
using System.Collections;
using Cinemachine;

namespace AF
{

    public class ParrySystem : MonoBehaviour
    {
        public CinemachineVirtualCamera parryCamera;

        public static ParrySystem instance;

        public bool parryingOngoing = false;

        public AudioClip parrySfx;

        Enemy invoker;

        Player player;

        private void Awake()
        {
            instance = this;

            player = GameObject.FindWithTag("Player").GetComponent<Player>();
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

            //if (invoker.receivingParryDamage)
            //{
            //    invoker.TakeParryDamage(player.weapon.criticalAttackPower,
            //        invoker.parryPositionBloodFx.position
            //    );

            //    invoker.animator.SetBool("receivingParryDamage", false);
            //}

            //parryingOngoing = invoker.animator.GetBool("parryOngoing");

            if (parryingOngoing == false)
            {
                // Stop parry event
                Time.timeScale = 1f;

                CameraManager.instance.SwitchToPreviousCamera();

                this.invoker = null;

            }
        }

        public void Dispatch(Player player, Enemy target)
        {
            //invoker = target;
            //invoker.animator.SetBool("parryOngoing", true);

            //invoker.PlaySfx(invoker.combatAudiosource, parrySfx);

            //var lookPos = target.transform.position - player.transform.position;
            //lookPos.y = 0;
            //var rotation = Quaternion.LookRotation(lookPos);
            //player.transform.rotation = rotation;

            //player.transform.position = target.parryPosition.position;

            //player.GetComponent<Animator>().Play("Parry");
            //invoker.animator.Play("Parried");

            Time.timeScale = 0.7f;

            CameraManager.instance.SwitchCamera(this.parryCamera.gameObject);
        }
    }

}
