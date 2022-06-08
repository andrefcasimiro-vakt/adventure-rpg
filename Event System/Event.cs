using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AF
{

    public class Event : MonoBehaviour
    {
        public string eventName;

        [HideInInspector]
        public EventPage[] eventPages;


        private void Awake()
        {
            EventPage[] eventPages = transform.GetComponentsInChildren<EventPage>(true);

            this.eventPages = eventPages;
        }

        private void Start()
        {
            RefreshEventPages();
        }

        public void RefreshEventPages()
        {
            // Deactivate all event pages
            foreach (EventPage evt in transform.GetComponentsInChildren<EventPage>(true))
            {
                evt.gameObject.SetActive(false);
            }

            // Find last event page where all conditions are met
            EventPage target = eventPages.Last(evt => {
                return evt.switchConditions.All(
                    // Evaluate if switch value equals the current value on the switch manager database
                    switchCondition => switchCondition.value == SwitchManager.instance.EvaluateSwitch(switchCondition.switchName)
                    );
            });

            Debug.Log("target: " + target);
        
            if (target == null && eventPages.Length > 0)
            {
                eventPages[0].gameObject.SetActive(true);
            }
            else if (target != null)
            {
                target.gameObject.SetActive(true);
            }
        }
    }

}
