using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public static int potionRegen = 20;

    public Enemy ninjiaPrefab;

    public GameObject potionEffect;
    public GameObject fireballExplosion;
    public GameObject fireball;
    public GameObject dragonFireball;
    public GameObject flotingText;

    public PlayerController player;
    public Dragon dragon;

    public Text timePassed;
    public Text bestScore;
    public Text score;

    bool enemySpawned = false;
    int spawnTime;

    public int goldEarnedRange = 50;
    public int enemyRespawnTime = 15;

    ArrayList enemies;

    private Vector3[] enemySapwnPosition = {new Vector3(2.85f, -2.33f, -1), new Vector3(4.85f, -2.33f, -1), new Vector3(39.12f, -4.95f, -1)
    , new Vector3(46f, -4.95f, -1), new Vector3(53f, -4.95f, -1) };

    private void Awake()
    {
        instance = this;
        enemies = new ArrayList();
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            Enemy e = Instantiate(ninjiaPrefab, enemySapwnPosition[i], Quaternion.identity);
            e.id = i;
        }

        bestScore.text = PlayerPrefs.GetString("Score");
    }

    // Update is called once per frame
    void Update()
    {
        int time = (int)Time.timeSinceLevelLoad;

        timePassed.text = Time.timeSinceLevelLoad.ToString("0.00");

        if(time % enemyRespawnTime == 0 && !enemySpawned)
        {
            spawnTime = time;
            enemySpawned = true;
            spawnEnemy();
        }
        else if(time > spawnTime)
        {
            enemySpawned = false;
        }
    }

    public void spawnEnemy()
    {
        int id = Random.Range(0, 4);
        Enemy e = Instantiate(ninjiaPrefab, enemySapwnPosition[id], Quaternion.identity);
        e.id = id;
    }

    public ArrayList GetEnemies()
    {
        return enemies;
    }

    public PlayerController GetPlayer()
    {
        return instance.player;
    }

}
