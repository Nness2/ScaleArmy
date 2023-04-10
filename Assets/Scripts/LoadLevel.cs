using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public GameObject MenuInGame;
    public PlayerController PlyCtr_Script;
    void Start()
    {
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");

    }

    public void BackToGame()
    {
        MenuInGame.SetActive(false);
    }

    public void OpenMenuInGame()
    {
        if(MenuInGame.activeSelf == false)
        {
            PlyCtr_Script.enabled = false;
            MenuInGame.SetActive(true);
        }
        else
        {
            PlyCtr_Script.enabled = true;
            MenuInGame.SetActive(false);
        }

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
