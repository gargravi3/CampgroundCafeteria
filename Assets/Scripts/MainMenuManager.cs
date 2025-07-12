using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject mainMenuPanel;
    public GameObject tutorialOptionsPanel;
    public GameObject introductionPanel;
    public GameObject introductionTutorialPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        Debug.Log("Menu Loaded");
        TutorialOptions();
    }
    public void TutorialOptions()
    {
        mainMenuPanel.SetActive(false);
        tutorialOptionsPanel.SetActive(true);
    }

    public void Introduction()
    {
        tutorialOptionsPanel.SetActive(false);
        introductionPanel.SetActive(true);
    }
    public void IntroductionTutorial()
    {
        tutorialOptionsPanel.SetActive(false);
        introductionTutorialPanel.SetActive(true);
    }

    public void LoadGame()
    {
        Debug.Log("Starting Game");
        SceneManager.LoadScene("Main");
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadTutorial()
    {
        Debug.Log("Starting Tutorial");
        SceneManager.LoadScene("Tutorial");
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        introductionPanel.SetActive(false);
        creditsPanel.SetActive(false);
        tutorialOptionsPanel.SetActive(false);
        introductionTutorialPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
