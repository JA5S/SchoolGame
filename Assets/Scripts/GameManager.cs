using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public GameObject gameOverMenu;
    private AudioSource audioSource;
    public AudioClip townMusic;
    public AudioClip battleMusic;

    private bool inCombat;
    private bool playingTownMusic = true;
    private bool playingBattleMusic = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Time.timeScale = gameOverMenu.activeSelf ? 0 : 1;

        if(inCombat && !playingBattleMusic)
        {
            audioSource.clip = battleMusic;
            audioSource.Play();
            playingBattleMusic = true;
            playingTownMusic = false;
        }
        if(!inCombat && !playingTownMusic)
        {
            audioSource.clip = townMusic;
            audioSource.Play();
            playingBattleMusic = false;
            playingTownMusic = true;
        }
    }

    public void setInCombat(bool battling)
    {
        inCombat = battling;
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Reload the game
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Quit Game");
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
