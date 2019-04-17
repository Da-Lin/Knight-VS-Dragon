using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int numOfPotion = 2;
    public int numOfFireball = 2;

    public Text numOfPotionText;
    public Text numOfFireballText;
    public Text goldText;

    public int gold = 100;

    // Start is called before the first frame update
    void Start()
    {
        numOfPotion = 0;
        numOfFireball = 0;
        gold = 0;
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = ": " + gold + "G";
        numOfPotionText.text = ": " + numOfPotion;
        numOfFireballText.text = ": " + numOfFireball;
    }
}
