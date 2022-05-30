using UnityEngine;
using System.Collections;

namespace AF
{

    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Healthbox))]
    [RequireComponent(typeof(EquipmentManager))]
    public class Character : MonoBehaviour
    {
        public readonly int hashDodging = Animator.StringToHash("Dodging");

        [HideInInspector] public Animator animator => GetComponent<Animator>();
        [HideInInspector] public Healthbox healthbox => GetComponent<Healthbox>();
        [HideInInspector] public EquipmentManager equipmentManager => GetComponent<EquipmentManager>();

        public string uuid;

        protected void Awake()
        {
            uuid = this.gameObject.name;
        }


        public bool IsDodging()
        {
            return animator.GetBool(hashDodging);
        }

    }
}
