using UnityEngine;

public class WeaponPickup : Pickable
{
    [SerializeField] GameObject weaponPrefab;

    public override void Interact()
    {
        Weapon weapon = weaponPrefab.GetComponent<Weapon>();
        if (weapon != null)
        {
            WeaponManager.instance.EquipWeapon(weaponPrefab, weapon.weaponData);
            Destroy(gameObject);
        }
    }
}
