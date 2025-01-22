using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private PlayerParameter parameter;
    [SerializeField] private float currentSpeed;
    private Rigidbody rb;
    private BotAnimation animationControl;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animationControl = GetComponent<BotAnimation>();
    }

    public void ISMove(float moveHorizontal, float moveVertical)
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * moveVertical + right * moveHorizontal;

        Vector3 move = desiredMoveDirection * currentSpeed;
        move.y = rb.velocity.y;
        rb.velocity = move;
        if (animationControl != null )
        {
            animationControl.CharaAnimation(moveHorizontal, moveVertical);
        }
    }
}
