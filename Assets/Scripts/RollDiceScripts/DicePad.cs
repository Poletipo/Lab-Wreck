using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DicePad : MonoBehaviour
{

    bool Active = true;
    public Material shutDownMat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (Active)
        {


            Player player = collision.collider.GetComponent<Player>();

            if (player != null)
            {
                player.SetDiceValue();
                GetComponent<MeshRenderer>().material = shutDownMat;
                Active = false;
            }
        }
    }



}
