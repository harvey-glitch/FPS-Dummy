using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public abstract class Weapon : MonoBehaviour
{
    public enum Category {primary, secondary, melee}

    [Tooltip("List of weapon categories")]
    public Category category;

    [Tooltip("Holds the weapon statistic")]
    public WeaponData weaponData;

    [Tooltip("Reload animation reference to play")]
    public AnimationClip reloadClip;

    private Animation _animation;

    private int _currentAmmo = 0;
    private float _lastFireTime = 0.0f;

    private void Start()
    {
        _animation = GetComponent<Animation>();
        
        if (_animation != null)
        _animation.AddClip(reloadClip, "Reload");
    }

    public void InitializeWeapon()
    {
        _currentAmmo = weaponData.maxAmmo;
        _lastFireTime = Time.time;
    }

    public virtual void Fire()
    {
        _currentAmmo--;
        _lastFireTime = Time.time;

        if (_currentAmmo == 0)
        {
            StartCoroutine(StartReload());
        }
    }

    public bool CanFire()
    {
        return _currentAmmo > 0 && Time.time >= _lastFireTime + weaponData.firerate;
    }

    protected IEnumerator StartReload()
    {
        float reloadTime = weaponData.reloadSpeed;
        float animationLength = reloadClip.length;

        float playbackSpeed = animationLength / reloadTime;
        _animation["Reload"].speed = playbackSpeed;
        _animation.Play("Reload");

        yield return new WaitForSeconds(reloadTime);

        _currentAmmo = weaponData.maxAmmo;
    }
}