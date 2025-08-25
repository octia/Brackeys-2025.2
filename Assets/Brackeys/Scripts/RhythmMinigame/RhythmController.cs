using Reflex.Attributes;
using UnityEngine;

public class RhythmController : MonoBehaviour
{
    [SerializeField] private float newNoteTime;
    public float speed;

    [Space]
    public Transform trackPerformPoint;
    public Transform failPoint;

    [Space]
    [SerializeField] private Transform leftTrack;
    [SerializeField] private Transform rightTrack;
    [SerializeField] private GameObject leftNote;
    [SerializeField] private GameObject rightNote;

    [Inject, HideInInspector]
    public BiscuitManager biscuitManager;

    float currentNewNoteTime;

    private void Update()
    {
        currentNewNoteTime -= Time.deltaTime;

        if (currentNewNoteTime < 0)
        {
            int direction = Random.Range(0, 2);

            GameObject newNote;

            if (direction == 0)
            {
                newNote = Instantiate(leftNote, leftTrack);
            }
            else
            {
                newNote = Instantiate(rightNote, rightTrack);
            }

            newNote.GetComponent<NoteController>().rhythmController = this;

            currentNewNoteTime = newNoteTime;
        }
    }
}
