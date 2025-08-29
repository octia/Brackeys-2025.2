using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowersPanel : MonoBehaviour
{
    [SerializeField] TowerScriptableObject[] buyableTowersData;

    [Space]
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
            newBuyableTowerButton.GetComponentsInChildren<TMP_Text>()[1].text = towerData.towerCost + " Biscuits";
            newBuyableTowerButton.GetComponentInChildren<RawImage>().texture = towerData.icon;
        }
    }

    public void ShowButton(TowerManager towerManager)
    {
        anim.SetTrigger("Show");

        currentTowerManager = towerManager;

        if (towerManager.spawned)
        {
            buyableTowersUI.SetActive(false);
            // TODO: Upgrade for the towers
        }
        else
        {
            buyableTowersUI.SetActive(true);
        }
    }

    public void BuyTower(TowerScriptableObject towerData)
    {
        if (biscuitManager.Biscuit - towerData.towerCost < 1)
        {
            return;
        }

        biscuitManager.Biscuit -= towerData.towerCost;
        currentTowerManager.towerData = towerData;
        currentTowerManager.SpawnTower();

        currentTowerManager.builderVisual.SetActive(false);

        HideButton();
    }

    public void HideButton()
    {
        anim.SetTrigger("Hide");
    }
}
