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
            Debug.Log("Current Ammo: " + CurrentAmmo);                                  //���݂̒e�����f�o�b�O���O�ɕ\��
            GameObject bullet = Instantiate(BulletPrefab, Muzzle.position, Muzzle.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Muzzle.forward * playerParameter.AttackSpeed;             //���ˑ��x�Œe��O���ɔ�΂�
            }
            Destroy(bullet, playerParameter.AttackRange / playerParameter.AttackSpeed); //�˒������ɒB����ƒe������
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
        Debug.Log("GO�I�I�I");
    }
}