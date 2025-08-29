using System.Collections.Generic;
using PrimeTween;
using Reflex.Attributes;
using UnityEngine;

public class TimerManager
{
    public float TimeScale => GetTimeScale();

    [Inject]
    private TimerManagerConfig config;

    private float timeScale = 1f;

    public bool IsPaused = false;

    private HashSet<object> registeredPausers = new();

    private Dictionary<object, float> registeredSlowers = new();

    private Tween timeScaleTween;

    private float timeScaleOverride = 1f;

    private bool isTimescaleOverriden = false;

    /// <summary>
    /// Registers a "game pauser" object.
    /// Useful when multiple UI elements pause the game.
    /// All pausers must be unregistered before game plays.
    /// Any registered pauser means game is paused.
    /// </summary>
    public void RegisterGamePauser(MonoBehaviour gamePauser)
    {
        registeredPausers.Add(gamePauser);
        if (registeredPausers.Count > 0)
        {
            SetGamePaused(true);
        }
    }

    public void UnregisterGamePauser(MonoBehaviour gamePauser)
    {
        registeredPausers.Remove(gamePauser);

        if (registeredPausers.Count == 0)
        {
            SetGamePaused(false);
        }
    }

    public void RegisterGameSlower(object gamePauser, float timeScale)
    {
        registeredSlowers.Add(gamePauser, timeScale);
        if (registeredSlowers.Count > 0)
        {
            SmoothSetTimeScale(config.SelectionTimeScale);
        }
    }

    public void UnregisterGameSlower(object gamePauser)
    {
        registeredSlowers.Remove(gamePauser);

        if (registeredSlowers.Count == 0)
        {
            SmoothSetTimeScale(config.NormalTimeScale);
        }
    }

    public void SetTimeScaleOverride(float scale)
    {
        isTimescaleOverriden = true;
        if (timeScaleTween.isAlive)
        {
            timeScaleTween.Stop();
        }

        timeScaleTween = TweenOverrideTimeScale(scale);
    }

    public void UnsetTimeScaleOverride()
    {
        if (timeScaleTween.isAlive)
        {
            timeScaleTween.Stop();
        }

        timeScaleTween = TweenOverrideTimeScale(config.NormalTimeScale);
        timeScaleTween.OnComplete(() => isTimescaleOverriden = false);
    }

    private void SetTimeScale(float newTimeScale)
    {
        if (timeScaleTween.isAlive)
        {
            timeScaleTween.Stop();
        }
        timeScale = newTimeScale;
    }

    private void SmoothSetTimeScale(float newTimeScale)
    {
        if (timeScaleTween.isAlive)
        {
            timeScaleTween.Stop();
        }

        timeScaleTween = TweenTimeScale(newTimeScale);
    }

    private Tween TweenTimeScale(float target)
    {
        return Tween.Custom(
            timeScale,
            target,
            config.TimeScaleChangeAnimDuration,
            (value) => timeScale = value
        );
    }

    private Tween TweenOverrideTimeScale(float target)
    {
        return Tween.Custom(
            timeScaleOverride,
            target,
            config.TimeScaleChangeAnimDuration,
            (value) => timeScaleOverride = value
        );
    }

    private void SetGamePaused(bool state)
    {
        IsPaused = state;
    }

    private float GetTimeScale()
    {
        if (IsPaused)
        {
            return 0;
        }

        if (isTimescaleOverriden)
        {
            return timeScaleOverride * config.StaticTimeScaleMultiplier;
        }

        return timeScale * config.StaticTimeScaleMultiplier;
    }
}
