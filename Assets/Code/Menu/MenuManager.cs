using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private static CanvasGroup _ingameMenu;

    private static Stopwatch _timeMenuIsDisplayed = new Stopwatch();
    private static Stopwatch _timeMenuIsHidden = new Stopwatch();

    public static bool IsPaused;

    public static bool IsShown
    {
        get { return _timeMenuIsDisplayed.ElapsedMilliseconds > 200; }
    }

    public static bool IsHidden
    {
        get { return _timeMenuIsHidden.ElapsedMilliseconds > 200; }
    }

    void Start()
    {
        _timeMenuIsHidden.Start();

        DontDestroyOnLoad(this);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Stage1");

        ResumeGame();
    }

    public void ResumeGame()
    {
        ResumeGame(_ingameMenu);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    internal static void ResumeGame(CanvasGroup gameMenu)
    {
        _ingameMenu = gameMenu;

        Time.timeScale = 1;
        _ingameMenu.alpha = 0x00;
        _ingameMenu.blocksRaycasts = false;

        IsPaused = false;

        _timeMenuIsHidden = new Stopwatch();
        _timeMenuIsHidden.Start();
    }

    internal static void ShowMenu(CanvasGroup gameMenu)
    {
        _ingameMenu = gameMenu;

        Time.timeScale = 0;
        _ingameMenu.alpha = 0xFF;
        _ingameMenu.blocksRaycasts = true;

        IsPaused = true;
        _timeMenuIsDisplayed = new Stopwatch();
        _timeMenuIsDisplayed.Start();
    }
}
