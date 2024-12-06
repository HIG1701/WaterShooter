using System.Collections;
using UnityEngine;

/// <summary>
/// �e�{�̂Ɋւ���N���X
/// </summary>
//TODO:�v���[���[����������̂͂Ȃ��H
public class GunManager : MonoBehaviour
{
    [SerializeField] private GunParameter gunParameter;
    [SerializeField] private Transform muzzle;                      //���ˌ�
    [SerializeField] private GameObject bulletPrefab;
    private int currentAmmo;
    private bool isReloading;
    private float fireCooldown;                                     //���˂̃N�[���_�E������
    private float bulletOffset;

    private void Start()
    {
        //�Q�[���J�n���A�e���[
        currentAmmo = gunParameter.MaxAmmo;
        isReloading = false;
        bulletOffset = 1f;
        fireCooldown = 0f;                                          //������
    }

    private bool CanShoot()
    {
        //�����[�h���łȂ� �� �e���c���Ă��� ==> true��Ԃ�
        return !isReloading && currentAmmo > 0;
    }

    private void Update()
    {
        //�N�[���_�E��������������
        if (fireCooldown > 0) fireCooldown -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (CanShoot() && fireCooldown <= 0f)
        {
            Vector3 spawnPosition = muzzle.position + muzzle.forward * bulletOffset;
            Instantiate(bulletPrefab, spawnPosition, muzzle.rotation);

            //�N�[���_�E��������
            fireCooldown = gunParameter.FireRate;
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

        //�����[�h���Ԃ̑ҋ@
        yield return new WaitForSeconds(gunParameter.ReloadTime);

        currentAmmo = gunParameter.MaxAmmo;
        isReloading = false;
        Debug.Log("Reload Complete!");
    }

    /// <summary>
    /// �����G���A�p���\�b�h
    /// </summary>
    public void WaterReload()
    {
        currentAmmo = gunParameter.MaxAmmo;
    }
}