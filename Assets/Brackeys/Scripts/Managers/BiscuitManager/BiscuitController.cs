using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BiscuitController : MonoBehaviour
{
    [SerializeField] private BakeryController bakeryController;

    [Space]
    public TMP_Text biscuitText;
    public Image biscuitBar;

    [Inject] private BiscuitManager biscuitManager;

    private void Awake()
    {
        biscuitManager.biscuitController = this;
        biscuitManager.maxBiscuits = bakeryController.maxBiscuitLevels[bakeryController.currentMaxBiscuitLevel].maxBiscuit;
        biscuitManager.Biscuit = biscuitManager.config.initialBiscuits;
    }
}
