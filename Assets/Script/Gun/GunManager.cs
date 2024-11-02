using System.Collections;
using UnityEngine;

//銃本体の処理を書く

public class GunManager : MonoBehaviour
{
    [SerializeField] private GunParameter gunParameter;             //銃Parameter
    [SerializeField] private Transform Muzzle;                      //発射口
    [SerializeField] private GameObject BulletPrefab;               //弾のプレハブ

    private int CurrentAmmo;                                        //現在の弾数
    private bool IsReloading = false;                               //リロード中かどうか
    private float NextFireTime;                                     //リロード間隔
    private float bulletOffset = 1f;                                //弾丸の生成オフセット距離

    private void Start()
    {
        //ゲーム開始時、弾を補充
        CurrentAmmo = gunParameter.MaxAmmo;
    }

    private bool CanShoot()
    {
        //リロード中でない　＆　弾が残っている ==> trueを返す
        return !IsReloading && CurrentAmmo > 0;
    }

    public void Shoot()
    {
        if (CanShoot() && Time.time >= NextFireTime)
        {
            //発射位置の前方にオフセットを追加
            Vector3 spawnPosition = Muzzle.position + Muzzle.forward * bulletOffset;

            //弾丸を生成し、発射位置に配置
            Instantiate(BulletPrefab, spawnPosition, Muzzle.rotation);
            NextFireTime = Time.time + gunParameter.FireRate;                           //次に発射できる時間を設定
            CurrentAmmo--;
            Debug.Log(CurrentAmmo);                                                     //現在の弾数をデバッグログに表示
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
        //待機時間
        yield return new WaitForSeconds(gunParameter.ReloadTime);
        //リセット
        CurrentAmmo = gunParameter.MaxAmmo;
        IsReloading = false;
        Debug.Log("Reload Complete!");
    }

    public void WaterReload()
    {
        CurrentAmmo = gunParameter.MaxAmmo;
    }
}