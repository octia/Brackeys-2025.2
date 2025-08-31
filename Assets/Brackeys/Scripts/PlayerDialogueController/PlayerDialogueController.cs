using System.Linq;
using PrimeTween;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using FMODUnity;

public class PlayerDialogueController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject dialogueBox;

    [SerializeField]
    private GameObject interactionBlocker;

    [SerializeField]
    private GameObject skipButton;

    [SerializeField]
    private TMP_Text dialogueTextBox;

    [SerializeField]
    private Image speakerIcon;

    [SerializeField]
    private InputAction skipAction;

    [SerializeField] private EventReference buttonClickSfx;

    [Inject]
    private PlayerDialogueConfig config;

    //[Inject]
    //private SoundManager soundManager;

    private Tween textAnimTween;

    private PlayerDialogueChain currentChain;

    private int currentDialogueIndex = 0;

    public void PlayText(string message, float lettersPerSecond, Sprite overrideSprite = null)
    {
        SetEnabled(true);

        speakerIcon.sprite = overrideSprite ? overrideSprite : config.DefaultIcon;
        dialogueTextBox.text = message;

        //var textSound = soundManager.PlayTextLoop();
        textAnimTween = Tween
            .Custom(
                0,
                message.Length,
                message.Length / lettersPerSecond,
                (val) => dialogueTextBox.maxVisibleCharacters = Mathf.RoundToInt(val),
                Ease.Linear
            );
        //.OnComplete(() => soundManager.StopLoop(textSound));
    }

    public void PlayTextChain(PlayerDialogueChainType type)
    {
        if (type == PlayerDialogueChainType.Empty)
        {
            return;
        }

        var chainToUse = config.PlayerDialogueChains.FirstOrDefault(
            (chain) => chain.ChainType == type
        );
        if (chainToUse == null)
        {
            return;
        }

        currentChain = chainToUse;
        currentDialogueIndex = 0;
        SetEnabled(true);
        PlayCurrentDialogue();
    }

    public void SkipAllText()
    {
        currentChain = null;
        currentDialogueIndex = 0;
        SetInteractionBlockEnabled(false);
        SetEnabled(false);
        PlayButtonSfx();
    }

    private void PlayCurrentDialogue()
    {
        if (currentChain == null)
        {
            return;
        }

        if (currentDialogueIndex >= currentChain.Dialogues.Count)
        {
            SetEnabled(false);
            SetInteractionBlockEnabled(false);
            currentChain = null;
            return;
        }

        if (currentChain.ShouldBlockInteraction)
        {
            SetInteractionBlockEnabled(true);
        }

        var currentDialogue = currentChain.Dialogues[currentDialogueIndex];
        PlayText(currentDialogue.Message, config.CharactersPerSecond, currentDialogue.IconOverride);
        currentDialogueIndex++;
    }

    private void SetInteractionBlockEnabled(bool enabled)
    {
        interactionBlocker.SetActive(enabled);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ProgressDialogue();
    }

    public void ProgressDialogue()
    {
        if (textAnimTween.isAlive)
        {
            textAnimTween.Complete();
            return;
        }
        PlayButtonSfx();
        PlayCurrentDialogue();
    }

    private void Awake()
    {
        skipButton.GetComponent<Button>().onClick.AddListener(SkipAllText);
        skipAction.performed += (_) => SkipAllText();
        PlayTextChain(PlayerDialogueChainType.InitialTutorial);
    }

    private void SetEnabled(bool status)
    {
        if (dialogueBox.activeSelf == status)
        {
            return;
        }

        if (status)
        {
            dialogueBox.SetActive(status);
        }

        if (currentChain)
        {
            skipButton.SetActive(currentChain.ShouldBeSkippable);
        }

        var startTweenValue = status ? 0f : 1f;
        var endTweenValue = status ? 1f : 0f;
        var longerTween = Tween.ScaleX(dialogueBox.transform, startTweenValue, endTweenValue, 0.7f);
        Tween.ScaleY(dialogueBox.transform, startTweenValue, endTweenValue, 0.4f);

        if (!status)
        {
            longerTween.OnComplete(() => dialogueBox.SetActive(false));
        }
    }

    private void PlayButtonSfx(EventReference? sfxOverride = null)
    {
        EventReference sfxToPlay = sfxOverride.HasValue ? sfxOverride.Value : buttonClickSfx;

        if (!sfxToPlay.IsNull)
            RuntimeManager.PlayOneShot(sfxToPlay);

    }
}
