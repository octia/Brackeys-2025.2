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
            biscuit = Mathf.Clamp(value, 0, maxBiscuits);
            biscuitController.biscuitText.text = biscuit + "/" + maxBiscuits + " Biscuits";

            if (value == 0)
            {
                // TODO: Create a death scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        get { return biscuit; }
    }
}
