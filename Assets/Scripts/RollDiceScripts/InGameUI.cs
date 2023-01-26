using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{

    public Player player;
    public TextMeshProUGUI speedTxt;
    public Health[] enemyHealth;
    public Image healthBar;
    int totalMaxHealth = 0;
    int totalCurrentHealth = 0;

    public GameObject WinScreen;
    public Spawner spawner;


    bool gameIsWon = false;


    // Start is called before the first frame update
    void Start()
    {

        foreach (Health item in enemyHealth)
        {
            totalMaxHealth += item.MaxHp;
        }
        totalCurrentHealth = totalMaxHealth;
        
        

    }

    // Update is called once per frame
    void Update()
    {
        speedTxt.text = player.diceValue.ToString();


        if (!gameIsWon)
        {
            totalCurrentHealth = 0;
            foreach (Health item in enemyHealth)
            {
                totalCurrentHealth += item.Hp;
            }

            if(totalCurrentHealth <= 0)
            {
                gameIsWon = true;
            }


            healthBar.fillAmount = 1.0f * totalCurrentHealth / totalMaxHealth ;
        }
        else if (!WinScreen.activeInHierarchy)
        {
            WinScreen.SetActive(true);
            spawner.enabled = false;
        }


    }
}
