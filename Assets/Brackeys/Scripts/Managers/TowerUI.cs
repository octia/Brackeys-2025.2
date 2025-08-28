using UnityEngine;
using UnityEngine.UI;
public class TowerUI : MonoBehaviour
{
    public Image cooldownBar;
    public Image healthBar;
    public Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }

    public void UpdateCooldown(float t) => cooldownBar.fillAmount = t;
    // public void UpdateHealth(float t) => healthBar.fillAmount = t;
}
