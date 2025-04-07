using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    [Header("WEAPONS")]
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;
    public Weapon meleeWeapon;

    [Header("SLOT")]
    public Transform weaponSlot;

    private Weapon[] equippedWeapons;
    private int currentIndex = 0;

    // Current weapon being held by the player
    [HideInInspector] public Weapon currentWeapon;

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
        FireCurrentWeapon();

        if (Input.mouseScrollDelta.y > 0)
            SwitchWeapon(1);
        else if (Input.mouseScrollDelta.y < 0)
            SwitchWeapon(-1);
    }

    
    private void FireCurrentWeapon()
    {
        if (Input.GetMouseButton(0) && ReadyToFire())
            currentWeapon.Fire();
    }

    public bool ReadyToFire()
    {
        // Tracks if the user fired the weapon
        return currentWeapon != null && currentWeapon.CanFire();
    }

    public void SetWeapon(Weapon newWeapon)
    {
        switch (newWeapon.category)
        {
            case Weapon.Category.primary:
                primaryWeapon = newWeapon;
                break;
            case Weapon.Category.secondary:
                secondaryWeapon = newWeapon;
                break;
            case Weapon.Category.melee:
                meleeWeapon = newWeapon;
                break;
        }

        UpdateEquippedWeapons();
        EquipWeapon(currentIndex); // Re-equip to update current weapon
    }

    private void UpdateEquippedWeapons()
    {
        List<Weapon> list = new List<Weapon>();

        if (primaryWeapon != null) list.Add(primaryWeapon);
        if (secondaryWeapon != null) list.Add(secondaryWeapon);
        if (meleeWeapon != null) list.Add(meleeWeapon);

        equippedWeapons = list.ToArray();
        currentIndex = Mathf.Clamp(currentIndex, 0, equippedWeapons.Length - 1);
    }

    private void SwitchWeapon(int direction)
    {
        if (equippedWeapons.Length == 0) return;

        currentIndex = (currentIndex + direction + equippedWeapons.Length) % equippedWeapons.Length;
        EquipWeapon(currentIndex);
    }

    private void EquipWeapon(int index)
    {
        if (currentWeapon != null)
            currentWeapon.gameObject.SetActive(false);

        currentWeapon = equippedWeapons[index];
        currentWeapon.gameObject.SetActive(true);

        currentWeapon.transform.SetParent(weaponSlot);
        currentWeapon.InitializeWeapon();
    }
}