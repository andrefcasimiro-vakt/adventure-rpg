using UnityEngine;
using System.Collections;

namespace AF
{

    public class EV_DisplayMessage : EventBase
    {
        public string actorName;
        public string message;

        public override IEnumerator Dispatch()
        {
            yield return StartCoroutine(DialogueUI.instance.ShowDialogue(actorName, message));
        }
    }

}
