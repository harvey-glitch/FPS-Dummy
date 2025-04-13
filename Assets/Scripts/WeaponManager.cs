using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    [Tooltip("Holds the current primary weapon")]
    public Weapon primaryWeapon;

    [Tooltip("Holds the current primary weapon")]
    public Weapon secondaryWeapon;

    [Tooltip("Holds the current equipped weapon")]
    public Weapon currentWeapon;

    [Tooltip("Transform where the weapons are place")]
    public Transform weaponSlot;

    private Weapon[] _equippedWeapons;
    private int _currentIndex = 0;

    #region Singleton
    private void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion


    private void Update()
    {
        FireWeapon();
    }
    
    private void FireWeapon()
    {
        // only fire when theres valid equipped weapon
        if (Input.GetMouseButton(0) && currentWeapon != null)
        {
            currentWeapon.Fire();
        }
    }

    public void SetWeapon(Weapon newWeapon)
    {
        // store the new weapon based on its category
        switch (newWeapon.category)
        {
            case Weapon.Category.primary:
                primaryWeapon = newWeapon;
                break;
            case Weapon.Category.secondary:
                secondaryWeapon = newWeapon;
                break;
        }

        UpdateWeapons();
        EquipWeapon(_currentIndex); // re-equip after updating weapons
    }

    private void UpdateWeapons()
    {
        // create a new list of currently equipped weapons
        List<Weapon> list = new List<Weapon>();

        if (primaryWeapon != null) list.Add(primaryWeapon);
        if (secondaryWeapon != null) list.Add(secondaryWeapon);

        // update the array of equipped weapons based on new list of weapons
        _equippedWeapons = list.ToArray();
        _currentIndex = Mathf.Clamp(_currentIndex, 0, _equippedWeapons.Length - 1);
    }

    private void SwitchWeapon(int direction)
    {
        // calculate the next weapon index in a circular manner
        if (_equippedWeapons.Length != 0)
        {
            _currentIndex = (_currentIndex + direction + _equippedWeapons.Length) % _equippedWeapons.Length;
            EquipWeapon(_currentIndex);
        }
    }

    private void EquipWeapon(int index)
    {
        // deactivate the currently equipped weapon
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }

        // equip the new weapon at the specified index
        currentWeapon = _equippedWeapons[index];
        currentWeapon.gameObject.SetActive(true);

        currentWeapon.InitializeWeapon();
    }
}