using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void StartGame()
    {
        Debug.Log("Start game button clicked");

        SceneManager.LoadScene("Stage1");
    }

    public void ExitGame()
    {
        Debug.Log("Exit button clicked");

        Application.Quit();
    }

    internal static void ShowMenu()
    {
        
    }
}
