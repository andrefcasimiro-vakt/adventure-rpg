using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{
    public enum Switches
    {
        TALK_WITH_SOMEONE
    }

    [System.Serializable]
    public class Switch
    {
        public Switches switchName;

        public bool value;
    }

    public class SwitchManager : MonoBehaviour
    {
        List<Switch> switches = new List<Switch>();

        public static SwitchManager instance;

        public void Awake()
        {

            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }

            DontDestroyOnLoad(this.gameObject);

            // Initialize all switches as false at the beginning of the game

            foreach (Switches s in (Switches[])Enum.GetValues(typeof(Switches)))
            {
                Switch newSwitch = new Switch();
                newSwitch.switchName = s;
                newSwitch.value = false;

                switches.Add(newSwitch);
            }

        }

        public void UpdateSwitch(Switches switchName, bool nextValue)
        {
            var idx = switches.FindIndex(s => s.switchName == switchName);

            if (idx != -1)
            {
                switches[idx].value = nextValue;
            }
        }

        public bool EvaluateSwitch(Switches switchName)
        {
            var idx = switches.FindIndex(s => s.switchName == switchName);
            if (idx != -1)
            {
                return switches[idx].value;
            }

            return false;
        }

    }

}
