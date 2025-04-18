using UnityEngine;

public class WeaponPickup : Pickable
{
    [Tooltip("The weapon prefab to be instantiated when picked")]
    public GameObject weaponPrefab;

    public override void Interact()
    {
        // Instantiate the weapon prefab to create an actual object
        GameObject weaponInstance = Instantiate(weaponPrefab, WeaponManager.instance.weaponSlot);
        
        Weapon weapon = weaponInstance.GetComponent<Weapon>();
        if (weapon != null)
        {
            WeaponManager.instance.SetWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
