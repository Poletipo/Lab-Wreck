using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    public int FaceValue = 0;





    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position+ transform.up*1);
    }
}
