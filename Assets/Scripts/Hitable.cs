using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitable : MonoBehaviour {

    public Action OnHit;

    public void Hit(int damage)
    {
        OnHit?.Invoke();
        //Debug.Log(gameObject.name + " has been hit for " + damage + " damage");
    }
}
