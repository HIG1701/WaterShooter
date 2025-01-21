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


    public void ISMove(float moveHorizontal, float moveVertical)
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0f;                                             //水平面上の前方向のみを制御
        right.y = 0f;                                               //水平面上の右方向のみを制御
        forward.Normalize();
        right.Normalize();

        //forward * moveVertical：カメラ前ベクトルに垂直方向を掛け、前後の移動方向を計算
        //right * moveHorizontal：カメラ右ベクトルに水平方向を掛け、左右の移動方向を計算
        Vector3 desiredMoveDirection = forward * moveVertical + right * moveHorizontal;

        Vector3 move = desiredMoveDirection * currentSpeed;
        move.y = rb.velocity.y;
        rb.velocity = move;
        Debug.Log("abc");
    }
}
