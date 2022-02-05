using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF {

    public class DisableHitboxs : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Player player = animator.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.DeactivateHitbox();
                return;
            }

            Character character = animator.gameObject.GetComponent<Character>();
            if (character != null)
            {
                character.DeactivateHitbox();
            }
        }
    }

}