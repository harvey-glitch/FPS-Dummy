using UnityEngine;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
    [SerializeField] Transform weaponslot;

    // Current weapon being held by the player
    private Weapon currentWeapon;

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
    void Start()
    {
        //  Get the child object of weapon slot weapon
        currentWeapon = weaponslot.GetChild(0).GetComponent<Weapon>();

        if (currentWeapon != null)
        {
            currentWeapon.Initialize(currentWeapon.weaponData);
        }
    }

    void Update()
    {
        // Check if the user fired and if theres valid weapon
        if (currentWeapon != null && Fired())
        {
            currentWeapon.Fire();
        }
    }

    // Method that tracks if the user fired the weapon
    public bool Fired()
    {
        return Input.GetMouseButton(0) && currentWeapon.CanFire();
    }

    // Method to equip new weapon
    public void EquipWeapon(GameObject weaponPrefab, WeaponData weaponData)
    {
        // Destroy the current weapon instance if it exists
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        // Instantiate the new weapon prefab and set it as a child of the weapon slot
        GameObject newWeapon = Instantiate(weaponPrefab, weaponslot.transform);

        // Get the Weapon component from the newly instantiated weapon
        currentWeapon = newWeapon.GetComponent<Weapon>();

        // Initialize the new weapon if it exists
        if (currentWeapon != null)
            currentWeapon.Initialize(weaponData);
    }
}