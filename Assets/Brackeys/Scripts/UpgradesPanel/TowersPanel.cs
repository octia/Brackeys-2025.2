using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowersPanel : MonoBehaviour
{
    [SerializeField] TowerScriptableObject[] buyableTowersData;

    [Space]
    [SerializeField] GameObject towerInfoUI;
    [SerializeField] GameObject buyableTowersUI;
    [SerializeField] GameObject buyableTowerButtonInstance;

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
    }

    public void HideButton()
    {
        anim.SetTrigger("Hide");
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
        }

    }
    public void RepairTower()
    {
        if (biscuitManager.Biscuit - currentTowerManager.towerData.levels[currentTowerManager.currentLevel].repairCost < 1)
        {
            return;
        }
        biscuitManager.Biscuit -= Mathf.RoundToInt(currentTowerManager.towerData.levels[currentTowerManager.currentLevel].repairCost);
        currentTowerManager.RepairHealth();
    }
}
