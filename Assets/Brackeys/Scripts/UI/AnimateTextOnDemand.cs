using TMPEffects.Components;
using UnityEngine;

public class AnimateTextOnDemand : MonoBehaviour
{

    private TMPAnimator tMPAnimator;
    private bool enableAnimation = false;

    void Start()
    {
        tMPAnimator = GetComponent<TMPAnimator>();
        tMPAnimator.UpdateAnimations(0);
        tMPAnimator.ResetTime();
    }

    void Update()
    {
        tMPAnimator.UpdateAnimations(Time.deltaTime / 10);

        if (enableAnimation == false) return;

        tMPAnimator.UpdateAnimations(Time.deltaTime);

    }

    public void SetAnimationFlag(bool flag)
    {
        enableAnimation = flag;
    }
}
