using UnityEngine;

public class BotAim : MonoBehaviour
{
    [Header("ターゲットの設定")]
    public Transform target; // BOTがエイムするターゲット（プレイヤーなど）

    [Header("エイムの速度と精度")]
    public float rotationSpeed = 5f; // エイムの回転速度
    public float aimAccuracy = 0.95f; // エイムの精度 (0 ~ 1) 1なら完璧に追尾

    [Header("可視化の設定")]
    public LineRenderer aimLine; // 赤い線を描画するLineRenderer
    public float lineLength = 10f; // 赤い線の長さ

    private void Start()
    {
        if (aimLine == null)
        {
            // LineRendererがアタッチされていなかったら自動生成
            aimLine = gameObject.AddComponent<LineRenderer>();
            aimLine.startWidth = 0.05f;
            aimLine.endWidth = 0.05f;
            aimLine.material = new Material(Shader.Find("Sprites/Default"));
            aimLine.startColor = Color.red;
            aimLine.endColor = Color.red;
        }
    }

    private void Update()
    {
        AimAtTarget();
        DrawAimLine();
    }

    /// <summary>
    /// ターゲットの方向を向く
    /// </summary>
    private void AimAtTarget()
    {
        if (target == null) return;

        // ターゲットの方向を計算
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        // 現在のBOTの向きを取得
        Quaternion currentRotation = transform.rotation;

        // ターゲットの方向を向くための回転を計算
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // 回転をスムーズに補間
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed * aimAccuracy);
    }

    /// <summary>
    /// エイムの方向を赤い線で表示する
    /// </summary>
    private void DrawAimLine()
    {
        if (aimLine == null) return;

        // BOTの前方（forward）の方向を計算
        Vector3 lineStart = transform.position + Vector3.up * 1.5f; // BOTの少し上から線を描画
        Vector3 lineEnd = lineStart + transform.forward * lineLength;

        // LineRendererで線を描画
        aimLine.positionCount = 2;
        aimLine.SetPosition(0, lineStart);
        aimLine.SetPosition(1, lineEnd);
    }
}
