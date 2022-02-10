using UnityEngine;
using System.Collections;

namespace AF
{

    [RequireComponent(typeof(Healthbox))]
    [RequireComponent(typeof(Inventory))]
    public class Character : MonoBehaviour
    {

        [HideInInspector] public Healthbox healthbox => GetComponent<Healthbox>();
        [HideInInspector] public Inventory inventory => GetComponent<Inventory>();

        public string uuid;

        protected void Awake()
        {
            uuid = this.gameObject.name;
        }

    }
}
