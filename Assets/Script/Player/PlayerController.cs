using UnityEngine;

/// <summary>
/// �v���C���[�Ɋւ���N���X
/// </summary>
//TODO:���S���̃J�����̏����������ɏ����ȁB����݌v���l������
//TODO:Animation����
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
    private Animator animator;
    private bool isGrounded;
    private bool isClimbing;
    private int coin;
    private float currentHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        currentSpeed = parameter.PlayerSpeed;
        currentHealth = parameter.PlayerHP;

        ////���̃R�����g�͋L�q�҂������Ă��ĕ�����Ȃ��Ȃ����̂ŁA�v�Z�����Ƃ��Ďc���Ă܂�
        ////�Q�l�����N�Fhttps://qiita.com/kaku0710/items/fdf5bab18b65f6f9dcb4
        deathCamera.gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        PlayerSpeedControl();                                   //�_�b�V������
        PlayerMove();                                           //�v���C���[�̈ړ�
        //PlayerClimbing();                                       //�Ǔo�菈���i���e�������j
        PlayerShift();                                          //���Ⴊ�ޏ����i���e�������j
        Playerfire();                                           //�v���C���[�̏e���i�o�O�������j
        PlayerReload();                                         //�����[�h
        PlayerAbility();                                        //�A�r���e�B�i���e�������j
        //�}�E�XScroll�ň����I���B�����ł���
        Debug.Log("�ڒn" + isGrounded);
        Debug.Log("�o��" + isClimbing);
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

        if (isGrounded)
        {
            Vector3 move = desiredMoveDirection * currentSpeed;
            move.y = rb.velocity.y;
            rb.velocity = move;
            if (animator != null) animator.SetFloat("Walk", desiredMoveDirection.magnitude);
        }
    }

    private void PlayerClimbing()
    {
        float CheckOffset = 1.0f;       //�ǔ���̃I�t�Z�b�g
        float upperCheckOffset = 12.5f; //�㕔�ǔ���̃I�t�Z�b�g
        float wallCheckDistance = 5.0f; //�ǔ���̋���
        Ray wallCheckRay = new Ray(transform.position + Vector3.up * CheckOffset, transform.forward);
        Ray upperCheckRay = new Ray(transform.position + Vector3.up * upperCheckOffset, transform.up);

        bool isForwardWall = Physics.Raycast(wallCheckRay, wallCheckDistance);
        bool isUpperWall = Physics.Raycast(upperCheckRay, wallCheckDistance);
        Debug.DrawRay(wallCheckRay.origin, wallCheckRay.direction * wallCheckDistance, Color.red);
        Debug.DrawRay(upperCheckRay.origin, upperCheckRay.direction * wallCheckDistance, Color.blue);
        //�ǂ����邩�ǂ��������O�ɏo��
        Debug.Log("Forward Wall: " + isForwardWall);
        Debug.Log("Upper Wall: " + isUpperWall);

        if (isForwardWall && !isUpperWall) isClimbing = true;
        else isClimbing = false;
        if (isClimbing)
        {
            //���͂��擾
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            //�ړ��x�N�g�����v�Z
            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);

            //Rigidbody��velocity��ݒ�
            rb.velocity = movement * currentSpeed;
        }
    }
    private void PlayerSpeedControl()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) currentSpeed = parameter.SprintSpeed;
        else currentSpeed = parameter.PlayerSpeed;
    }
    private void PlayerShift()
    {
        //TODO:�V�t�g�ł��Ⴊ��
    }
    private void Playerfire()
    {
        if (Input.GetMouseButton(0)) gunManager.Shoot();
        //TODO:�E�}�E�X�ŃG�C��
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
        //Debug.Log(coin);
    }
    private void OnCollisionStay(Collision collision)
    {
        //TODO:�Q�[���J�n���A�ڒnFlag�����bfalse�ɂȂ�o�O����������
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
        if (collision.gameObject.CompareTag("Wall")) isClimbing = false;
    }
}