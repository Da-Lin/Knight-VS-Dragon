using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    Rigidbody2D rigidBody;

    public object healthBar;

    bool timeToAttack = false;
    bool attack = false;
    bool faceRight = false;
    bool jump = false;

    public bool die = false;

    Vector3 diePos = new Vector3(0, 0, 0);

    public int id = 0;

    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;

    float attackChangeTime = 1.0f;

    public float health = 100;
    public float maxHealth = 100;

    int meleeDmg = 20;

    PlayerController player;

    bool stateChanged = false;

    enum State { idle, move, jump};

    State currentState = State.idle;

    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.GetEnemies().Add(this);
        player = GameController.instance.player;
        rigidBody = GetComponent<Rigidbody2D>();

    }


    void ShowFloatingText()
    {

        var text = Instantiate(GameController.instance.flotingText, transform.position, Quaternion.identity);
        int goldEarned = Random.Range(GameController.instance.goldEarnedRange, GameController.instance.goldEarnedRange + 50);
        text.GetComponent<TextMesh>().text = "+ " + goldEarned + "G";
        player.inventory.gold += goldEarned;

    }

    // Update is called once per frame
    void Update()
    {
        if (player.isPaused)
        {
            return;
        }

        if (!die && health <= 0)
        {
            //show floating text
            ShowFloatingText();
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

        if (!timeToAttack)
        {
            timeToAttack = true;
            ChangeAttack();
            ChangeJump();
            StartCoroutine("NeedToChangeAttack");
            if (attack)
            {
                animator.SetTrigger("attack");
                if(Mathf.Abs(player.transform.position.x - transform.position.x) < 13)
                {
                    SoundManager.instance.PlaySwordAttackSound();
                }

                if ((player.transform.position.x < transform.position.x && !faceRight)|| (player.transform.position.x > transform.position.x && faceRight) )
                {
                    if(Mathf.Abs(player.transform.position.x - transform.position.x) < 2.35
                    && player.transform.position.y - transform.position.y > -0.8
                    && player.transform.position.y - transform.position.y < 1.5)
                    {
                        player.TakeDamage(meleeDmg);
                    }

                }

            }

            if (jump)
            {
                if (Mathf.Abs(player.transform.position.x - transform.position.x) < 13)
                {
                    SoundManager.instance.PlayJumpSound();
                }
                jump = false;
                animator.SetBool("jump", true);
                rigidBody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            }

        }

        // enemy move
        if (faceRight)
        {
            //avoid enemy moving out of edge
            if (id < 2)
            {
                if (transform.position.x > 13)
                {
                    currentState = State.idle;
                }
            }
            else if (id < 5 && transform.position.x > 62)
            {
                currentState = State.idle;
            }
            if (currentState == State.move)
            {
                transform.position += transform.right * Time.deltaTime;
            }
            UpdateFace(1);
        }
        else
        {
            //avoid enemy moving out of edge
            if (id < 2)
            {
                if (transform.position.x < 0.5)
                {
                    currentState = State.idle;
                }
            }
            if (currentState == State.move)
            {
                transform.position += -transform.right * Time.deltaTime;
            }
            UpdateFace(-1);
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

    IEnumerator NeedToStateChange()
    {
        yield return new WaitForSeconds(2f);

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
    
        float r = Random.Range(0f, 1f);
        if(r < 0.5)
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

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(0.8f);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            animator.SetBool("jump", false);
        }
    }


}
