using System.Collections;
using UnityEngine;

/// <summary>
/// �e�{�̂Ɋւ���N���X
/// </summary>
public class GunManager : MonoBehaviour
{
    [SerializeField] private GunParameter gunParameter;             //�eParameter
    [SerializeField] private Transform muzzle;                      //���ˌ�
    [SerializeField] private GameObject bulletPrefab;               //�e�̃v���n�u
    private int currentAmmo;                                        //���݂̒e��
    private bool isReloading;                                       //�����[�h�����ǂ���
    private float nextFireTime;                                     //�����[�h�Ԋu
    private float bulletOffset;                                     //�e�ۂ̐����I�t�Z�b�g����

    private void Start()
    {
        //�Q�[���J�n���A�e���[
        currentAmmo = gunParameter.MaxAmmo;
        isReloading = false;
        bulletOffset = 1f;
    }

    private bool CanShoot()
    {
        //�����[�h���łȂ��@���@�e���c���Ă��� ==> true��Ԃ�
        return !isReloading && currentAmmo > 0;
    }

    public void Shoot()
    {
        if (CanShoot() && Time.time >= nextFireTime)
        {
            //���ˈʒu�̑O���ɃI�t�Z�b�g��ǉ�
            Vector3 spawnPosition = muzzle.position + muzzle.forward * bulletOffset;

            //�e�ۂ𐶐����A���ˈʒu�ɔz�u
            //TODO:Time.time�ɂ��čl����
            Instantiate(bulletPrefab, spawnPosition, muzzle.rotation);
            nextFireTime = Time.time + gunParameter.FireRate;
            currentAmmo--;
            Debug.Log(currentAmmo);
        }
    }

    public void StartReload()
    {
        if (!CanShoot()) StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        //�ҋ@����
        yield return new WaitForSeconds(gunParameter.ReloadTime);
        //���Z�b�g
        currentAmmo = gunParameter.MaxAmmo;
        isReloading = false;
        Debug.Log("Reload Complete!");
    }

    public void WaterReload()
    {
        currentAmmo = gunParameter.MaxAmmo;
    }
}