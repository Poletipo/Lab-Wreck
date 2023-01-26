using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceUpgrade : MonoBehaviour
{


    bool UpgradeActive = true; 

    public float ResetTime = 2;
    float resetTimer = 0;

    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!UpgradeActive)
        {
            resetTimer += Time.deltaTime;

            if(resetTimer >= ResetTime)
            {
                ReactivateUpgrade();
            }

        }

    }

    private void ReactivateUpgrade()
    {
        resetTimer = 0;
        UpgradeActive = true;
        meshRenderer.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (UpgradeActive)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                Upgrade(player);
            }
        }
    }

    private void Upgrade(Player player)
    {
        player.UpgradeDice();
        UpgradeActive = false;

        meshRenderer.enabled = false;
    }
}
