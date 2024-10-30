using System.Collections;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] Transform Muzzle;                      //���ˌ�
    [SerializeField] GameObject BulletPrefab;               //�e�̃v���n�u
    [SerializeField] GunParameter gunParameter;             //�eParameter

    private int CurrentAmmo;                                //���݂̒e��
    private bool IsReloading = false;                       //�����[�h�����ǂ���

    private void Start()
    {
        //�Q�[���J�n���A�e���[
        CurrentAmmo = gunParameter.MaxAmmo;
    }

    public bool CanShoot()
    {
        //�����[�h���łȂ��@���@�e���c���Ă���
        return !IsReloading && CurrentAmmo > 0;
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            CurrentAmmo--;
            Debug.Log(CurrentAmmo);                                      //���݂̒e�����f�o�b�O���O�ɕ\��
            GameObject bullet = Instantiate(BulletPrefab, Muzzle.position, Muzzle.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            //���ˑ��x�Œe��O���ɔ�΂�
            if (rb != null) rb.velocity = Muzzle.forward * gunParameter.AttackSpeed;
            Destroy(bullet, gunParameter.AttackRange / gunParameter.AttackSpeed);     //�˒������ɒB����ƒe������
        }
    }

    public IEnumerator Reload()
    {
        IsReloading = true;
        Debug.Log("Reloading...");
        //�ҋ@����
        yield return new WaitForSeconds(gunParameter.ReloadSpeed);
        //���Z�b�g
        CurrentAmmo = gunParameter.MaxAmmo;
        IsReloading = false;
        Debug.Log("Reload Complete!");
    }
}