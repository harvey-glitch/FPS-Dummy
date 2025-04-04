using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("AUDIO")]
    [SerializeField] AudioClip gunShotAudio; // Reference to the audio clip
    [SerializeField] AudioSource audioSource; // Reference to the audio source
    [SerializeField] public WeaponData weaponData; // Reference to the weapon data
    [HideInInspector] protected int currentAmmo; // Tracks the current ammo of the weapon
    protected float nextFireTime; // Tracks when the weapon can be fire again

    public void Initialize(WeaponData data)
    {
        weaponData = data;
        currentAmmo = weaponData.maxAmmo;
    }

    // Abstract virtual for firing projectiles
    public abstract void Fire();

    public bool CanFire()
    {
        // Check if the weapon can be fire
        return Time.time >= nextFireTime && currentAmmo > 0;
    }

    protected IEnumerator ReloadWeapon()
    {
        // Track the time elapsed during the reload process
        float timeElapsed = 0f;

        while (timeElapsed <= weaponData.reloadSpeed)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Reset the current ammo
        currentAmmo = weaponData.maxAmmo;
    }

    public void PlayAudio()
    {
        if (gunShotAudio != null)
        {
            AudioManager.instance.PlayAudioOnce(audioSource, gunShotAudio);
        }
    }
}