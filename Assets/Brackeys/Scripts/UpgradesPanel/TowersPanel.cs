using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using FMODUnity;

public class TowersPanel : MonoBehaviour
{
    [Inject]
    private PlayerDialogueController dialogueController;

    [SerializeField] TowerScriptableObject[] buyableTowersData;

    [Space]
    [SerializeField] BakeryController bakeryController;

    [Space]
    [SerializeField] GameObject towerInfoUI;
    [SerializeField] GameObject bakeryInfoUI;
    [SerializeField] GameObject buyableTowersUI;
    [SerializeField] TowerPurchaseUI buyableTowerButtonInstance;
    [SerializeField] private EventReference towerBuildSfx;
    [SerializeField] private EventReference towerUpgradeSfx;
    [SerializeField] private EventReference towerUpkeepSfx;

    [Space]
    [SerializeField] RawImage infoIcon;
    [SerializeField] TMP_Text infoNameText;
    [SerializeField] TMP_Text infoDescriptionText;
    [SerializeField] TMP_Text infoLevelText;
    [SerializeField] TMP_Text infoUpgradeCostText;
    [SerializeField] TMP_Text infoRepairCostText;
    [SerializeField] Button repairButton;

    [Space]
    [SerializeField] TMP_Text bakeryMaxBiscuitText;
    [SerializeField] TMP_Text bakeryMultiplierText;
    [SerializeField] TMP_Text bakeryMaxBiscuitCostText;
    [SerializeField] TMP_Text bakeryMultiplierCostText;

    [SerializeField]
    private TooltipController tooltip;

    [Inject]
    private BiscuitManager biscuitManager;

    private Animator anim;

    private TowerManager currentTowerManager;

    private InputAction interactInput;

    private bool hidden;

    private UIGamePauser uiGamePauser;

    private static bool hasPurchasedATower = false;

    private void Start()
    {
        anim = GetComponent<Animator>();

        foreach (TowerScriptableObject towerData in buyableTowersData)
        {
            TowerPurchaseUI newBuyableTowerButton = Instantiate(buyableTowerButtonInstance, buyableTowersUI.transform);

            newBuyableTowerButton.Initialize(towerData);

            newBuyableTowerButton.AddClickListener(() => BuyTower(towerData));

            newBuyableTowerButton.AddHoverEnterEvent(() => tooltip.Initialize(towerData));
            newBuyableTowerButton.AddHoverExitEvent(() => tooltip.Disappear());

            newBuyableTowerButton.GetComponentInChildren<RawImage>().texture = towerData.icon;
        }

        uiGamePauser = GetComponent<UIGamePauser>();

        interactInput = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        if (interactInput.WasPressedThisFrame() && uiGamePauser.enabled)
        {
            HideButton();
        }
    }

    // Bakery info
    public void ShowButton()
    {
        anim.SetTrigger("Show");

        towerInfoUI.SetActive(false);
        bakeryInfoUI.SetActive(true);
        buyableTowersUI.SetActive(false);

        UpdateBakeryText();
    }

    // Towers building & upgrades
    public void ShowButton(TowerManager towerManager)
    {
        if (!hasPurchasedATower)
        {
            dialogueController.PlayTextChain(PlayerDialogueChainType.PurchaseTowerTutorial);
            hasPurchasedATower = true;
        }

        anim.SetTrigger("Show");

        currentTowerManager = towerManager;

        if (towerManager.spawned)
        {
            towerInfoUI.SetActive(true);
            bakeryInfoUI.SetActive(false);
            buyableTowersUI.SetActive(false);

            infoIcon.texture = towerManager.towerData.icon;
            infoNameText.text = towerManager.towerData.towerName;
            infoDescriptionText.text = towerManager.towerData.towerDescription;
            infoLevelText.text = "Level " + (towerManager.currentLevel + 1);
            infoUpgradeCostText.text = towerManager.towerData.levels[towerManager.currentLevel].towerCost + "b";
            infoRepairCostText.text = towerManager.towerData.levels[towerManager.currentLevel].repairCost + "b";
            repairButton.interactable = true;
        }
        else
        {
            towerInfoUI.SetActive(false);
            bakeryInfoUI.SetActive(false);
            buyableTowersUI.SetActive(true);
        }
    }

    public void BuyTower(TowerScriptableObject towerData)
    {
        if (biscuitManager.Biscuit - towerData.levels[currentTowerManager.currentLevel].towerCost < 1)
        {
            return;
        }

        biscuitManager.Biscuit -= Mathf.RoundToInt(towerData.levels[currentTowerManager.currentLevel].towerCost);
        currentTowerManager.towerData = towerData;
        currentTowerManager.SpawnTower();

        currentTowerManager.builderVisual.SetActive(false);

        HideButton();
        PlayButtonSfx(towerBuildSfx);
    }

    public void MaxBiscuitUpgrade()
    {
        if (bakeryController.currentMaxBiscuitLevel + 1 >= bakeryController.maxBiscuitLevels.Length || biscuitManager.Biscuit - bakeryController.maxBiscuitLevels[bakeryController.currentMaxBiscuitLevel + 1].cost < 1)
        {
            return;
        }

        biscuitManager.Biscuit -= bakeryController.maxBiscuitLevels[bakeryController.currentMaxBiscuitLevel + 1].cost;

        bakeryController.UpgradeMaxBiscuitLevel();

        UpdateBakeryText();
    }

    public void MultiplierUpgrade()
    {
        if (bakeryController.currentBiscuitMultiplierLevel + 1 >= bakeryController.biscuitMultiplierLevels.Length || biscuitManager.Biscuit - bakeryController.biscuitMultiplierLevels[bakeryController.currentBiscuitMultiplierLevel + 1].cost < 1)
        {
            return;
        }

        biscuitManager.Biscuit -= bakeryController.biscuitMultiplierLevels[bakeryController.currentBiscuitMultiplierLevel + 1].cost;

        bakeryController.UpgradeMultiplierLevel();

        UpdateBakeryText();
    }

    private void UpdateBakeryText()
    {
        bakeryMaxBiscuitText.text = bakeryController.maxBiscuitLevels[bakeryController.currentMaxBiscuitLevel].maxBiscuit + " Max Biscuit";
        bakeryMultiplierText.text = bakeryController.biscuitMultiplierLevels[bakeryController.currentBiscuitMultiplierLevel].multiplier + " Multiplier";

        if (bakeryController.currentMaxBiscuitLevel + 1 >= bakeryController.maxBiscuitLevels.Length)
        {
            bakeryMaxBiscuitCostText.text = "MAX";
        }
        else
        {
            bakeryMaxBiscuitCostText.text = bakeryController.maxBiscuitLevels[bakeryController.currentMaxBiscuitLevel + 1].cost + "b";
        }

        if (bakeryController.currentBiscuitMultiplierLevel + 1 >= bakeryController.biscuitMultiplierLevels.Length)
        {
            bakeryMultiplierCostText.text = "MAX";
        }
        else
        {
            bakeryMultiplierCostText.text = bakeryController.biscuitMultiplierLevels[bakeryController.currentBiscuitMultiplierLevel + 1].cost + "b";
        }
    }

    public void HideButton()
    {
        anim.SetTrigger("Hide");

        // PlayButtonSfx();
        hidden = true;
    }

    //HEY DOUGLAS! Sorry this is so messy. I just wanted a quick and dirty, hardcoded way to call upgrade and repair!
    //Feel free to rewrite this however you want!
    //This was done at 3AM my time. I swear I am not this bad at coding lol
    //I HARDEDCODED THESE FUNCTIONS INTO THE PREFAB DIRECTLY!!! Please remove em when u wanna do it through code!

    public void UpgradeTower()
    {
        if (biscuitManager.Biscuit - currentTowerManager.towerData.levels[currentTowerManager.currentLevel].towerCost < 1)
        {
            return;
        }

        if (currentTowerManager.currentLevel + 1 < currentTowerManager.towerData.levels.Count)
        {
            biscuitManager.Biscuit -= Mathf.RoundToInt(currentTowerManager.towerData.levels[currentTowerManager.currentLevel + 1].towerCost);
            currentTowerManager.UpgradeLevel();

            infoLevelText.text = "Level " + (currentTowerManager.currentLevel + 1);
            infoUpgradeCostText.text = currentTowerManager.towerData.levels[currentTowerManager.currentLevel].towerCost + "b";
        }

        PlayButtonSfx(towerUpgradeSfx);
    }

    public void RepairTower()
    {
        if (biscuitManager.Biscuit - currentTowerManager.towerData.levels[currentTowerManager.currentLevel].repairCost < 1)
        {
            return;
        }

        biscuitManager.Biscuit -= Mathf.RoundToInt(currentTowerManager.towerData.levels[currentTowerManager.currentLevel].repairCost);
        currentTowerManager.RepairHealth();

        repairButton.interactable = false;

        PlayButtonSfx(towerUpkeepSfx);
    }

    private void PlayButtonSfx(EventReference sfx)
    {
        if (!sfx.IsNull)
            RuntimeManager.PlayOneShot(sfx);
    }
}