using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;

public class BotAim : MonoBehaviour
{
    //[Header("ターゲットの設定")]
    //public Transform target;

    [Header("エイムの速度と精度")]
    public float adsSpeed = 5f;
    public float aimLevel = 0.95f;

    [Header("可視化の設定")]
    public LineRenderer aimLine;
    public float lineLength = 10f;

    private void Start()
    {
        if (aimLine == null)
        {
            aimLine = gameObject.AddComponent<LineRenderer>();
            aimLine.startWidth = 0.05f;
            aimLine.endWidth = 0.05f;
            aimLine.material = new Material(Shader.Find("Sprites/Default"));
            aimLine.startColor = Color.red;
            aimLine.endColor = Color.red;
        }
    }

    /// <summary>
    /// ターゲットの方向を向く
    /// </summary>
    public void AimAtTarget(Transform target)
    {
        bool isAimStop = false;
        target = transform.GetComponent<AIControl>().SendInSightPlayerObject;
        Debug.Log(transform.GetComponent<AIControl>().SendPlayerInSightCheck());
        if (target == null || !transform.gameObject.GetComponent<AIControl>().SendPlayerInSightCheck())
        {
            this.transform.rotation = this.transform.parent.transform.rotation;
            return;
        }

        Vector3 characterAimArea = (target.transform.position - transform.position).normalized;
        float viewAngleTarget = Vector3.Angle(transform.forward, transform.parent.forward);

        Vector3 targetPosition = target.GetComponent<Collider>().bounds.center;
        Vector3 targetObject = (targetPosition - transform.position).normalized;

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(targetObject);

        //ここのifで視野角こえたら上のreturn.書き直し予定
        if (viewAngleTarget > this.transform.GetComponent<AIControl>().SendViewAngle() / 2 && !isAimStop)
        {
            isAimStop = true;
            return;
        }
        else if (isAimStop)
        {
            isAimStop = false;
        }
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * adsSpeed * aimLevel);
    }

    /// <summary>
    /// エイムの方向を赤い線で表示する
    /// </summary>
    private void DrawAimLine()
    {
        if (aimLine == null) return;

        Vector3 lineStart = transform.position + Vector3.up * 1.5f;
        Vector3 lineEnd = lineStart + transform.forward * lineLength;

        aimLine.positionCount = 2;
        aimLine.SetPosition(0, lineStart);
        aimLine.SetPosition(1, lineEnd);
    }
}
