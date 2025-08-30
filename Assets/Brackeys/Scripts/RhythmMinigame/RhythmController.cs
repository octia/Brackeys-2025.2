using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class RhythmController : MonoBehaviour
{
    [SerializeField] private int notesForBatch;
    [SerializeField] private int biscuitPerBatch;
    [SerializeField] private int[] pointsForMultiplier;
    [SerializeField] private int multiplierLimit;
    [SerializeField] private float newNoteTime;
    public float speed;
    
    [SerializeField] private EventReference minigameMusic;

    [Space]
    [SerializeField] Transform trackPerformPoint;
    public Transform failPoint;

    [Space]
    [SerializeField] private Transform performPerfectPoint;
    [SerializeField] private Transform failPerfectPoint;
    [SerializeField] private EventReference missedNoteSfx;

    [Space]
    [SerializeField] private GameObject main;
    [SerializeField] private TMP_Text perfectText;
    [SerializeField] private TMP_Text produceText;
    [SerializeField] private EventReference minigameStartSfx;
    [SerializeField] private EventReference minigameExitSfx;

    [Space]
    [SerializeField] private RectTransform progressBar;
    [SerializeField] private RectTransform progressBarProgress;
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private EventReference multiplierSfx;

    [Space]
    [SerializeField] private Transform leftTrack;
    [SerializeField] private Transform rightTrack;
    [SerializeField] private GameObject leftNote;
    [SerializeField] private GameObject rightNote;
    [SerializeField] private EventReference leftNoteSfx;
    [SerializeField] private EventReference rightNoteSfx;

    [Space]
    [SerializeField] private BakeryController bakeryController;

    // ...
    private InputAction leftPerformInput;
    private InputAction rightPerformInput;
    private InputAction interactInput;

    [Inject, HideInInspector]
    public BiscuitManager biscuitManager;

    [Inject]
    private PlayerController playerController;

    private float currentNewNoteTime;

    private bool onLeft;

    private int notesProgress;
    private int multiplier = 1;
    private int perfectNotes;
    private int multiplierProgress;

    [HideInInspector] public int baseMultiplier;

    private SceneAudio sceneAudio;

    private void Start()
    {
        leftPerformInput = InputSystem.actions.FindAction("ANote");
        rightPerformInput = InputSystem.actions.FindAction("DNote");
        interactInput = InputSystem.actions.FindAction("Interact");
        
        sceneAudio = FindObjectOfType<SceneAudio>();
    }

    private void Update()
    {
        if (!main.activeSelf)
        {
            return;
        }

        Transform nearestNote = null;
        bool performed = false;

        if (leftPerformInput.WasPressedThisFrame()&& leftTrack.childCount > 0)
        {
            nearestNote = leftTrack.GetChild(0);
            performed = true;
            PlayButtonSfx(leftNoteSfx);
        }

        if (rightPerformInput.WasPressedThisFrame() && rightTrack.childCount > 0)
        {
            nearestNote = rightTrack.GetChild(0);
            performed = true;
            PlayButtonSfx(rightNoteSfx);
        }

        if (performed)
        {
            if (nearestNote != null && nearestNote.transform.position.y < trackPerformPoint.position.y)
            {
                PerformNote();

                if (nearestNote.transform.position.y > failPerfectPoint.position.y && nearestNote.transform.position.y < performPerfectPoint.position.y)
                {
                    PerformPerfectNote();
                    multiplierProgress += 2;
                }
                else
                {
                    perfectText.gameObject.SetActive(false);
                    multiplierProgress += 1;
                    perfectNotes = 0;
                }

                if (multiplierProgress >= pointsForMultiplier[multiplier - 1] && multiplier < multiplierLimit)
                {
                    multiplier++;
                    multiplierText.text = multiplier + "x";

                    perfectNotes = 0;
                }

                Destroy(nearestNote.gameObject);
            }
            else
            {
                ResetTracks();
                PlayButtonSfx(missedNoteSfx);
            }
        }

        currentNewNoteTime -= Time.deltaTime;

        if (currentNewNoteTime < 0)
        {
            GameObject newNote;

            newNote = Instantiate(onLeft ? leftNote : rightNote, onLeft ? leftTrack : rightTrack);
            newNote.GetComponent<NoteController>().rhythmController = this;

            currentNewNoteTime = newNoteTime;

            onLeft = !onLeft;
        }
    }

    public void ProduceButton()
    {
        main.SetActive(!main.activeSelf);

        bakeryController.infoButton.SetActive(!main.activeSelf);
        playerController.movement.canMove = !main.activeSelf;
        playerController.visual.gameObject.SetActive(!main.activeSelf);

        if (main.activeSelf)
        {
            produceText.text = "Leave";

            PlayButtonSfx(minigameStartSfx);

            ResetTracks();

            // Start ambience/music when active
            if (AudioManager.Instance != null)
            {
                // Fade ambience in
                sceneAudio?.OnMinigameStart();

                // Fade minigame music in
                // AudioManager.Instance.PlayMusic(produceMusicEvent, 1.5f);
            }
        }
        else
        {
            produceText.text = "Bake";

            PlayButtonSfx(minigameExitSfx);

            // Stop ambience/music when deactivated
            if (AudioManager.Instance != null)
            {
                // Fade ambience out
                sceneAudio?.OnMinigameEnd();

                // Fade minigame music out
                // AudioManager.Instance.FadeMusic(0f, 1.5f, stopAfterFade: true);
            }



        }
    }

    public void PerformNote()
    {
        print(biscuitPerBatch * baseMultiplier * multiplier);
        notesProgress += 1;

        if (notesProgress >= notesForBatch)
        {
            biscuitManager.Biscuit += biscuitPerBatch * baseMultiplier * multiplier;
            notesProgress = 0;
            PlayButtonSfx(multiplierSfx);
        }

        RectSetRight(progressBarProgress, 600 - progressBar.rect.width / notesForBatch * notesProgress);
    }

    public void PerformPerfectNote()
    {
        perfectNotes++;

        perfectText.text = "Perfect hit! (" + perfectNotes + "x)";
        perfectText.gameObject.SetActive(true);
    }

    public void ResetTracks()
    {
        notesProgress = 0;
        multiplier = 1;
        perfectNotes = 0;
        multiplierProgress = 0;

        perfectText.gameObject.SetActive(false);

        RectSetRight(progressBarProgress, 600 - progressBar.rect.width / notesForBatch * notesProgress);
        multiplierText.text = multiplier + "x";

        Transform[] leftTrackNotes = leftTrack.GetComponentsInChildren<Transform>();
        Transform[] rightTrackNotes = rightTrack.GetComponentsInChildren<Transform>();

        for (int i = 1; i < leftTrackNotes.Length; i++)
        {
            Destroy(leftTrackNotes[i].gameObject);
        }

        for (int i = 1; i < rightTrackNotes.Length; i++)
        {
           Destroy(rightTrackNotes[i].gameObject);
        }
    }

    public static void RectSetRight(RectTransform rect, float right)
    {
        rect.offsetMax = new Vector2(-right, rect.offsetMax.y);
    }

    private void PlayButtonSfx(EventReference sfx)
    {
        if (!sfx.IsNull)
            RuntimeManager.PlayOneShot(sfx);
    }

}
