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

        public void Dispatch(Player player, EnemyMelee target)
        {
            invoker = target;

            player.transform.position = target.parryPosition.position;
            player.transform.rotation = Quaternion.LookRotation(target.parryPosition.position - player.transform.position);

            player.GetComponent<Animator>().Play("Parry");
            invoker.animator.Play("Parried");
            invoker.animator.SetBool("parryOngoing", true);

            Time.timeScale = 0.7f;

            mainCamera.gameObject.SetActive(false);
            parryCamera.gameObject.SetActive(true);
        }
    }

}