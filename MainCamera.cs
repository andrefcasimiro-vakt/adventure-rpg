using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;            //Private variable to store the offset distance between the player and camera

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
                offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
                transform.position = player.transform.position + offset;
    }
}
