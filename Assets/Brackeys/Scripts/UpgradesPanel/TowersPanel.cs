using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class TowersPanel : MonoBehaviour
{
    [SerializeField] TowerScriptableObject[] buyableTowersData;

    [Space]
    [SerializeField] GameObject towerInfoUI;
    [SerializeField] GameObject buyableTowersUI;
    [SerializeField] GameObject buyableTowerButtonInstance;
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

    [Inject]
    private BiscuitManager biscuitManager;

    private Animator anim;

    private TowerManager currentTowerManager;

    private void Start()
    {
        anim = GetComponent<Animator>();

        foreach (TowerScriptableObject towerData in buyableTowersData)
        {
            GameObject newBuyableTowerButton = Instantiate(buyableTowerButtonInstance, buyableTowersUI.transform);

            newBuyableTowerButton.GetComponent<Button>().onClick.AddListener(() => BuyTower(towerData));
            newBuyableTowerButton.GetComponentsInChildren<TMP_Text>()[0].text = towerData.towerName;
            newBuyableTowerButton.GetComponentsInChildren<TMP_Text>()[1].text = towerData.levels[0].towerCost + " Biscuits";
            newBuyableTowerButton.GetComponentInChildren<RawImage>().texture = towerData.icon;
        }
    }

    public void ShowButton(TowerManager towerManager)
    {
        anim.SetTrigger("Show");

        currentTowerManager = towerManager;

        if (towerManager.spawned)
        {
            towerInfoUI.SetActive(true);
            buyableTowersUI.SetActive(false);

            infoIcon.texture = towerManager.towerData.icon;
            infoNameText.text = towerManager.towerData.towerName;
            infoDescriptionText.text = towerManager.towerData.towerDescription;
            infoLevelText.text = "Level " + (towerManager.currentLevel + 1);
            infoUpgradeCostText.text = towerManager.towerData.levels[towerManager.currentLevel].towerCost + "b";
            infoRepairCostText.text = towerManager.towerData.levels[towerManager.currentLevel].repairCost + "b";
        }
        else
        {
            towerInfoUI.SetActive(false);
            buyableTowersUI.SetActive(true);
        }
    }

    public void BuyTower(TowerScriptableObject towerData)
    {
        if (biscuitManager.Biscuit - towerData.levels[0].towerCost < 1)
        {
            return;
        }

        biscuitManager.Biscuit -= Mathf.RoundToInt(towerData.levels[0].towerCost);
        currentTowerManager.towerData = towerData;
        currentTowerManager.SpawnTower();

        currentTowerManager.builderVisual.SetActive(false);

        HideButton();
        PlayButtonSfx(towerBuildSfx);
    }

    public void HideButton()
    {
        anim.SetTrigger("Hide");

        // PlayButtonSfx();
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

        PlayButtonSfx(towerUpkeepSfx);

    }

    private void PlayButtonSfx(EventReference sfx)
    {
        if (!sfx.IsNull)
            RuntimeManager.PlayOneShot(sfx);
    }

}
