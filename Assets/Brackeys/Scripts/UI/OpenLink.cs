using UnityEngine;
using UnityEngine.UI;

public class OpenLink : MonoBehaviour
{
    public string link;

    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OpenURL);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OpenURL);
    }
    public void OpenURL()
    {
        Application.OpenURL(link);
    }
}
