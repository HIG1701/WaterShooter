using UnityEngine;

/// <summary>
/// �v���C���[�Ɋւ���N���X
/// </summary>

//TODO:���S���̃J�����̏����������ɏ����ȁB����݌v���l������

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerParameter parameter;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float currentSpeed;                        //����Speed
    [SerializeField] private GunManager gunManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera deathCamera;
    private GameManager gameManager;
    private Rigidbody rb;
    private bool isGrounded;
    private int coin;
    private float currentHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
    }


    private void Start()
    {
        currentSpeed = parameter.PlayerSpeed;
        currentHealth = parameter.PlayerHP;


        //���̃R�����g�͋L�q�҂������Ă��ĕ�����Ȃ��Ȃ����̂ŁA�v�Z�����Ƃ��Ďc���Ă܂�
        //�Q�l�����N�Fhttps://qiita.com/kaku0710/items/fdf5bab18b65f6f9dcb4
        //Physics.gravity�F�f�t�H���g�F(0, -9.81, 0)
        //GravityMultiplier = 2f�ɐݒ��F(0, -19.62, 0)
        if (Physics.gravity.y * parameter.GravityMultiplier > -20) Physics.gravity *= parameter.GravityMultiplier;
        deathCamera.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        PlayerJump();                                           //�W�����v����
        PlayerSpeedControl();                                   //�_�b�V������
        PlayerMove();                                           //�v���C���[�̈ړ�
        PlayerShift();                                          //���Ⴊ�ޏ����i���e�������j
        Playerfire();                                           //�v���C���[�̏e���i�o�O�������j
        PlayerReload();                                         //�����[�h
        PlayerAbility();                                        //�A�r���e�B�i���e�������j
        //�}�E�XScroll�ň����I���B�����ł���
    }
    public void DrinkInInventory(GameObject drinkDate)
    {
        parameter.playerInventory.drinkType.Add(drinkDate);
    }

    private void PlayerMove()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;                                             //�����ʏ�̑O�����݂̂𐧌�
        right.y = 0f;                                               //�����ʏ�̉E�����݂̂𐧌�
        forward.Normalize();
        right.Normalize();

        //forward * moveVertical�F�J�����O�x�N�g���ɐ����������|���A�O��̈ړ��������v�Z
        //right * moveHorizontal�F�J�����E�x�N�g���ɐ����������|���A���E�̈ړ��������v�Z
        Vector3 desiredMoveDirection = forward * moveVertical + right * moveHorizontal;

        //Ray���g�p���A�ǔ����΍􂷂�
        //�Q�l�����N�Fhttps://note.com/ryuryu_game/n/ncf259eb5f044
        //Ray�̊J�n�ʒu��Ray�̕�����ݒ�
        Vector3 rayStart = transform.position;
        Vector3 rayDirection = desiredMoveDirection;

        //�ړ�������Ray�����
        RaycastHit hit;
        if (!Physics.Raycast(rayStart, rayDirection, out hit, 0.5f))
        {
            if (desiredMoveDirection != Vector3.zero)
            {
                if (moveVertical != 0)
                {
                    rb.MovePosition(transform.position + desiredMoveDirection * currentSpeed * Time.fixedDeltaTime);
                }
            }
        }
        else
        {
            //TODO:�_�b�V���ǔ���
            //�_�b�V�����ɂ��蔲������������ł��Ă��Ȃ�
            currentSpeed = parameter.DownSpeed;

            //TODO:�ǂ̂ڂ莞������
            //Ray���ǂɃq�b�g���Ă���΁A�ǂ����
            if (hit.collider.CompareTag("Wall"))
            {
                float climbSpeed = 5f;
                if (moveVertical > 0) desiredMoveDirection = Vector3.up * climbSpeed;
            }
        }
        rb.MovePosition(transform.position + desiredMoveDirection * currentSpeed * Time.fixedDeltaTime);
    }

    private void PlayerSpeedControl()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) currentSpeed = parameter.SprintSpeed;
        else currentSpeed = parameter.PlayerSpeed;
    }

    private void PlayerJump()
    {
        //AddForce���Ɖ��̂����܂������Ȃ������̂ŁA�x���V�e�B�ł���Ă܂�
        if (isGrounded && Input.GetKeyDown(KeyCode.Space)) rb.velocity = new Vector3(rb.velocity.x, parameter.JumpVelocity, rb.velocity.z);
    }

    private void PlayerShift()
    {
        //TODO:���Ⴊ��
        //�V�t�g�ł��Ⴊ��
    }

    private void Playerfire()
    {
        if (Input.GetMouseButton(0)) gunManager.Shoot();
        //TODO:�G�C��
        //�E�}�E�X�ŃG�C��
    }

    private void PlayerReload()
    {
        if (Input.GetKeyDown(KeyCode.R)) gunManager.StartReload();
    }

    private void PlayerAbility()
    {
    }

    private void Die()
    {
        gameObject.SetActive(false);
        //���X�|�[���^�C�}�[���J�n������
        gameManager.StartRespawnTimer(gameObject);
        //���C���J�����𖳌������A���S���J������L����
        mainCamera.gameObject.SetActive(false);
        deathCamera.gameObject.SetActive(true);
        deathCamera.transform.position = transform.position + new Vector3(0, 10, 0); //���ɔz�u
        deathCamera.transform.LookAt(transform.position);
    }
    public void Respawn()
    {
        gameObject.SetActive(true);

        //���S���J�����𖳌������A���C���J������L����
        deathCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�v���C���[�R�C���ʒǉ�
        if (collision.gameObject.TryGetComponent<CoinScript>(out var coinManager))
        {
            coin += coinManager.Coin;
            Destroy(collision.gameObject);
        }
        Debug.Log(coin);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;

        if (collision.gameObject.CompareTag("DamageArea"))
        {
            currentHealth -= 9999 * Time.deltaTime;
            if (currentHealth <= 0) Die();
        }

        ////�����G���A��ŁA��ɋʂ��t�����U�����
        //if (collision.gameObject.CompareTag("WaterArea")) gunManager.WaterReload();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }
}
