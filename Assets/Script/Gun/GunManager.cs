using System.Collections;
using UnityEngine;

/// <summary>
/// 銃本体に関するクラス
/// </summary>
//TODO:プレーヤーが加速するのはなぜ？
public class GunManager : MonoBehaviour
{
    [SerializeField] private GunParameter gunParameter;
    [SerializeField] private Transform muzzle;                      //発射口
    [SerializeField] private GameObject bulletPrefab;
    private int currentAmmo;
    private bool isReloading;
    private float fireCooldown;                                     //発射のクールダウン時間
    private float bulletOffset;

    private void Start()
    {
        //ゲーム開始時、弾を補充
        currentAmmo = gunParameter.MaxAmmo;
        isReloading = false;
        bulletOffset = 1f;
        fireCooldown = 0f;                                          //初期化
    }

    private bool CanShoot()
    {
        //リロード中でない ＆ 弾が残っている ==> trueを返す
        return !isReloading && currentAmmo > 0;
    }

    private void Update()
    {
        //クールダウンを減少させる
        if (fireCooldown > 0) fireCooldown -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (CanShoot() && fireCooldown <= 0f)
        {
            Vector3 spawnPosition = muzzle.position + muzzle.forward * bulletOffset;
            Instantiate(bulletPrefab, spawnPosition, muzzle.rotation);

            //クールダウンを挟む
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

        //リロード時間の待機
        yield return new WaitForSeconds(gunParameter.ReloadTime);

        currentAmmo = gunParameter.MaxAmmo;
        isReloading = false;
        Debug.Log("Reload Complete!");
    }

    /// <summary>
    /// 噴水エリア用メソッド
    /// </summary>
    public void WaterReload()
    {
        currentAmmo = gunParameter.MaxAmmo;
    }
}