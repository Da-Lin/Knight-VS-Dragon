using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    public GameObject shopPanel;

    int potionPrice = 10;
    int fileballPrice = 30;
    int meleeDmgPrice = 50;
    int maxHealthPrice = 100;
    int enemyGoldPrice = 200;
    int repairWeaponPrice = 50;

    bool playerEntered = false;

    PlayerController player;

    void Start()
    {
        player = GameController.instance.GetPlayer();
        shopPanel.SetActive(false);
    }

    void Update()
    {
        if (playerEntered)
        {
            if (player.upKeyTapped)
            {
                if (shopPanel.activeSelf)
                {
                    shopPanel.SetActive(false);
                }
                else
                {
                    shopPanel.SetActive(true);
                }

            }

            if (shopPanel.activeSelf && player.escapKeyTapped)
            {
                shopPanel.SetActive(false);
            }
        }
        else if(shopPanel.activeSelf)
        {
            shopPanel.SetActive(false);
        }


    }

    public void BuyPotion()
    {
        if(player.inventory.gold >= potionPrice)
        {
            player.inventory.numOfPotion += 1;
            player.inventory.gold -= potionPrice;
        }
       
    }

    public void BuyFireball()
    {
        if (player.inventory.gold >= fileballPrice)
        {
            player.inventory.numOfFireball += 1;
            player.inventory.gold -= fileballPrice;
        }
    }

    public void IncreaseMeleeDmg()
    {
        if (player.inventory.gold >= meleeDmgPrice)
        {
            player.inventory.gold -= meleeDmgPrice;
            player.meleeDamage += 10;
        }
       
    }

    public void IncreaseEnemyGold()
    {
        if (player.inventory.gold >= enemyGoldPrice)
        {
            player.inventory.gold -= enemyGoldPrice;
            GameController.instance.goldEarnedRange += 50;
        }

    }

    public void IncreaseMaxHealth()
    {
        if (player.inventory.gold >= maxHealthPrice)
        {
            player.health += 50;
            player.weaponHealth += 50;
            player.maxHealth += 50;
            player.inventory.gold -= maxHealthPrice;
        }
    }

    public void RepairWeapon()
    {
        if (player.inventory.gold >= repairWeaponPrice)
        {
            player.weaponHealth = player.maxHealth;
            player.inventory.gold -= repairWeaponPrice;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            playerEntered = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            playerEntered = false;
        }
    }
}
