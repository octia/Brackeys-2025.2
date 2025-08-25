using Reflex.Attributes;
using TMPro;
using UnityEngine;

public class BiscuitController : MonoBehaviour
{
    public TMP_Text biscuitText;

    [Inject] private BiscuitManager biscuitManager;

    private void Awake()
    {
        biscuitManager.biscuitController = this;
        biscuitManager.Biscuit = 0;
    }
}
