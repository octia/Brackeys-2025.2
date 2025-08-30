using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RhythmController : MonoBehaviour
{
    [SerializeField] private int notesForBatch;
    [SerializeField] private int biscuitPerBatch;
    [SerializeField] private int[] pointsForMultiplier;
    [SerializeField] private int multiplierLimit;
    [SerializeField] private float newNoteTime;
    public float speed;

    [Space]
    [SerializeField] Transform trackPerformPoint;
    public Transform failPoint;

    [Space]
    [SerializeField] private Transform performPerfectPoint;
    [SerializeField] private Transform failPerfectPoint;

    [Space]
    [SerializeField] private GameObject main;
    [SerializeField] private TMP_Text perfectText;
    [SerializeField] private TMP_Text produceText;

    [Space]
    [SerializeField] private RectTransform progressBar;
    [SerializeField] private RectTransform progressBarProgress;
    [SerializeField] private TMP_Text multiplierText;

    [Space]
    [SerializeField] private Transform leftTrack;
    [SerializeField] private Transform rightTrack;
    [SerializeField] private GameObject leftNote;
    [SerializeField] private GameObject rightNote;

    // ...
    private InputAction leftPerformInput;
    private InputAction rightPerformInput;
    private InputAction interactInput;

    [Inject, HideInInspector]
    public BiscuitManager biscuitManager;

    [Inject]
    private PlayerController playerController;

    float currentNewNoteTime;

    bool onLeft;

    int notesProgress;
    int multiplier = 1;
    int perfectNotes;
    int multiplierProgress;

    private void Start()
    {
        leftPerformInput = InputSystem.actions.FindAction("ANote");
        rightPerformInput = InputSystem.actions.FindAction("DNote");
        interactInput = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        if (!main.activeSelf)
        {
            return;
        }

        Transform nearestNote = null;
        bool performed = false;

        if (leftPerformInput.WasPressedThisFrame())
        {
            nearestNote = leftTrack.GetChild(0);
            performed = true;
        }

        if (rightPerformInput.WasPressedThisFrame())
        {
            nearestNote = rightTrack.GetChild(0);
            performed = true;
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

        playerController.movement.canMove = !main.activeSelf;
        playerController.visual.gameObject.SetActive(!main.activeSelf);

        if (main.activeSelf)
        {
            produceText.text = "Leave";

            ResetTracks();

            // Start ambience/music when active
            if (AudioManager.Instance != null)
            {
                // Fade ambience in
                AudioManager.Instance.FadeAmbient(1f, 1.5f);

                // Or play specific music
                // AudioManager.Instance.PlayMusic(produceMusicEvent, 1.5f);
            }
        }
        else
        {
            produceText.text = "Bake";

            // Stop ambience/music when deactivated
            if (AudioManager.Instance != null)
            {
                // Fade ambience out
                AudioManager.Instance.FadeAmbient(0f, 1.5f);

                // Or stop current music
                // AudioManager.Instance.FadeMusic(0f, 1.5f, stopAfterFade: true);
            }
        }
    }

    public void PerformNote()
    {
        notesProgress += 1;

        if (notesProgress >= notesForBatch)
        {
            biscuitManager.Biscuit += biscuitPerBatch * multiplier;
            notesProgress = 0;
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
}
