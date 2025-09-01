using Reflex.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BiscuitManager : MonoBehaviour
{
    public BiscuitController biscuitController;


    [Inject, HideInInspector]
    public BiscuitManagerConfig config;

    public int maxBiscuits;

    private int biscuit;


    public int Biscuit
    {
        set
        {
            biscuit = Mathf.Clamp(value, int.MinValue, maxBiscuits);
            biscuitController.biscuitText.text = biscuit + "/" + maxBiscuits;

            biscuitController.biscuitText.text = biscuit + " / " + maxBiscuits;
            float fillAmount = (float)biscuit / maxBiscuits;
            biscuitController.biscuitBar.fillAmount = fillAmount;
            if (value <= 0)
            {
                // TODO: Create a death scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        get { return biscuit; }
    }


}
