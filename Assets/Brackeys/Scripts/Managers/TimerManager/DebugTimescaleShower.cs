using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class DebugTimescaleShower : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    [Inject]
    private TimerManager timerManager;

    private void Update()
    {
        text.text = $"Timescale: {timerManager.TimeScale}";
    }
}
