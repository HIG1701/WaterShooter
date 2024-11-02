using System.Collections;
using UnityEngine;

//�e�{�̂̏���������

public class GunManager : MonoBehaviour
{
    [SerializeField] private GunParameter gunParameter;             //�eParameter
    [SerializeField] private Transform Muzzle;                      //���ˌ�
    [SerializeField] private GameObject BulletPrefab;               //�e�̃v���n�u
    private int CurrentAmmo;                                        //���݂̒e��
    private bool IsReloading;                                       //�����[�h�����ǂ���
    private float NextFireTime;                                     //�����[�h�Ԋu
    private float bulletOffset;                                     //�e�ۂ̐����I�t�Z�b�g����

    private void Start()
    {
        //�Q�[���J�n���A�e���[
        CurrentAmmo = gunParameter.MaxAmmo;
        IsReloading = false;
        bulletOffset = 1f;
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
        //�R���[�`�����쓮�����郁�\�b�h
        //���X�N���v�g�ŌĂяo���ׂɍ��܂���
        //!CanShoot�FReload�����ʂ��Ȃ����
        if (!CanShoot()) StartCoroutine(Reload());
    }

    private IEnumerator Reload()
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