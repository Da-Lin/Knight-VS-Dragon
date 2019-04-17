using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Outlet
    Rigidbody2D rigidBody;
    public float moveSpeed = 8f;
    public float jumpSpeed = 5f;

    public GameObject inventoryPanel;

    public bool upKeyTapped = false;
    public bool escapKeyTapped = false;
    public bool isPaused = false;
    public bool block = false;

    public bool die = false;

    SpriteRenderer sprite;

    Dragon dragon;

    bool attack = false;

    public Image healthBar;
    public Image weaponHealthBar;

    public float health = 100f;
    public float weaponHealth = 100f;
    public float maxHealth = 100f;

    public Inventory inventory;

    public int meleeDamage = 20;

    public Animator animator;

    //Player State
    public bool faceRight = true;

    // State Tracking
    public int jumpsLeft;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        dragon = GameController.instance.dragon;
    }

    private void FixedUpdate()
    {
        if (isPaused)
        {
            return;
        }

        animator.SetFloat("speed", rigidBody.velocity.magnitude);
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("walk") && rigidBody.velocity.magnitude > 0)
        {
            animator.speed = rigidBody.velocity.magnitude / 3f;
        }
        else
        {
            animator.speed = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            return;
        }

        healthBar.fillAmount = health / maxHealth;
        weaponHealthBar.fillAmount = weaponHealth / maxHealth;

        if (health <= 0)
        {
            die = true;
            animator.SetTrigger("die");
            StartCoroutine("DieAnimation");
            return;
        }

        // Block
        if(weaponHealth > 0)
        {
            if (Input.GetKey(KeyCode.L))
            {
                block = true;
                animator.SetBool("block", true);
            }
            else
            {
                block = false;
                animator.SetBool("block", false);
            }
        }
        else
        {
            block = false;
            animator.SetBool("block", false);
        }

        // Move left
        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.AddForce(Vector2.left * moveSpeed);
            sprite.flipX = true;
            faceRight = false;
        }

        // Move right
        if (Input.GetKey(KeyCode.D))
        {
            rigidBody.AddForce(Vector2.right * moveSpeed);
            sprite.flipX = false;
            faceRight = true;
        }

        //use potion
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (inventory.numOfPotion >= 1 && health < maxHealth)
            {
                animator.SetTrigger("cast");
                SoundManager.instance.PlayRegenSound();
                health += GameController.potionRegen;
                GameObject potionEffect = Instantiate(GameController.instance.potionEffect, transform.position, Quaternion.identity);
                Destroy(potionEffect, 0.25f);
                inventory.numOfPotion -= 1;
            }
        }

        if (!block)
        {
            // attack
            if (Input.GetKeyDown(KeyCode.J))
            {
                attack = animator.GetCurrentAnimatorStateInfo(0).IsName("attack");
                if (!attack)
                {
                    animator.SetTrigger("attack");
                    SoundManager.instance.PlaySwordAttackSound();
                    if (!dragon.die)
                    {
                        if ((faceRight && transform.position.x < dragon.transform.position.x) || (!faceRight && transform.position.x > dragon.transform.position.x))
                        {
                            if (Mathf.Abs(transform.position.x - dragon.transform.position.x) < 2.2f
                                && transform.position.y - dragon.transform.position.y < 1
                                && transform.position.y - dragon.transform.position.y > -1)
                            {
                                dragon.TakeDamage(meleeDamage);
                            }
                        }
                    }

                    foreach (Enemy e in GameController.instance.GetEnemies())
                    {
                        if (e != null)
                        {
                            if ((faceRight && transform.position.x < e.transform.position.x) || (!faceRight && transform.position.x > e.transform.position.x))
                            {
                                if (Mathf.Abs(transform.position.x - e.transform.position.x) < 2.2f
                                    && transform.position.y - e.transform.position.y < 2
                                    && transform.position.y - e.transform.position.y > -0.5)
                                {
                                    e.TakeDamage(meleeDamage);
                                }
                            }
                        }


                    }
                }

            }

            //use fireball
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (inventory.numOfFireball >= 1)
                {
                    animator.SetTrigger("cast");
                    SoundManager.instance.PlayFireballShootSound();
                    inventory.numOfFireball -= 1;
                    GameObject fireball = Instantiate(GameController.instance.fireball, transform.position - new Vector3(0, 0.5f, 0), Quaternion.identity);

                }
            }
        }


        //display inverntory
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(false);
            }
            else
            {
                inventoryPanel.SetActive(true);
            }
        }

        //cheat
        if (Input.GetKeyDown(KeyCode.C))
        {
            inventory.numOfPotion = 100;
            inventory.numOfFireball = 100;
            inventory.gold = 10000;
            health = 1000;
            maxHealth = 1000;
            weaponHealth = 1000;
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
            upKeyTapped = true;
        }
        else
        {
            upKeyTapped = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapKeyTapped = true;
            MenuController.instance.Show();
        }
        else
        {
            escapKeyTapped = false;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpsLeft > 0)
            {
                SoundManager.instance.PlayJumpSound();
                animator.SetBool("jump", true);
                rigidBody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                jumpsLeft--;
            }
        }
    }

    IEnumerator DieAnimation()
    {
        yield return new WaitForSeconds(0.8f);

        SceneManager.LoadScene("SampleScene");
    }

    IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(0.75f);

        attack = false;
    }

    public void TakeDamage(int damage)
    {
        if (!die)
        {
            animator.SetTrigger("hurt");
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

    void UpdateFace(int face)
    {
        Vector3 scale = transform.localScale;
        scale.x = face;
        transform.localScale = scale;
    }

    // Check for Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {

            //draw rays from left edge to right edge
            //Debug.DrawRay(transform.position + new Vector3(-0.1f, 0, 0), -transform.up * 1.25f);
            List<RaycastHit2D[]> hits = new List<RaycastHit2D[]>();
            for (float i = -0.1f; i < 0.25f; i += 0.01f)
            {
                hits.Add(Physics2D.RaycastAll(transform.position + new Vector3(i, 0, 0), -transform.up, 1.4f));
            }

            foreach (RaycastHit2D[] hit in hits)
            {
                for (int i = 0; i < hit.Length; i++)
                {
                    if (hit[i].collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        jumpsLeft = 2;
                        animator.SetBool("jump", false);
                    }
                }
            }

        }
    }

}
