using PrimeTween;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuUi : MonoBehaviour
{
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private float subMenuPopupDuration = 0.5f;
    [SerializeField] private EventReference buttonClickSfx;

    private string mainGameLevelName = "Gameplay";

    [Header("Main Menu Buttons")]
    [SerializeField] private GameObject mainMenuButtonsContainer;
    private Button playButton;
    private Button htpButton;
    private Button optionsButton;
    private Button creditsButton;
    private Button quitButton;

    [Header("Submenus")]
    [SerializeField] private GameObject htpContainer;
    [SerializeField] private GameObject optionsContainer;
    [SerializeField] private GameObject creditsContainer;

    private Button htpBackButton;
    private Button optionsBackButton;
    private Button creditsBackButton;

    private bool IsAPanelOpen = false;
    void Awake()
    {
        mainCanvas = GameObject.Find("MainCanvas")?.GetComponent<Canvas>();

        // Find Main Menu Buttons
        mainMenuButtonsContainer = mainMenuButtonsContainer ?? mainCanvas.transform.Find("MainMenuUI/ButtonsContainer")?.gameObject;
        if (mainMenuButtonsContainer != null)
        {
            playButton = mainMenuButtonsContainer.transform.Find("PlayButton")?.GetComponent<Button>();
            htpButton = mainMenuButtonsContainer.transform.Find("HTPButton")?.GetComponent<Button>();
            optionsButton = mainMenuButtonsContainer.transform.Find("OptionsButton")?.GetComponent<Button>();
            creditsButton = mainMenuButtonsContainer.transform.Find("CreditsButton")?.GetComponent<Button>();
            quitButton = mainMenuButtonsContainer.transform.Find("QuitButton")?.GetComponent<Button>();
        }

        // Find submenu back buttons
        htpBackButton = htpContainer?.transform.Find("BackButton")?.GetComponent<Button>();
        optionsBackButton = optionsContainer?.transform.Find("BackButton")?.GetComponent<Button>();
        creditsBackButton = creditsContainer?.transform.Find("BackButton")?.GetComponent<Button>();
    }

    private void Start()
    {
        // Add button listeners
        playButton?.onClick.AddListener(OnPlayClicked);
        htpButton?.onClick.AddListener(() => OnOpenSubMenu(htpContainer));
        optionsButton?.onClick.AddListener(() => OnOpenSubMenu(optionsContainer));
        creditsButton?.onClick.AddListener(() => OnOpenSubMenu(creditsContainer));
        quitButton?.onClick.AddListener(OnQuitClicked);

        htpBackButton?.onClick.AddListener(() => OnExitSubMenu(htpContainer));
        optionsBackButton?.onClick.AddListener(() => OnExitSubMenu(optionsContainer));
        creditsBackButton?.onClick.AddListener(() => OnExitSubMenu(creditsContainer));

        // Hide submenus initially
        htpContainer?.SetActive(false);
        htpContainer.transform.localScale = Vector3.zero;
        optionsContainer?.SetActive(false);
        optionsContainer.transform.localScale = Vector3.zero;
        creditsContainer?.SetActive(false);
        creditsContainer.transform.localScale = Vector3.zero;
    }

    private void PlayButtonSfx()
    {
        if (!buttonClickSfx.IsNull)
            RuntimeManager.PlayOneShot(buttonClickSfx);
    }


    private void OnDestroy()
    {
        playButton?.onClick.RemoveAllListeners();
        htpButton?.onClick.RemoveAllListeners();
        optionsButton?.onClick.RemoveAllListeners();
        creditsButton?.onClick.RemoveAllListeners();
        quitButton?.onClick.RemoveAllListeners();

        htpBackButton?.onClick.RemoveAllListeners();
        optionsBackButton?.onClick.RemoveAllListeners();
        creditsBackButton?.onClick.RemoveAllListeners();
    }

    private void OnPlayClicked()
    {
        Debug.Log("Play button clicked");
        PlayButtonSfx();

        if (AudioManager.Instance != null)
        {
            // Optional: fade ambient and menu music while loading the scene immediately
            AudioManager.Instance.FadeAmbient(0f, 1f);
            AudioManager.Instance.FadeMusic(0f, 1f); // You�ll need a FadeMusic method in AudioManager

            // Load scene immediately
            SceneManager.LoadScene(mainGameLevelName);
        }
        else
        {
            SceneManager.LoadScene(mainGameLevelName);
        }
    }

    private void OnQuitClicked()
    {
        Debug.Log("Quit button clicked");
        PlayButtonSfx();
        Application.Quit();
    }

    private void OnOpenSubMenu(GameObject targetContainer)
    {
        PlayButtonSfx();
        if (IsAPanelOpen == true) return;
        IsAPanelOpen = true;
        targetContainer.SetActive(true);
        Tween.Scale(targetContainer.transform, endValue: 1f, duration: subMenuPopupDuration, ease: Ease.OutSine);
    }

    private void OnExitSubMenu(GameObject targetContainer)
    {
        PlayButtonSfx();
        Tween.Scale(targetContainer.transform, endValue: 0, duration: subMenuPopupDuration, endDelay: 0.5f)
            .OnComplete(() => targetContainer.SetActive(false));
        Tween.Scale(targetContainer.transform, endValue: 0, duration: subMenuPopupDuration, endDelay: 0.001f, ease: Ease.OutQuad)
            .OnComplete(() =>
            {
                if (targetContainer.transform.localScale == Vector3.zero)
                {
                    targetContainer.SetActive(false);
                    IsAPanelOpen = false;
                }
            });
    }
}
