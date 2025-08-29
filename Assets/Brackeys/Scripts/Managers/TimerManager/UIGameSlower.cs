using Reflex.Attributes;
using UnityEngine;

public class UIGameSlower : MonoBehaviour
{
    [Inject]
    private TimerManager timerManager;

    [Inject]
    private TimerConfig timerConfig;

    private void OnEnable()
    {
        timerManager.RegisterGameSlower(this, timerConfig.SelectionTimeScale);
    }

    private void OnDisable()
    {
        timerManager.UnregisterGameSlower(this);
    }
}
