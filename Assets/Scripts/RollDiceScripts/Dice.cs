using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{

    Face[] faces;
    public Mesh meshCollider;

    // Start is called before the first frame update
    void Start()
    {
        faces = GetComponentsInChildren<Face>();
    }


    public int GetFaceUpValue()
    {
        float dotresult = -9;
        int result = 0;

        if(faces == null)
        {
            return 1;
        }

        foreach (Face face in faces)
        {

            float tempDot = Vector3.Dot(face.transform.up, Vector3.up);
            //Debug.Log(face.FaceValue);
            if (tempDot >= dotresult)
            {
                result = face.FaceValue;
                dotresult = tempDot;
            }
        }
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
