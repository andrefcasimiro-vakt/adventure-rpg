using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{

    public class DeactivateHitboxOnExit : StateMachineBehaviour
    {
        CombatManager combatManager;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.TryGetComponent<CombatManager>(out combatManager);

            if (combatManager != null)
            {
                combatManager.DeactivateHitbox();
            }
        }
    }

}
