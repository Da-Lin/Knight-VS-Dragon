using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFireball : MonoBehaviour
{
    // Start is called before the first frame update
    private readonly int damage = 30;
    private bool faceRight = true;
    private Dragon dragon;
    void Start()
    {
        dragon = GameController.instance.dragon;
        StartCoroutine(DestroySelf());
        faceRight = dragon.faceRight;

        if (!faceRight)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (faceRight)
        {
            transform.position += transform.right * Time.deltaTime * 5;
        }
        else
        {
            transform.position += -transform.right * Time.deltaTime * 5;
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(1f);

        // Code to execute after the delay
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (!player.die)
            {
                Destroy(gameObject);
                GameObject explode = Instantiate(GameController.instance.fireballExplosion, collision.contacts[0].point, Quaternion.identity);
                Destroy(explode, 0.5f);
                SoundManager.instance.PlayFireballExplodeSound();

                if (player.block)
                {
                    if((faceRight && player.faceRight) || (!faceRight && !player.faceRight))
                    {
                        player.TakeDamage(damage);
                    }
                    else
                    {
                        player.weaponHealth -= damage;
                        SoundManager.instance.PlaySwordBlockSound();
                        if (player.weaponHealth < 0)
                        {
                            player.weaponHealth = 0;
                        }
                    }
                }
                else
                {
                    player.TakeDamage(damage);
                }
            }
        }
    }
}
