using Reflex.Attributes;
using UnityEngine;

public class UIGamePauser : MonoBehaviour
{
    [Inject]
    private TimerManager timerManager;

    private void OnEnable()
    {
        timerManager.RegisterGamePauser(this);
    }

    private void OnDisable()
    {
        timerManager.UnregisterGamePauser(this);
    }
}
