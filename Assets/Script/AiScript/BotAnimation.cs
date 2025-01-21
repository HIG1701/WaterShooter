using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
public class BotAnimation : MonoBehaviour
{
    private Animator animator;
    private Vector3 previousPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public enum playerAnimationNumbers
    {
        Idle,
        FlontRun,
        BackRun,
        LeftRun,
        RightRun,
        Jump,
        Squat,
        FaceDown,
    }

    private void Update()
    {
        CharaAnimation();
    }

    public void CharaAnimation()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Debug.Log(moveHorizontal);
        Debug.Log(moveVertical);

        //forwardを1,backを-1;右を1,左を-1.
        //Vector3 moveDirection = (transform.position - previousPosition).normalized;

        //Vector3 charaForward = transform.forward;
        //Vector3 charaRight = transform.right;

        //float forwardDot = Vector3.Dot(charaForward, moveDirection);
        //float rightDot = Vector3.Dot(charaRight, moveDirection);

        //previousPosition = transform.position;

        //if (forwardDot > 0.1f)
        //{
        //    Debug.Log("前進中" + forwardDot);
        //}
        //else if (forwardDot < -0.1f)
        //{
        //    Debug.Log("後退中" + forwardDot);
        //}

        //if (rightDot > 0.1f)
        //{
        //    Debug.Log("右へ移動中" + rightDot);
        //}
        //else if (rightDot < -0.1f)
        //{
        //    Debug.Log("左へ移動中" + rightDot);
        //}
    }

    //public void KeyRef(KeyCode kesySend)
    //{
    //    WalkingAnimation();
    //}

    //private void MoveCheck()
    //{
    //    Vector3 vector = transform.position.Normalize();
    //}

    //    public void StopAnimation()
    //    {
    //        if (chekKey == (int)playerAnimationNumbers.StopUp)
    //        {
    //            animator.SetInteger("Animeset", (int)playerAnimationNumbers.StopUp);
    //        }
    //        else if (chekKey == (int)playerAnimationNumbers.StopDown)
    //        {
    //            animator.SetInteger("Animeset", (int)playerAnimationNumbers.StopDown);
    //        }
    //        else if (chekKey == (int)playerAnimationNumbers.StopLeft)
    //        {
    //            animator.SetInteger("Animeset", (int)playerAnimationNumbers.StopLeft);
    //        }
    //        else if (chekKey == (int)playerAnimationNumbers.StopRight)
    //        {
    //            animator.SetInteger("Animeset", (int)playerAnimationNumbers.StopRight);
    //        }
    //    }

    //    private void WalkingAnimation()
    //    {
    //        switch (playerKeyCode)
    //        {
    //            case KeyCode.UpArrow:
    //            case KeyCode.W:
    //                animator.SetInteger("Animeset", (int)playerAnimationNumbers.Up);
    //                break;
    //            case KeyCode.DownArrow:
    //            case KeyCode.S:
    //                animator.SetInteger("Animeset", (int)playerAnimationNumbers.Down);
    //                break;
    //            case KeyCode.RightArrow:
    //            case KeyCode.D:
    //                animator.SetInteger("Animeset", (int)playerAnimationNumbers.Right);
    //                break;
    //            case KeyCode.LeftArrow:
    //            case KeyCode.A:
    //                animator.SetInteger("Animeset", (int)playerAnimationNumbers.Left);
    //                break;
    //            default:
    //                break;
    //        }
    //    }
}