using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public GameObject MenuInGame;
    public PlayerController PlyCtr_Script;
    public Text StartText;
    public GameObject RestartButton;

    public Transform MonsterParent;

    void Start()
    {
        if(StartText != null) //permet de savoir si on est dans le menu
        {
            if (PlayerPrefs.GetFloat("Teddy") <= 0)
            {
                StartText.text = "Start";
                RestartButton.SetActive(false);
            }
            else
            {
                StartText.text = "Resume";
                RestartButton.SetActive(true);
            }
        }

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

    public void NextLevel()
    {
        PlyCtr_Script.GetComponent<ArmyManager>().SaveStartMapData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        PlayerPrefs.SetInt("MapLevel", PlayerPrefs.GetInt("MapLevel") + 1);
    }

    public void RestartRun()
    {
        PlayerPrefs.DeleteAll();
        StartText.text = "Start";
        RestartButton.SetActive(false);
    }

}
