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

    public void CharaAnimation(float moveHorizontal, float moveVertical)
    {
        animator.SetFloat("forward", (float)moveVertical);
        animator.SetFloat("right", (float)moveHorizontal);
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

    public void JumpAnimation()
    {
        animator.SetInteger("Animeset", (int)2);
    }

    public void ShiftAnimation()
    {
        animator.SetInteger("Animeset", (int)1);
    }

    public void AnimationReset()
    {
        animator.SetInteger("Animeset", (int)0);
    }

    [SerializeField] public Transform aimTarget;

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator && aimTarget)
        {
            // LookAtの適用度（0.0f 〜 1.0f）
            animator.SetLookAtWeight(1.0f, 0.5f, 0.7f, 1.0f, 0.5f);

            // 照準ターゲットの位置に視線を向ける
            animator.SetLookAtPosition(aimTarget.position);

            // 右手IK（銃をターゲットに向ける）
            //animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.1f);
            //animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

            // ターゲットの位置・回転に合わせる
            animator.SetIKPosition(AvatarIKGoal.RightHand, aimTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, aimTarget.rotation);

        }
    }
}