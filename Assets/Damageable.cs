using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    public struct HitData {
        public Vector3 position;
        public Vector3 direction;
        public float force;
        public int damage;
    }

    public Action<HitData> OnHit;

    private HitData hitInfo = new HitData();

    public void Hit(HitData data)
    {
        hitInfo = data;
        OnHit?.Invoke(hitInfo);
    }

    public HitData GetHitData()
    {
        return hitInfo;
    }

}
