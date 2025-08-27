using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuUi : MonoBehaviour
{
    [SerializeField] private Canvas mainCanvas;
    private string mainGameLevelName = "Gameplay";
    [SerializeField] private float subMenuPopupDuration = 0.5f;

    [Header("Main Menu Buttons")]
    private GameObject mainMenuParent;
    [SerializeField] private GameObject mainMenuButtonsContainer;
    private Button playButton;
    private Button htpButton;
    private Button optionsButton;
    private Button creditsButton;
    private Button quitButton;
    [Header("How To Play Menu")]
    private GameObject htpParent;
    [SerializeField] private GameObject htpContainer;
    private Button htpBackButton;

    [Header("Options Menu")]
    private GameObject optionsParent;
    [SerializeField] private GameObject optionsContainer;
    private Button optionsBackButton;

    [Header("Credits Menu")]

    private GameObject creditsParent;
    [SerializeField] private GameObject creditsContainer;
    private Button creditsBackButton;

    private bool IsAPanelOpen = false;
    void Awake()
    {
        mainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        mainMenuParent = mainCanvas.transform.Find("MainMenuUI")?.gameObject;
        mainMenuButtonsContainer = mainMenuParent.transform.Find("ButtonsContainer")?.gameObject;

        htpParent = mainCanvas.transform.Find("HTPUI")?.gameObject;
        htpContainer = htpParent.transform.Find("HTPContainer")?.gameObject;

        optionsParent = mainCanvas.transform.Find("OptionsUI")?.gameObject;
        optionsContainer = optionsParent.transform.Find("OptionsContainer")?.gameObject;

        creditsParent = mainCanvas.transform.Find("CreditsUI")?.gameObject;
        creditsContainer = creditsParent.transform.Find("CreditsContainer")?.gameObject;

        if (mainMenuButtonsContainer != null)
        {
            playButton = mainMenuButtonsContainer.transform.Find("PlayButton")?.GetComponent<Button>();
            htpButton = mainMenuButtonsContainer.transform.Find("HTPButton")?.GetComponent<Button>();
            optionsButton = mainMenuButtonsContainer.transform.Find("OptionsButton")?.GetComponent<Button>();
            creditsButton = mainMenuButtonsContainer.transform.Find("CreditsButton")?.GetComponent<Button>();
            quitButton = mainMenuButtonsContainer.transform.Find("QuitButton")?.GetComponent<Button>();
        }

        if (htpContainer != null)
        {
            htpBackButton = htpContainer.transform.Find("BackButton")?.GetComponent<Button>();
        }
        if (optionsContainer != null)
        {
            optionsBackButton = optionsContainer.transform.Find("BackButton")?.GetComponent<Button>();
        }
        if (creditsContainer != null)
        {
            creditsBackButton = creditsContainer.transform.Find("BackButton")?.GetComponent<Button>();
        }

    }
    void Start()
    {
        playButton?.onClick.AddListener(OnPlayClicked);
        htpButton?.onClick.AddListener(() => OnOpenSubMenu(htpContainer));
        optionsButton?.onClick.AddListener(() => OnOpenSubMenu(optionsContainer));
        creditsButton?.onClick.AddListener(() => OnOpenSubMenu(creditsContainer));
        quitButton?.onClick.AddListener(OnQuitClicked);

        htpBackButton?.onClick.AddListener(() => OnExitSubMenu(htpContainer));
        optionsBackButton?.onClick.AddListener(() => OnExitSubMenu(optionsContainer));
        creditsBackButton?.onClick.AddListener(() => OnExitSubMenu(creditsContainer));

        htpContainer?.SetActive(false);
        optionsContainer?.SetActive(false);
        creditsContainer?.SetActive(false);
    }
    private void OnPlayClicked()
    {
        Debug.Log("Play button clicked");
        SceneManager.LoadScene(mainGameLevelName);
    }
    private void OnQuitClicked()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }
    private void OnOpenSubMenu(GameObject targetContainer)
    {
        if (IsAPanelOpen == true) return;
        IsAPanelOpen = true;
        targetContainer.SetActive(true);
        Tween.Scale(targetContainer.transform, endValue: 1f, duration: subMenuPopupDuration, ease: Ease.OutSine);
    }
    private void OnExitSubMenu(GameObject targetContainer)
    {
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
