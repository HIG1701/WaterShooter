using System.Collections;
using UnityEngine;

public class BotShootingMove : MonoBehaviour
{
    private bool isShooting;
    private CharacterMove characterMove;
    private float randomSecond = 0;
    private int moveDirection = 0;
    private float previosDirection = 0;

    private enum moveDirectionEnum { Left, Right };

    void Start()
    {
        characterMove = GetComponent<CharacterMove>();
    }

    public void BotShootingMoveReset()
    {
        isShooting = false;
        randomSecond = 0;
        moveDirection = 0;
        previosDirection = 0;
    }

    public void BotShootingMoveStart()
    {
        StartCoroutine(Shooting());
    }

    private IEnumerator Shooting()
    {
        if (isShooting) { yield break; }
        isShooting = true;
        randomSecond = Random.Range(0, 1f);
        if (moveDirection != (int)moveDirectionEnum.Left) { moveDirection = (int)moveDirectionEnum.Left; }
        else { moveDirection = (int)moveDirectionEnum.Right; }

        int moveValue = 0;
        switch (moveDirection)
        {
            case (int)moveDirectionEnum.Left:
                moveValue = 1;
                break;
            case (int)moveDirectionEnum.Right:
                moveValue = -1;
                break;
        }

        while (!(previosDirection > 1f) && !(previosDirection < -1f))
        {
            if (moveDirection == (int)moveDirectionEnum.Left) { previosDirection += 0.05f; }
            else if (moveDirection == (int)moveDirectionEnum.Right) { previosDirection -= 0.05f; }
            yield return null;
        }

        yield return new WaitForSeconds(randomSecond);
        isShooting = false;
        previosDirection = moveValue;
    }

    private void MoveShoot()
    {
        if (!isShooting) { return; }
        characterMove.ISMove(previosDirection, 0);
    }

    private void FixedUpdate()
    {
        MoveShoot();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
