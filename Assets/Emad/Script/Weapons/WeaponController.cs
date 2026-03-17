using UnityEngine;
using UnityEngine.InputSystem;


public class WeaponInputHandler : MonoBehaviour
{
    public IWeapon currentWeapon;

    private PlayerInputActions input;

    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Fire.started += ctx => currentWeapon?.OnFireStarted();
        input.Player.Fire.canceled += ctx => currentWeapon?.OnFireCanceled();
        input.Player.Reload.performed += ctx => currentWeapon?.OnReload();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}