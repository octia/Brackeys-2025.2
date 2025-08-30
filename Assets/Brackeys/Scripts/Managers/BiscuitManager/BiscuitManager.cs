using Reflex.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BiscuitManager : MonoBehaviour
{
    public BiscuitController biscuitController;

    [Inject, HideInInspector]
    public BiscuitManagerConfig config;

    private int biscuit;
    public int Biscuit
    {
        set
        {
            biscuit = value;
            biscuitController.biscuitText.text = biscuit + " Biscuits";

            if (value <= 0)
            {
                // TODO: Create a death scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        get { return biscuit; }
    }
}
