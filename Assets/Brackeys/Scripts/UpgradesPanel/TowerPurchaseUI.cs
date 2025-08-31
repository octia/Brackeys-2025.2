using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerPurchaseUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private RawImage image;

    [SerializeField]
    private TMP_Text towerName;

    [SerializeField]
    private TMP_Text towerCost;

    [SerializeField]
    private Button button;

    private Action onHoverEnter;
    private Action onHoverExit;

    public void Initialize(TowerScriptableObject data)
    {
        image.texture = data.icon;
        towerName.text = data.towerName;
        towerCost.text = data.levels[0].towerCost + " Biscuits";
    }

    public void AddClickListener(UnityAction onClick)
    {
        button.onClick.AddListener(onClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHoverEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onHoverExit.Invoke();
    }

    internal void AddHoverEnterEvent(Action value)
    {
        onHoverEnter += value;
    }

    internal void AddHoverExitEvent(Action value)
    {
        onHoverExit += value;
    }

    private void OnDestroy()
    {
        onHoverExit.Invoke();
    }
}