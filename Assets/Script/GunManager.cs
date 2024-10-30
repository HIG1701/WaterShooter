using System.Collections;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] Transform Muzzle;                      //発射口
    [SerializeField] GameObject BulletPrefab;               //弾のプレハブ
    [SerializeField] GunParameter gunParameter;             //銃Parameter

    private int CurrentAmmo;                                //現在の弾数
    private bool IsReloading = false;                       //リロード中かどうか

    private void Start()
    {
        //ゲーム開始時、弾を補充
        CurrentAmmo = gunParameter.MaxAmmo;
    }

    public bool CanShoot()
    {
        //リロード中でない　＆　弾が残っている
        return !IsReloading && CurrentAmmo > 0;
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            CurrentAmmo--;
            Debug.Log(CurrentAmmo);                                      //現在の弾数をデバッグログに表示
            GameObject bullet = Instantiate(BulletPrefab, Muzzle.position, Muzzle.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            //発射速度で弾を前方に飛ばす
            if (rb != null) rb.velocity = Muzzle.forward * gunParameter.AttackSpeed;
            Destroy(bullet, gunParameter.AttackRange / gunParameter.AttackSpeed);     //射程距離に達すると弾を消す
        }
    }

    public IEnumerator Reload()
    {
        IsReloading = true;
        Debug.Log("Reloading...");
        //待機時間
        yield return new WaitForSeconds(gunParameter.ReloadSpeed);
        //リセット
        CurrentAmmo = gunParameter.MaxAmmo;
        IsReloading = false;
        Debug.Log("Reload Complete!");
    }
}