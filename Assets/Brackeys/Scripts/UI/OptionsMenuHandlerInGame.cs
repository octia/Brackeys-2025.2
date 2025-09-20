using UnityEngine;
using UnityEngine.InputSystem;

public class OptionsMenuHandlerInGame : MonoBehaviour
{
    [SerializeField] InputActionAsset actions;
    InputAction toogleMenu;
    [SerializeField] GameObject MenuObject;


    void OnEnable()
    {
        var map = actions.FindActionMap("EscapeTab");



#if UNITY_WEBGL
    toogleMenu = map.FindAction("TAB");
#else
        toogleMenu = map.FindAction("ESC");
#endif
        toogleMenu.performed += OnEscape;
        toogleMenu.Enable();

    }

    void OnDisable()
    {
        toogleMenu.performed -= OnEscape;
        toogleMenu.Disable();
    }

    void OnEscape(InputAction.CallbackContext ctx)
    {
        MenuObject.SetActive(!MenuObject.activeSelf);
    }
}
