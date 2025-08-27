using UnityEngine;

public class UpgradesPanel : MonoBehaviour
{
    private bool hidden;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ShowButton()
    {
        hidden = !hidden;
        anim.SetTrigger("Show");
    }
}
