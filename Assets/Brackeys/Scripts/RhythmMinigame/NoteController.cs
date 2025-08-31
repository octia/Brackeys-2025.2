using Reflex.Attributes;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public RhythmController rhythmController;

    private void Update()
    {
        if (rhythmController.timerManager.IsPaused)
        {
            return;
        }

        // Note movement
        transform.position += new Vector3(0, -1) * Time.deltaTime * rhythmController.speed;

        // Failed note
        if (transform.position.y < rhythmController.failPoint.position.y)
        {
            rhythmController.ResetTracks();
        }
    }
}