using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{
    public Animator animator;
    Rigidbody2D rigidBody;

    bool timeToAttack = false;
    bool attack = false;
    public bool faceRight = false;
    bool jump = false;
    bool playerHit = false;
    bool playerNearby = false;

    public bool die = false;

    public Image healthBar;

    public int id = 0;

    public float jumpSpeed = 5f;

    float attackChangeTime = 1.0f;

    public float health = 500;
    public float maxHealth = 500;

    int moveSpeed = 2;

    int strikeDmg = 10;

    PlayerController player;

    bool stateChanged = false;

    enum State { idle, move, jump };

    State currentState = State.idle;

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.instance.player;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        health = 500;
        maxHealth = 500;
}

    // Update is called once per frame
    void Update()
    {
        if (player.isPaused)
        {
            return;
        }

        healthBar.fillAmount = health / maxHealth;

        if(Mathf.Abs(player.transform.position.x - transform.position.x) < 15)
        {
            healthBar.gameObject.SetActive(true);
        }
        else
        {
            healthBar.gameObject.SetActive(false);
        }

        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 8)
        {
            playerNearby = true;
        }
        else
        {
            playerNearby = false;
        }

        if (!die && health <= 0)
        {
            die = true;
            gameObject.layer = 9;
            animator.SetBool("die", true);
            StartCoroutine(DestroySelf());
            GameController.instance.GetEnemies().Remove(this);
            return;
        }

        if (die)
        {
            return;
        }

        if (!stateChanged)
        {
            stateChanged = true;
            ChangeState();
            StartCoroutine("NeedToStateChange");
        }

        //check if hit player
        if (currentState == State.move)
        {
            if (!playerHit)
            {
                playerHit = true;
                StartCoroutine("ChangePlayerHit");
                if ((player.transform.position.x < transform.position.x && !faceRight) || (player.transform.position.x > transform.position.x && faceRight))
                {
                    if (Mathf.Abs(player.transform.position.x - transform.position.x) < 1.2
                    && player.transform.position.y - transform.position.y > -0.5
                    && player.transform.position.y - transform.position.y < 0.7)
                    {
                        player.TakeDamage(strikeDmg);
                    }

                }
            }

        }

        // enemy move
        if (faceRight)
        {
            if (currentState == State.move)
            {
                transform.position += transform.right * Time.deltaTime * moveSpeed;
            }
            UpdateFace(1);
        }
        else
        {
            if (currentState == State.move && transform.position.x > 74)
            {
                transform.position += -transform.right * Time.deltaTime * moveSpeed;
            }
            UpdateFace(-1);
        }

        if (!timeToAttack)
        {
            timeToAttack = true;
            ChangeAttack();
            ChangeJump();
            StartCoroutine("NeedToChangeAttack");
            if (attack)
            {
                animator.SetTrigger("attack");

                if (Mathf.Abs(player.transform.position.x - transform.position.x) < 13)
                {
                    SoundManager.instance.PlayDragonFireballShootSound();
                }
                GameObject fireball = Instantiate(GameController.instance.dragonFireball, transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);

            }

            if (jump)
            {
                jump = false;
                if (Mathf.Abs(player.transform.position.x - transform.position.x) < 13)
                {
                    SoundManager.instance.PlayJumpSound();
                }
                animator.SetBool("jump", true);
                rigidBody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            }

        }
    }

    void UpdateFace(int face)
    {
        Vector3 scale = transform.localScale;
        scale.x = face;
        transform.localScale = scale;
    }

    IEnumerator NeedToChangeAttack()
    {
        yield return new WaitForSeconds(attackChangeTime);

        timeToAttack = false;
    }

    IEnumerator ChangePlayerHit()
    {
        yield return new WaitForSeconds(0.5f);

        playerHit = false;
    }

    IEnumerator NeedToStateChange()
    {
        if (playerNearby)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        stateChanged = false;
    }

    public void TakeDamage(int damage)
    {
        if (!die)
        {
            animator.SetTrigger("hit");
            health -= damage;
            if (health < 0)
            {
                health = 0;
            }
        }

        StartCoroutine("CollideFlash");
    }

    IEnumerator CollideFlash()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.3f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    //check to see if need to attack
    void ChangeAttack()
    {

        float r = Random.Range(0f, 1f);
        if (r < 0.5)
        {
            attack = true;
        }
        else
        {
            attack = false;
        }
    }

    void ChangeJump()
    {

        float r = Random.Range(0f, 1f);
        if (r < 0.5)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
    }

    void ChangeState()
    {
        if (playerNearby)
        {
            if(player.transform.position.x < transform.position.x)
            {
                faceRight = false;
            }
            else
            {
                faceRight = true;
            }
            currentState = State.move;
            animator.SetBool("move", true);
        }
        else
        {
            float r = Random.Range(0f, 1f);
            if (r < 0.5)
            {
                currentState = State.idle;
                animator.SetBool("move", false);
            }
            else
            {
                float f = Random.Range(0f, 1f);
                if (f < 0.5)
                {
                    faceRight = true;
                }
                else
                {
                    faceRight = false;
                }
                currentState = State.move;
                animator.SetBool("move", true);
            }
        }

    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(0.8f);

        Destroy(gameObject);
        string bestScore = PlayerPrefs.GetString("Score");
        string score = Time.timeSinceLevelLoad.ToString("0.00");
        GameController.instance.score.text = "Congratulations! Your score is " + score + ".";

        if (bestScore.Length == 0 ||float.Parse(bestScore) > float.Parse(score))
        {
            PlayerPrefs.SetString("Score", score);
            GameController.instance.bestScore.text = score;
        }

        MenuController.instance.ShowEndMenu();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            animator.SetBool("jump", false);
        }
    }

}
