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

    private void Update()
    {
        Time.timeScale = gameOverMenu.activeSelf ? 0 : 1;
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
