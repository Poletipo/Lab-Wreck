using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    public IDamager hitter;

    public void ActivateHitter()
    {
        hitter.ActivateDamager();
    }


    public void EndHitter()
    {
        hitter.DeactivateDamager();
    }


}
