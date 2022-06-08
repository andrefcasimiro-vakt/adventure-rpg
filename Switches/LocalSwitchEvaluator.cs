using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AF
{


    public class LocalSwitchEvaluator : MonoBehaviour
    {

        public LocalSwitch localSwitch;

        public bool activateWhenTrue;

        private void Start()
        {
            Evaluate();
        }

        public void Evaluate()
        {
            if (localSwitch.value == true)
            {
                if (activateWhenTrue)
                {
                    foreach (Transform t in transform)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
                else
                {
                    foreach (Transform t in transform)
                    {
                        t.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (activateWhenTrue)
                {
                    foreach (Transform t in transform)
                    {
                        t.gameObject.SetActive(false);
                    }
                }
                else
                {
                    foreach (Transform t in transform)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
            }
        }

    }

}