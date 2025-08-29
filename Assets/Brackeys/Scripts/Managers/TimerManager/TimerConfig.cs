using UnityEngine;

[CreateAssetMenu(menuName = "Brackeys2025/Config/TimerConfig", fileName = "TimerConfig")]
public class TimerConfig : ScriptableObject
{
    [field: SerializeField]
    [field: Range(0.01f, 10f)]
    public float StaticTimeScaleMultiplier { get; private set; } = 1f;

    [field: SerializeField]
    public float NormalTimeScale { get; private set; } = 1f;

    [field: SerializeField]
    [field: Range(0.01f, 1f)]
    public float SelectionTimeScale { get; private set; } = 0.01f;

    [field: SerializeField]
    [field: Range(0.01f, 1f)]
    public float AbilitySlowTimeScale { get; private set; } = 0.2f;

    [field: SerializeField]
    [field: Range(0.01f, 1f)]
    public float TimeScaleChangeAnimDuration { get; private set; } = 0.15f;
}
