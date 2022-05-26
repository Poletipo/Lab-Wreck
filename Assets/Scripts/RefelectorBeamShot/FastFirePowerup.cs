using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastFirePowerup : Shop {

    public float FireRateIncreaseMultiplier = 1.2f;

    public override void Upgrade()
    {
        Firearm firearm = _player.GetComponent<Firearm>();
        firearm.fireRateSpeed /= FireRateIncreaseMultiplier;
        Cost = (int)(Cost * CostIncreaseMultiplier);
    }
}
