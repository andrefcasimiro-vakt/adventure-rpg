using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF { 
    public abstract class State : MonoBehaviour
    {
        public abstract void OnEnter(Character character);
        public abstract State Tick(Character character);
    }
}