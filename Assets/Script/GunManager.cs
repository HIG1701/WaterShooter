using System.Collections;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] Transform Muzzle;                      //���ˌ�
    [SerializeField] GameObject BulletPrefab;               //�e�̃v���n�u
    [SerializeField] PlayerParameter playerParameter;       //�v���C���[Parameter

    private int CurrentAmmo;                                //���݂̒e��
    private bool IsReloading = false;                       //�����[�h�����ǂ���

    private void Start()
    {
        //�Q�[���J�n���A�ʂ��[
        CurrentAmmo = playerParameter.MaxAmmo;
    }

    public bool CanShoot()
    {
        //�����[�h���łȂ��@���@�ʂ��c���Ă���
        return !IsReloading && CurrentAmmo > 0;
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            CurrentAmmo--;
            Instantiate(BulletPrefab, Muzzle.position, Muzzle.rotation);
        }
    }

    public IEnumerator Reload()
    {
        IsReloading = true;
        Debug.Log("Reloading...");
        //�ҋ@����
        yield return new WaitForSeconds(playerParameter.ReloadSpeed);
        //���Z�b�g
        CurrentAmmo = playerParameter.MaxAmmo;
        IsReloading = false;
    }
}