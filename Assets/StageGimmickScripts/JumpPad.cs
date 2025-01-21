using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("ジャンプ高さ"), SerializeField] private float jumpHeightPower = 50f;
    [Header("ジャンプ前進"), SerializeField] private float jumpForcePower = 100f;
    [Header("目標到着時間"), SerializeField] private float jumpTime = 3f;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(JumpPadControl(other.transform));
    }

    private System.Collections.IEnumerator JumpPadControl(Transform playerTransform)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = playerTransform.position;
        Vector3 targetPosition = startPosition + (playerTransform.forward * jumpForcePower);

        while (elapsedTime < jumpTime)
        {
            elapsedTime += Time.deltaTime;
            Collider[] playerCollision = Physics.OverlapSphere(playerTransform.position, 0.1f);
            foreach (Collider collider in playerCollision)
            {
                Debug.LogWarning(collider.gameObject);
                if (collider.gameObject != this.gameObject && collider.gameObject != playerTransform.gameObject) { yield break; }
            }

            float t = elapsedTime / jumpTime;

            // 放物線の軌道: y = 4h(-t^2 + t)
            float height = 4 * jumpHeightPower * (-Mathf.Pow(t, 2) + t);
            playerTransform.position = Vector3.Lerp(startPosition, targetPosition, t) + Vector3.up * height;
            yield return null;
        }

        // 最終位置に調整
        playerTransform.position = targetPosition;
    }
}
