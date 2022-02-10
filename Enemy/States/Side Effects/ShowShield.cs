using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    public class ShowShield : StateMachineBehaviour
    {
        Enemy enemy;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.TryGetComponent<Enemy>(out enemy);

            enemy.GetComponent<Inventory>().shield.SetActive(true);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            enemy.GetComponent<Inventory>().shield.SetActive(false);
        }
    }

}
