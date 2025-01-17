using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BotAnimation : MonoBehaviour
{
    private Animator animator;

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