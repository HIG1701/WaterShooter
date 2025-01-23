using UnityEngine;

public class AimTargetController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float aimDistance = 10f;
    [SerializeField] private float heightOffset = 0f;
    [SerializeField] private bool isBot = false;

    void Update()
    {
        PlayerAimTarget();
        BotAimTarget();
    }

    private void PlayerAimTarget()
    {
        if (!isBot)
        {
            Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * aimDistance;
            targetPosition.y += heightOffset;
            transform.position = targetPosition;
        }
    }

    private void BotAimTarget()
    {
        if (isBot)
        {
        }
    }
}
