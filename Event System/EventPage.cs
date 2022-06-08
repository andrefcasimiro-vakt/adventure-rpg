using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public enum EventTrigger
    {
        AUTOMATIC,
        ON_KEY_PRESS,
        ON_PLAYER_TOUCH,
    }


    public class EventPage : InputListener
    {
        // EDITOR
        public EventTrigger eventTrigger = EventTrigger.ON_KEY_PRESS;

        public Switch[] switchConditions;

        [HideInInspector]
        public Event eventParent;

        [HideInInspector]
        public List<EventBase> events = new List<EventBase>();

        private bool isRunning = false;

        private bool canInteract = false;

        private void Awake()
        {
            this.eventParent = this.transform.GetComponentInParent<Event>();

            EventBase[] gatheredEvents = GetComponents<EventBase>();

            this.events.Clear();
            foreach (EventBase ev in gatheredEvents)
            {
                events.Add(ev);
            }
        }

        public IEnumerator DispatchEvents()
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.MarkAsBusy();
            }

            isRunning = true;

            foreach (EventBase ev in events)
            {
                if (ev != null)
                {
                    yield return StartCoroutine(ev.Dispatch());
                }
            }

            if (player != null)
            {
                player.MarkAsActive();
            }

            yield return new WaitForSeconds(1f);

            isRunning = false;
        }

        public bool IsRunning()
        {
            return isRunning;
        }


        // EVENT TRIGGERS

        private void OnTriggerStay(Collider other)
        {
            if (canInteract == false)
            {
                return;
            }

            if (other.gameObject.tag == "Player")
            {
                if (isRunning)
                {
                    return;
                }

                if (hasPressedConfirmButton)
                {
                    StartCoroutine(DispatchEvents());
                }
            }
        }


        private void OnEnable()
        {
            base.OnEnable();

            canInteract = false;

            StartCoroutine(AllowEventInteraction());
        }

        IEnumerator AllowEventInteraction()
        {
            yield return new WaitForSeconds(1f);

            canInteract = true;
        }


        private void OnDisable()
        {
            base.OnDisable();

            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.MarkAsActive();
            }

            canInteract = false;
        }

        // UTILS

        // By Default, disable combat when player is near an event
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                PlayerCombatManager playerCombatManager = other.GetComponent<PlayerCombatManager>();
                playerCombatManager.DisableCombat();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                PlayerCombatManager playerCombatManager = other.GetComponent<PlayerCombatManager>();
                playerCombatManager.EnableCombat();
            }
        }
    }

}
