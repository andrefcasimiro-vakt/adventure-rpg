using UnityEngine;
using System.Collections;

namespace AF
{
    [RequireComponent(typeof (Animator))]
    public class ParryManager : MonoBehaviour
    {
        public readonly int hashParrying = Animator.StringToHash("Parrying");

        Animator animator => GetComponent<Animator>();


        public void EnableParrying()
        {
            animator.SetBool(hashParrying, true);
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void DisableParrying()
        {
            animator.SetBool(hashParrying, false);
        }

        public bool IsParrying()
        {
            return animator.GetBool(hashParrying);
        }
    }
}
