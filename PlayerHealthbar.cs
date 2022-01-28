using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AF
{

    public class PlayerHealthbar : MonoBehaviour
    {
        public Image fillImage;

        Player player;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        // Update is called once per frame
        void Update()
        {
            fillImage.fillAmount = player.health * 0.01f;
        }
    }

}
