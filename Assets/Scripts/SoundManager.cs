using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    AudioSource audioSource;
    public AudioClip gameBgSound;
    public AudioClip menuBgSound;
    public AudioClip swordAttackSound;
    public AudioClip fireballShoot;
    public AudioClip dragonFireballShoot;
    public AudioClip fireballExplode;
    public AudioClip buttonClick;
    public AudioClip swordBlock;
    public AudioClip jumpSound;
    public AudioClip regenSound;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGameBgSound()
    {
        if(audioSource != null)
        {
            audioSource.clip = gameBgSound;
            audioSource.loop = true;
            audioSource.Play();
        }

    }

    public void PlayMenuBgSound()
    {
        audioSource.clip = menuBgSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySwordAttackSound()
    {
        audioSource.PlayOneShot(swordAttackSound);
    }

    public void PlayButtonClickSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(buttonClick);
        }
    }

    public void PlayFireballShootSound()
    {
        audioSource.PlayOneShot(fireballShoot);
    }

    public void PlayDragonFireballShootSound()
    {
        audioSource.PlayOneShot(dragonFireballShoot);
    }
    public void PlaySwordBlockSound()
    {
        audioSource.PlayOneShot(swordBlock);
    }

    public void PlayFireballExplodeSound()
    {
        audioSource.PlayOneShot(fireballExplode);
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void PlayRegenSound()
    {
        audioSource.PlayOneShot(regenSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
