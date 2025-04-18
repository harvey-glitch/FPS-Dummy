using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    public void Update()
    {
        CalculateRecoilOffset();
    }

    public void CalculateRecoilOffset()
    {
        if (Input.GetMouseButton(0) && WeaponManager.instance.isWeaponFired())
        {
            WeaponData weaponData = WeaponManager.instance.currentWeapon.weaponData;

            // create random horizontal rotation (y)
            float rotationY = Random.Range(-weaponData.horizontalRecoil, weaponData.horizontalRecoil);
            float rotationX = -Mathf.Abs(weaponData.verticalRecoil); // upward rotation

            Vector3 newPosition = Vector3.back * weaponData.recoilKickback;
            Vector3 newRotation = new Vector3(rotationX, rotationY, 0.0f);

            WeaponAnimator.instance.AddRecoilOffset(newPosition, newRotation);
        }
    }
}