using UnityEngine;

public class UIFollower : MonoBehaviour
{
    public Transform target;
    [SerializeField] private RectTransform canvasRect;

    void LateUpdate()
    {
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(target.position);
        Vector2 targetScreenPosition = new Vector2(
        (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
        (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));

        //now you can set the position of the ui element
        GetComponent<RectTransform>().anchoredPosition = targetScreenPosition;
    }
}