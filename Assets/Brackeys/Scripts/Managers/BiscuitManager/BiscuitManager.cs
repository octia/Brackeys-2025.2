using Reflex.Attributes;
using UnityEngine;

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
        }
        get { return biscuit; }
    }
}
