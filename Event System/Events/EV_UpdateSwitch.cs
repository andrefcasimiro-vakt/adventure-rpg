using UnityEngine;
using System.Collections;

namespace AF
{

    public class EV_UpdateSwitch : EventBase
    {
        public Switches targetSwitch;
        public bool newValue;

        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(UpdateSwitch());
        }

        private IEnumerator UpdateSwitch()
        {
            SwitchManager.instance.UpdateSwitch(targetSwitch, newValue);

            // On switch changes, reevaluate all events in the scene
            Event[] events = FindObjectsOfType<Event>(true);
            foreach (Event e in events)
            {
                e.RefreshEventPages();
            }

            yield return null;
        }
    }

}