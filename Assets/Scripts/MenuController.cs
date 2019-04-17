using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    private void Awake()
    {
        instance = this;
        Hide();
    }

    public GameObject mainMenu;
    public GameObject hotKeysMenu;
    public GameObject bestScoreMenu;
    public GameObject endMenu;

    void SwitchMenu(GameObject someMenu)
    {
        mainMenu.SetActive(false);
        hotKeysMenu.SetActive(false);
        bestScoreMenu.SetActive(false);
        endMenu.SetActive(false);

        someMenu.SetActive(true);
    }

    public void RestartGame()
    {
        SoundManager.instance.PlayButtonClickSound();
        SceneManager.LoadScene("SampleScene");
    }

    public void Show()
    {
        SoundManager.instance.PlayMenuBgSound();
        ShowMainMenu();
        gameObject.SetActive(true);
        Time.timeScale = 0;
        GameController.instance.player.isPaused = true;
    }

    public void Hide()
    {
        if (GameController.instance != null && GameController.instance.dragon.die)
        {
            return;
        }
        if(SoundManager.instance != null)
        {
            SoundManager.instance.PlayGameBgSound();
            SoundManager.instance.PlayButtonClickSound();
        }

        gameObject.SetActive(false);
        Time.timeScale = 1;
        if(GameController.instance != null)
        {
            GameController.instance.player.isPaused = false;
        }

    }

    public void ShowMainMenu()
    {
        SoundManager.instance.PlayButtonClickSound();
        SwitchMenu(mainMenu);
    }

    public void ShowHotKeys()
    {
        SoundManager.instance.PlayButtonClickSound();
        SwitchMenu(hotKeysMenu);
    }

    public void ShowBestScore()
    {
        SoundManager.instance.PlayButtonClickSound();
        SwitchMenu(bestScoreMenu);
    }

    public void ShowEndMenu()
    {
        SoundManager.instance.PlayMenuBgSound();
        SwitchMenu(endMenu);
        gameObject.SetActive(true);
        Time.timeScale = 0;
        GameController.instance.player.isPaused = true;
    }

    public void ResetScore()
    {
        PlayerPrefs.DeleteAll();
        SoundManager.instance.PlayButtonClickSound();
        GameController.instance.bestScore.text = "XX:XX";
    }
}
