using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AF { 
    public class EnemyHealthbar : MonoBehaviour
    {
        public Character character;

        public Image fillImage;

        // Update is called once per frame
        void Update()
        {
            fillImage.fillAmount = character.health * .01f;
        }
    }
}

