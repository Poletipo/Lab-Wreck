using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSpaceTransform : MonoBehaviour
{
    public Transform cam;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 look = player.position - cam.position ;
        look.y = 0;

        transform.rotation = Quaternion.LookRotation(look, Vector3.up);


    }
}
