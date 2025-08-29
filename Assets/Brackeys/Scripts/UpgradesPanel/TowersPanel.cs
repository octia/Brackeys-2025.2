using UnityEngine;

public class TowersPanel : MonoBehaviour
{
    private bool hidden;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ShowButton(TowerManager tower)
    {
        hidden = true;
        anim.SetTrigger("Show");

        if (tower.spawned)
        {
            // TODO: Show towers to spawn
        }
    }

    public void HideButton()
    {
        hidden = false;
        anim.SetTrigger("Hide");
    }
}
