using Reflex.Attributes;
using UnityEngine;

public class UIGameSlower : MonoBehaviour
{
    [Inject]
    private TimerManager timerManager;

    [Inject]
    private TimerManagerConfig config;

    private void OnEnable()
    {
        timerManager.RegisterGameSlower(this, config.SelectionTimeScale);
    }

    private void OnDisable()
    {
        timerManager.UnregisterGameSlower(this);
    }
}
