using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorObject : MonoBehaviour {

    public MovementController mc;


    // Start is called before the first frame update
    void Start()
    {
        mc.OnDirectionChanged += OnDirectionChanged;
    }

    private void OnDirectionChanged(MovementController mc)
    {
        if (mc.facingDirection.x < 0) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (mc.facingDirection.x > 0) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
