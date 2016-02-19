using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private static CanvasGroup _ingameMenu;

    private static Stopwatch _timeMenuIsDisplayed = new Stopwatch();
    private static Stopwatch _timeMenuIsHidden = new Stopwatch();

    public static bool IsPaused;
    private static string _currentStage;
    private static CanvasGroup _stageComplete;

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
        _currentStage = "Stage1";
        SceneManager.LoadScene(_currentStage);

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

    public void NextStage()
    {
        switch (_currentStage)
        {
            case "Stage1":
                _currentStage = "Stage2";
                break;
            case "Stage2":
                _currentStage = "Stage3";
                break;
            case "Stage3":
                _currentStage = "Stage4";
                break;
            case "Stage4":
                _currentStage = "Stage5";
                break;
            case "Stage5":
                _currentStage = "TheEnd";
                break;
        }
        Time.timeScale = 1;
        //IsPaused = false;
        _stageComplete.alpha = 0x00;
        _stageComplete.blocksRaycasts = false;

        SceneManager.LoadScene(_currentStage);
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

    public static void ShowStageComplete(Canvas stageCompleteCanvas)
    {
        _stageComplete = stageCompleteCanvas.GetComponent<CanvasGroup>();

        Time.timeScale = 0;
        _stageComplete.alpha = 0xFF;
        _stageComplete.blocksRaycasts = true;

        //IsPaused = true;
    }
}
