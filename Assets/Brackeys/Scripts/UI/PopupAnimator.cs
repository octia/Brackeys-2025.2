using PrimeTween;
using UnityEngine;

public class PopupAnimator : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 1f)]
    private float animationTime = 0.25f;

    public new bool enabled = false;
    private Vector3 startScale = Vector3.one;
    private Tween tween;

    private void Awake()
    {
        startScale = transform.localScale;
        transform.localScale = Vector3.zero;
        if (!enabled)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            SetEnabled();
        }
        else
        {
            SetDisabled();
        }
    }

    private void SetEnabled()
    {
        if (enabled)
        {
            return;
        }
        enabled = true;

        if (tween.isAlive)
        {
            tween.Stop();
        }
        gameObject.SetActive(true);
        tween = Tween.Scale(transform, startScale, animationTime);
    }

    private void SetDisabled()
    {
        if (!enabled)
        {
            return;
        }

        gameObject.SetActive(true);
        enabled = false;

        if (tween.isAlive)
        {
            tween.Stop();
        }
        tween = Tween.Scale(transform, 0, animationTime);
        tween.OnComplete(() => gameObject.SetActive(false));
    }
}
