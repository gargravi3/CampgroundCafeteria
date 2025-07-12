using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        public string instruction;
        public Vector3 highlightPosition;
        public Vector2 highlightSize;
        public bool waitForInput;
        public KeyCode requiredKey;
        public float duration = 5f;
    }

    [SerializeField] private TutorialStep[] tutorialSteps;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private RectTransform highlightBox;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button skipButton;
    
    private int currentStep = 0;
    private Camera mainCamera;

    void Start()
    {
        Debug.Log("Tutorial Manager started");
        mainCamera = Camera.main;
        SetupButtons();
        StartTutorial();
    }

    void SetupButtons()
    {
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(NextStep);
            Debug.Log("Next button configured");
        }
        else
        {
            Debug.LogError("Next button is null!");
        }

        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipTutorial);
            Debug.Log("Skip button configured");
        }
        else
        {
            Debug.LogError("Skip button is null!");
        }
    }

    void StartTutorial()
    {
        ShowStep(0);
    }

    void ShowStep(int stepIndex)
    {
        Debug.Log($"Showing step {stepIndex}");
        
        if (stepIndex >= tutorialSteps.Length)
        {
            EndTutorial();
            return;
        }

        TutorialStep step = tutorialSteps[stepIndex];
        
        // Set instruction text with visual key prompt if needed
        if (step.requiredKey != KeyCode.None)
        {
            instructionText.text = step.instruction + $"\n\nPress [{step.requiredKey}]";
        }
        else
        {
            instructionText.text = step.instruction;
        }
        
        Debug.Log($"Step instruction: {step.instruction}");
        Debug.Log($"Wait for input: {step.waitForInput}");
        Debug.Log($"Required key: {step.requiredKey}");
        
        // Position highlight box
        if (step.highlightPosition != Vector3.zero)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(step.highlightPosition);
            highlightBox.position = screenPos;
            highlightBox.sizeDelta = step.highlightSize;
            highlightBox.gameObject.SetActive(true);
        }
        else
        {
            highlightBox.gameObject.SetActive(false);
        }

        // Handle step progression
        if (step.waitForInput)
        {
            // Hide Next button if waiting for specific key
            if (step.requiredKey != KeyCode.None)
            {
                nextButton.gameObject.SetActive(false);
                StartCoroutine(WaitForKeyInput(step));
            }
            else
            {
                // Show Next button for manual progression
                nextButton.gameObject.SetActive(true);
            }
        }
        else
        {
            nextButton.gameObject.SetActive(false);
            StartCoroutine(AutoProgressStep(step.duration));
        }
    }

    IEnumerator WaitForKeyInput(TutorialStep step)
    {
        while (true)
        {
            // Check for specific key input
            if (step.requiredKey != KeyCode.None && Input.GetKeyDown(step.requiredKey))
            {
                Debug.Log($"Player pressed {step.requiredKey}");
                // Small delay to let player see the movement happen
                yield return new WaitForSeconds(0.5f);
                NextStep();
                yield break;
            }
            // If no required key, wait for Next button (handled elsewhere)
            else if (step.requiredKey == KeyCode.None)
            {
                yield break; // Exit, Next button will handle progression
            }
            yield return null;
        }
    }

    IEnumerator AutoProgressStep(float duration)
    {
        yield return new WaitForSeconds(duration);
        NextStep();
    }

    public void NextStep()
    {
        Debug.Log($"Progressing from step {currentStep} to {currentStep + 1}");
        currentStep++;
        ShowStep(currentStep);
    }

    public void SkipTutorial()
    {
        Debug.Log("Tutorial skipped");
        EndTutorial();
    }

    void EndTutorial()
    {
        Debug.Log("Tutorial ended");
        tutorialPanel.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    // Debug helper - Press P in play mode to get world positions
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"World Position: {hit.point}");
            }
        }
    }
}