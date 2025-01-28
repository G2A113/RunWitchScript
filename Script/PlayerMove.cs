using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[�̓����A�W�����v�A�U���A�X�R�A�Ǘ��A�V�[���J�ڂ𐧌䂷��N���X�B
/// </summary>
public class PlayerMove : MonoBehaviour {
    #region �ϐ�
    [Header("�V�[���J�ڐݒ�")]
    [SerializeField] private string _clearScene; // �N���A���̃V�[����
    [SerializeField] private string _gameOverScene = default;  // �Q�[���I�[�o�[���̃V�[����

    [Header("�I�u�W�F�N�g�v�[���ƃG�t�F�N�g")]
    [SerializeField] private ObjectPoolController _objectPool = default; // �e�ۂ̃I�u�W�F�N�g�v�[��
    [SerializeField] private GameObject _sceneChangeManager = null;   // �V�[���ύX���Ǘ�����I�u�W�F�N�g
    [SerializeField] private GameObject _clawSlashEffect = null;      // ����U���̃G�t�F�N�g
    

    [Header("�v���C���[�̔\�͐ݒ�")]
    [SerializeField] private float _defaltJumpPower = 700f; // �W�����v�̗�
    [SerializeField] private  float _specialJumpPowerChange = 0.85f; // ����U���W�����v�̗�
    private const float TIMER_INCREASE_RATE = 1f; // ���b�^�C�������������

    private const float DASH_SPEED_MIN = 1f;// �_�b�V�����x�̍ŏ��l
    private const float DASH_SPEED_MAX = 10f;// �_�b�V�����x�̍ő�lisusing UnityEngine;
    [SerializeField]
    private float _primaryDashSpeed = 6;
    [Header("UI�ݒ�"), SerializeField]
    private TextMeshProUGUI _timerText = default; // �^�C���\���p��UI�e�L�X�g
    private float _keepTimer = default;                // ����̃^�C��

    // �R���|�[�l���g���ԊǗ��p�ϐ�
    private Rigidbody2D _playerRigidbody = default;  // �v���C���[��Rigidbody2D
    private SEScript _objectSE = null;//SE
    private Animator _playerAnimator = default;      // �v���C���[�̃A�j���[�V�����R���g���[���[
    private bool _animeGround = true;

    // ��ԊǗ��ϐ�
    private float _currentTimer = 0f;                // ���݂̃^�C��
    private float _currentDashSpeed = 0f;            // ���݂̃_�b�V�����x
    private int _currentJumpCount = 0;               // ���݂̃W�����v��
    private const int JUMP_LIMIT = 3;                // �ő�W�����v��

    // ���̓A�N�V����
    private InputAction _moveAction = default;       // �ړ��A�N�V����
    private InputAction _jumpAction = default;       // �W�����v�A�N�V����
    private InputAction _bulletAction = default;     // �e�ۃA�N�V����
    private InputAction _swingAction = default;      // ����U���A�N�V����

    // �v���C���[�̌��݂̏��
    private PlayerState _currentState = PlayerState.Idle; // �v���C���[�̏�Ԃ��Ǘ�����

    
    
   
    #endregion

    #region �v���p�e�B
    public int JumpCount {
        set {
            _currentJumpCount = value;
        }
        get {
            return _currentJumpCount;
        }
    }
    public float DashSpeedNow {
        set {
            _currentDashSpeed = value;
        }
        get {
            return _currentDashSpeed;
        }
    }// ���Ɉړ����鑬�x
    #endregion
    #region ���\�b�h
    /// <summary>
    /// �X�N���v�g�̏������������s���܂��B
    /// �K�v�ȃR���|�[�l���g��ϐ��̐ݒ���s���܂��B
    /// </summary>
    private void Start() {
        InitializeInputActions(); // ���̓A�N�V�����̏�����
        InitializeComponents();  // �R���|�[�l���g�̏�����
        UpdateTimerUI();         // �X�R�AUI�̏�����
    }

    /// <summary>
    /// ���̓A�N�V���������������܂��B
    /// PlayerInput�R���|�[�l���g����A�N�V�������擾���܂��B
    /// </summary>
    private void InitializeInputActions() {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        _moveAction = playerInput.actions["Move"];
        _jumpAction = playerInput.actions["Jump"];
        _bulletAction = playerInput.actions["Bullet"];
        _swingAction = playerInput.actions["Swing"];
    }

    /// <summary>
    /// Rigidbody��Animator�Ȃǂ̃R���|�[�l���g���擾���ď��������܂��B
    /// </summary>
    private void InitializeComponents() {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        //SEScript�̎擾
        _objectSE = GameObject.FindWithTag("SE").GetComponent<SEScript>();
        _currentDashSpeed = _primaryDashSpeed;
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
    /// �Q�[�����W�b�N����͏����A�X�R�A�X�V���������܂��B
    /// </summary>
    private void Update() {
        HandleTimer();    // �X�R�A�̊Ǘ�
        HandleMovement(); // �v���C���[�̈ړ�����
        HandleActions();  // �v���C���[�̃A�N�V��������
    }

    /// <summary>
    /// ���t���[���̃X�R�A���Z������UI�̍X�V���s���܂��B
    /// </summary>
    private void HandleTimer() {
        _currentTimer += Time.deltaTime; // ���Ԃɉ����ăX�R�A�����Z
        if (_currentTimer >= 1 && _currentState != PlayerState.Dead) {
            _currentTimer -= 1;
            _keepTimer += 1;
        }
        UpdateTimerUI();                 // �X�R�AUI���X�V
    }

    /// <summary>
    /// �X�R�AUI���X�V���܂��B
    /// </summary>
    private void UpdateTimerUI() {
        _timerText.text = $"{Mathf.FloorToInt(_keepTimer):000000}"; // �X�R�A���t�H�[�}�b�g���ĕ\��
    }

    /// <summary>
    /// �v���C���[�̈ړ��ƃW�����v�̓��͂��������܂��B
    /// </summary>
    private void HandleMovement() {
        // ���͒l���擾
        Vector2 inputMoveAxis = _moveAction.ReadValue<Vector2>();
        //���n����ƃW�����v�񐔂̐���
        if (_currentJumpCount == 0 && _animeGround == false) {
            _animeGround = true;
            _playerAnimator.SetBool("isGround", _animeGround);
        } else if (_currentJumpCount > 0 && _animeGround == true) {
            _animeGround = false;
            _playerAnimator.SetBool("isGround", _animeGround);
        }

        // �_�b�V�����x�͈̔͂𐧌�
        _currentDashSpeed = Mathf.Clamp(_currentDashSpeed, DASH_SPEED_MIN, DASH_SPEED_MAX);
        
        // �W�����v����
        if (_currentState == PlayerState.Idle && _jumpAction.WasPressedThisFrame() && _currentJumpCount < JUMP_LIMIT) {
            Jump();
        }
    }

    /// <summary>
    /// �v���C���[�̃W�����v���������s���܂��B
    /// </summary>
    private void Jump() {
        _currentJumpCount++; // �W�����v�񐔂����Z
        _animeGround = false;
        _objectSE.JumpSE();
        _playerAnimator.SetBool("isGround", _animeGround);
        _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, 0);
        _playerRigidbody.AddForce(transform.up * _defaltJumpPower);// ������ɗ͂�������
    }

    /// <summary>
    /// �v���C���[�̍U�������A�N�V�����̓��͂��������܂��B
    /// </summary>
    private void HandleActions() {
        if (_currentState == PlayerState.Idle && _bulletAction.WasPressedThisFrame()) {
            StartAttack(); // �ʏ�U�����J�n
        } else if (_currentState == PlayerState.Idle && _swingAction.WasPressedThisFrame()) {
            StartSpecialAttack(); // ����U�����J�n
        }
    }

    /// <summary>
    /// �ʏ�U�����J�n���܂��B
    /// </summary>
    private void StartAttack() {
        _currentState = PlayerState.Attacking; // ��Ԃ�Attacking�ɐݒ�
        _playerAnimator.SetBool("attack", true); // �A�j���[�V�������g���K�[
        
    }

    /// <summary>
    /// ����U�����J�n���܂��B
    /// </summary>
    private void StartSpecialAttack() {
        _currentState = PlayerState.SpecialAttacking; // ��Ԃ�SpecialAttacking�ɐݒ�
        _playerAnimator.SetBool("attackAnother", true); // �A�j���[�V�������g���K�[
        _playerRigidbody.AddForce(Vector2.up * _defaltJumpPower * _specialJumpPowerChange); // ����W�����v�����s
    }

    /// <summary>
    /// �Œ�t���[�����Ƃɕ������Z�֘A�̍X�V�������s���܂��B
    /// �v���C���[�����S��ԂłȂ��ꍇ�A���ړ��̑��x��ݒ肵�܂��B
    /// </summary>
    private void FixedUpdate() {
        if (_currentState != PlayerState.Dead) {
            // Rigidbody2D �ɉ��ړ��̑��x��ݒ�i���݂̃_�b�V�����x, ���݂̏c�������x�j
            _playerRigidbody.velocity = new Vector2(_currentDashSpeed, _playerRigidbody.velocity.y);
        }
    }


    /// <summary>
    /// �v���C���[�����̃I�u�W�F�N�g�ƏՓ˂����ۂ̏����B
    /// </summary>
    /// <param name="collision">�Փ˂����I�u�W�F�N�g�̏��B</param>
    private void OnTriggerEnter2D(Collider2D collision) {
        //�G�̍U������є͈͊O�ւ̈ړ��ɂ�鎀�S
        if ((collision.gameObject.CompareTag("Dead") || collision.gameObject.CompareTag("Sunder")) &&
            _currentState != PlayerState.Dead) {
            _currentState = PlayerState.Dead;//���S��ԂɕύX
            transform.Find("BGM").gameObject.SetActive(false);//BGM�̒�~
            _objectSE.ImpactSE();
            _objectSE.DeadSE(); 
            _playerAnimator.SetBool("dead", true);//�A�j���[�^�[�Ŏ��S��ԂɕύX
            GameObject.FindWithTag("Carten").GetComponent<FadeManager>()._keepGameTimer = (int)_keepTimer;//�v���C�^�C���̕ۑ�
            _sceneChangeManager.GetComponent<SceneChange>().ChangeScene(_gameOverScene);//�V�[���J�ځ@�Q�[���I�[�o�[
        }
        //�ł�㩂ɂ�鎀�S
        if (collision.gameObject.CompareTag("Poison") && _currentState != PlayerState.Dead) {
            _currentState = PlayerState.Dead;//���S��ԂɕύX
            transform.Find("BGM").gameObject.SetActive(false);//BGM�̒�~
            _objectSE.PoisonSE();          
            _playerAnimator.SetBool("dead", true);//�A�j���[�^�[�Ŏ��S��ԂɕύX
            GameObject.FindWithTag("Carten").GetComponent<FadeManager>()._keepGameTimer = (int)_keepTimer;//�v���C�^�C���̕ۑ�
            _sceneChangeManager.GetComponent<SceneChange>().ChangeScene(_gameOverScene);//�V�[���J�ځ@�Q�[���I�[�o�[
        }
    }
    /// <summary>
    /// �Q�[�����N���A�����ۂ̏����B
    /// </summary>
    public void ClearChange() {
        GameObject.FindWithTag("Carten").GetComponent<FadeManager>()._keepGameTimer = (int)_keepTimer;
        _sceneChangeManager.GetComponent<SceneChange>().ChangeScene(_clearScene);
    }
    /// <summary>
    /// �e�ۍU���A�j���[�^�[�t���O�p
    /// </summary>
    private void AnimeAttackFalse() {

        _playerAnimator.SetBool("attack", false);

    }
    /// <summary>
    /// �e�ۍU������
    /// </summary>
    private void AttackBullet() {
        _currentState = PlayerState.Idle;
        _objectSE.BulletSE();
        //�I�u�W�F�N�g�v�[����Launch�֐��Ăяo��
        _objectPool.Launch(transform.position + new Vector3(1, 0, 0));
    }
    /// <summary>
    /// �ߐڍU���U���A�j���[�^�[�t���O
    /// </summary>
    private void AnimeAttackAnotherFalse() {

        _playerAnimator.SetBool("attackAnother", false);
    }
    /// <summary>
    /// �ߐڍU���U���o��
    /// </summary>
    private void ClawAttack() {
        _objectSE.SwingSE();
        _currentState = PlayerState.Idle;
        _clawSlashEffect.SetActive(true);
    }
    #endregion
}

/// <summary>
/// �v���C���[�̏�Ԃ�\���񋓌^�B
/// </summary>
public enum PlayerState {
    Idle,             // �ҋ@��
    Attacking,        // �ʏ�U����
    SpecialAttacking, // ����U����
    Dead              // ���S���
}
