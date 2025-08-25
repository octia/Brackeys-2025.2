using UnityEngine;

public class BiscuitManager : MonoBehaviour
{
    public BiscuitController biscuitController;

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
