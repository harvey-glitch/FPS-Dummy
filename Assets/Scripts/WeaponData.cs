using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    [Header("General Setting")]
    public float damage;
    public float firerate;
    public int maxAmmo;
    public float reloadSpeed;

    [Header("For Shotgun Only")]
    public int pellets;
    public float spreadAngle;
}