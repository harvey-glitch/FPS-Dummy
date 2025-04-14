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

    private Dictionary<Weapon.Category, Weapon> _equippedWeapons = new();
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
        // Destroy old weapon if one already exists in the same slot
        if (_equippedWeapons.ContainsKey(newWeapon.category) && _equippedWeapons[newWeapon.category] != null)
        {
            Destroy(_equippedWeapons[newWeapon.category].gameObject);
        }

        _equippedWeapons[newWeapon.category] = newWeapon;
        EquipWeapon(newWeapon.category);
    }

    private void EquipWeapon(Weapon.Category category)
    {
        if (currentWeapon != null)
        {
            currentWeapon.gameObject.SetActive(false);
        }

        currentWeapon = _equippedWeapons[category];
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.InitializeWeapon();
    }
}