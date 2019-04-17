using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    private readonly int damage = 30;
    private bool faceRight = true;
    private PlayerController player;
    void Start()
    {
        player = GameController.instance.player;
        StartCoroutine(DestroySelf());
        faceRight = player.faceRight;

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
        if (collision.gameObject.GetComponent<Enemy>())
        {
            if (!collision.gameObject.GetComponent<Enemy>().die)
            {
                Destroy(gameObject);
                GameObject explode = Instantiate(GameController.instance.fireballExplosion, collision.contacts[0].point, Quaternion.identity);
                Destroy(explode, 0.5f);
                SoundManager.instance.PlayFireballExplodeSound();
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
        }else if (collision.gameObject.GetComponent<Dragon>())
        {
            if (!collision.gameObject.GetComponent<Dragon>().die)
            {
                Destroy(gameObject);
                GameObject explode = Instantiate(GameController.instance.fireballExplosion, collision.contacts[0].point, Quaternion.identity);
                Destroy(explode, 0.5f);
                SoundManager.instance.PlayFireballExplodeSound();
                collision.gameObject.GetComponent<Dragon>().TakeDamage(damage);
            }
        }



    }
}
