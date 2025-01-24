using System.Collections;
using TMPro;
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
    [SerializeField] private PlayMusic playMusic;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    private int currentAmmo;
    private bool isReloading;
    private float fireCooldown;                                     //発射のクールダウン時間
    private float bulletOffset;

    private void Start()
    {
        //ゲーム開始時、弾を補充
        currentAmmo = gunParameter.MaxAmmo;
        isReloading = false;
        bulletOffset = 20f;
        fireCooldown = 0f;                                          //初期化
    }

    public bool CanShoot()
    {
        //リロード中でない ＆ 弾が残っている ==> trueを返す
        return !isReloading && currentAmmo > 0;
    }

    private void Update()
    {
        SendCurrentAmmo();
        //クールダウンを減少させる
        if (fireCooldown > 0) fireCooldown -= Time.deltaTime;
    }

    private void SendCurrentAmmo()
    {
        if (gameObject.GetComponentInParent<PlayerController>() != null)
        {
            PlayerManager.parameters[gameObject.GetComponentInParent<PlayerController>().CharacterIndex].playerRuntimeDate.date.currentAmmo = currentAmmo;
        }
        else if (gameObject.GetComponentInParent<AIControl>() != null)
        {
            PlayerManager.parameters[gameObject.GetComponentInParent<AIControl>().CharacterIndex].playerRuntimeDate.date.currentAmmo = currentAmmo;
        }
    }

    public void Shoot()
    {
        if (CanShoot() && fireCooldown <= 0f)
        {
            playMusic.OnPlayMusic();
            Vector3 spawnPosition = muzzle.position + muzzle.forward * bulletOffset;

            GameObject bulletObject = Instantiate(bulletPrefab, spawnPosition, muzzle.rotation);
            if (gameObject.GetComponentInParent<PlayerController>() != null)
            {
                bulletObject.GetComponent<Bullet>().CharaIndex = gameObject.GetComponentInParent<PlayerController>().CharacterIndex;
                SetDateManager.TotalFireCount(gameObject.GetComponentInParent<PlayerController>().CharacterIndex);
            }
            else if (gameObject.GetComponentInParent<AIControl>() != null)
            {
                bulletObject.GetComponent<Bullet>().CharaIndex = gameObject.GetComponentInParent<AIControl>().CharacterIndex;
                SetDateManager.TotalFireCount(gameObject.GetComponentInParent<AIControl>().CharacterIndex);
            }

            //クールダウンを挟む
            fireCooldown = gunParameter.FireRate;
            currentAmmo--;
        }
    }

    public void StartReload()
    {
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        if (textMeshProUGUI != null) textMeshProUGUI.SetText("Reloading...");
        //リロード時間の待機
        yield return new WaitForSeconds(gunParameter.ReloadTime);

        currentAmmo = gunParameter.MaxAmmo;
        isReloading = false;
        Debug.Log("Reload Complete!");
        if (textMeshProUGUI != null) textMeshProUGUI.SetText("Reload Complete!");
        yield return new WaitForSeconds(gunParameter.ReloadTime);
        if (textMeshProUGUI != null) textMeshProUGUI.SetText("");
    }

    /// <summary>
    /// 噴水エリア用メソッド
    /// </summary>
    public void WaterReload()
    {
        currentAmmo = gunParameter.MaxAmmo;
    }
}