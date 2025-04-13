using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    [Tooltip("Damage output of the weapon")]
    public float damage;

    [Tooltip("How fast the weapon can shoot")]
    public float firerate;

    [Tooltip("Max number of ammo a weapn can have")]
    public int maxAmmo;

    [Tooltip("How fast the weapon reloads")]
    public float reloadSpeed;

    [Tooltip("How strong the weapon kickback when fired")]
    public float recoilStrength;

    [Tooltip("Rotation offset of the recoil [optional]")]
    public float recoilOffset;

    [Tooltip("Number of bullet per shot [for shotgun only]")]
    public int pellets;

    [Tooltip("Max angle the bullet can spread [for shotgun only] ")]
    public float spreadAngle;
}