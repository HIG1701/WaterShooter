using System.Collections;
using UnityEngine;

/// <summary>
/// 銃本体に関するクラス
/// </summary>

//TODO:インパルスを使うとよりいいらしい
public class GunManager : MonoBehaviour
{
    [SerializeField] private GunParameter gunParameter;
    [SerializeField] private Transform muzzle;                      //発射口
    [SerializeField] private GameObject bulletPrefab;
    private int currentAmmo;
    private bool isReloading;
    private float nextFireTime;
    private float bulletOffset;

    private void Start()
    {
        //ゲーム開始時、弾を補充
        currentAmmo = gunParameter.MaxAmmo;
        isReloading = false;
        bulletOffset = 1f;
    }

    private bool CanShoot()
    {
        //リロード中でない　＆　弾が残っている ==> trueを返す
        return !isReloading && currentAmmo > 0;
    }

    public void Shoot()
    {
        if (CanShoot() && Time.time >= nextFireTime)
        {
            //発射位置の前方にオフセットを追加
            Vector3 spawnPosition = muzzle.position + muzzle.forward * bulletOffset;

            //弾丸を生成し、発射位置に配置
            //TODO:Time.timeについて考える
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
        //待機時間
        yield return new WaitForSeconds(gunParameter.ReloadTime);
        //リセット
        currentAmmo = gunParameter.MaxAmmo;
        isReloading = false;
        Debug.Log("Reload Complete!");
    }

    public void WaterReload()
    {
        currentAmmo = gunParameter.MaxAmmo;
    }
}