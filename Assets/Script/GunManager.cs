using System.Collections;
using UnityEngine;

//�e�{�̂̏���������

public class GunManager : MonoBehaviour
{
    [SerializeField] private GunParameter gunParameter;             //�eParameter
    [SerializeField] private Transform Muzzle;                      //���ˌ�
    [SerializeField] private GameObject BulletPrefab;               //�e�̃v���n�u

    private int CurrentAmmo;                                        //���݂̒e��
    private bool IsReloading = false;                               //�����[�h�����ǂ���
    private float NextFireTime;                                     //�����[�h�Ԋu
    private float bulletOffset = 1f;                                //�e�ۂ̐����I�t�Z�b�g����

    private void Start()
    {
        //�Q�[���J�n���A�e���[
        CurrentAmmo = gunParameter.MaxAmmo;
    }

    private bool CanShoot()
    {
        //�����[�h���łȂ��@���@�e���c���Ă��� ==> true��Ԃ�
        return !IsReloading && CurrentAmmo > 0;
    }

    public void Shoot()
    {
        if (CanShoot() && Time.time >= NextFireTime)
        {
            //���ˈʒu�̑O���ɃI�t�Z�b�g��ǉ�
            Vector3 spawnPosition = Muzzle.position + Muzzle.forward * bulletOffset;

            //�e�ۂ𐶐����A���ˈʒu�ɔz�u
            Instantiate(BulletPrefab, spawnPosition, Muzzle.rotation);
            NextFireTime = Time.time + gunParameter.FireRate;                           //���ɔ��˂ł��鎞�Ԃ�ݒ�
            CurrentAmmo--;
            Debug.Log(CurrentAmmo);                                                     //���݂̒e�����f�o�b�O���O�ɕ\��
        }
    }

    public void StartReload()
    {
        if (!CanShoot()) StartCoroutine(Reload());
    }

    public IEnumerator Reload()
    {
        IsReloading = true;
        Debug.Log("Reloading...");
        //�ҋ@����
        yield return new WaitForSeconds(gunParameter.ReloadTime);
        //���Z�b�g
        CurrentAmmo = gunParameter.MaxAmmo;
        IsReloading = false;
        Debug.Log("Reload Complete!");
    }

    public void WaterReload()
    {
        CurrentAmmo = gunParameter.MaxAmmo;
    }
}