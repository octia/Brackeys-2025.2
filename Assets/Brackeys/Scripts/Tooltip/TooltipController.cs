using PrimeTween;
using TMPro;
using UnityEngine;

public class TooltipController : MonoBehaviour
{
    [SerializeField]
    private Transform animationTarget;

    [SerializeField]
    private TMP_Text towerName;

    [SerializeField]
    private TMP_Text towerDescription;

    [SerializeField]
    [Range(0f, 1f)]
    private float animationDuration = 0.25f;

    [SerializeField]
    [Range(0f, 1f)]
    private float closeDelay = 0.25f;

    private new Sequence animation;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Initialize(TowerScriptableObject tower)
    {
        gameObject.SetActive(true);
        Appear();
        towerName.text = tower.towerName;
        towerDescription.text = tower.towerDescription;
    }

    public void Appear()
    {
        if (animation.isAlive)
        {
            animation.Stop();
        }

        animation = Sequence.Create(Tween.Scale(animationTarget, 1, animationDuration));
    }

    public void Disappear()
    {
        if (animation.isAlive)
        {
            animation.Stop();
        }

        animation = Sequence.Create(Tween.Delay(closeDelay));
        animation.Chain(Tween.Scale(animationTarget, 0, animationDuration));
    }
}
