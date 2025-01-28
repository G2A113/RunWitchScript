using UnityEngine;
/// <summary>
/// �G�l�~�[�@�A�C�̓����A�U���A�V�[���h�Ǘ��A�j�󏈗����s���N���X�B
/// </summary>
public class EyeEnemyMove : MonoBehaviour {
    // ���̓G�Ɋւ���ݒ�
    private ClearScript _clearScript; // �Q�[���N���A��Ԃ��Ǘ�����X�N���v�g
    [SerializeField] private GameObject _nextEnemy; // ���ɐ��������G�̃v���n�u
    [SerializeField] private int _nextEnemyPositionX = 50; // ���̓G�̐����ʒu�iX���j
    [SerializeField] private int _nextEnemyPositionY = 3;  // ���̓G�̐����ʒu�iY���j

    // �G�̓����Ɋւ���ݒ�
    [SerializeField] private GameObject _barrier;  // �������ɕ\������o���A
    [SerializeField] private GameObject _efect;    // �����G�t�F�N�g
    private const float INITIAL_POSITION_Y = 3;          // �����ʒu��Y���W
    [Header("�㉺��")] [SerializeField] private float _range = 1; // �㉺�ɓ����͈�
    private float _uniquTime = 0;                 // �����̃^�C�~���O
    [Header("���x")] [SerializeField] private float _speed = 1; // �㉺�ړ��̑��x

    // �U���Ɋւ���ݒ�
    private float _attackInterval;                // ���݂̍U���C���^�[�o������
    private float _attackIntervalLimit;           // �U���C���^�[�o���̌��E�l
    [SerializeField] private float _attackLimitMin = 3; // �U���Ԋu�̍ŏ��l
    [SerializeField] private float _attackLimitMax = 5; // �U���Ԋu�̍ő�l

    // ���̑��̐ݒ�
    private GameObject _camera;                   // ���C���J����
    [SerializeField] private float _startLength = 5; // �G�������o������
    private Animator _anim;                       // �G�̃A�j���[�^�[
    private bool _start;                          // �G�������n�߂���
    private SEScript _objectSE;                   // ���ʉ��X�N���v�g
    private bool _attack;                         // �U�������ǂ���
    [SerializeField] private GameObject _breakEfect; // �V�[���h����ꂽ�ۂ̃G�t�F�N�g
    [SerializeField] private GameObject _auraObject; // �V�[���h�̃I�[���\��
    [SerializeField] private int _shirldCount = 3;   // �V�[���h�̑ϋv�l

    void Start() {
        // �G��e�I�u�W�F�N�g����؂藣��
        gameObject.transform.parent = null;

        // �U���Ԋu�������_���ŏ�����
        _attackIntervalLimit = Random.Range(_attackLimitMin, _attackLimitMax);

        // �K�v�ȃR���|�[�l���g���擾
        _anim = gameObject.GetComponent<Animator>();
        _camera = GameObject.FindWithTag("MainCamera");
        _objectSE = GameObject.FindWithTag("SE").GetComponent<SEScript>();
        _clearScript = GameObject.FindWithTag("Clear").GetComponent<ClearScript>();
    }

    void Update() {
        if (_start && !_attack) {
            // �㉺�ړ�����
            _uniquTime += Time.deltaTime * _speed;
            _attackInterval += Time.deltaTime;
            transform.position = new Vector2(
                _camera.transform.position.x + _startLength,
                (Mathf.Sin(_uniquTime) * _range) + INITIAL_POSITION_Y);
        } else if (_attack) {
            // �U�������ʒu���ێ�
            transform.position = new Vector2(
                _camera.transform.position.x + _startLength,
                (Mathf.Sin(_uniquTime) * _range) + INITIAL_POSITION_Y);
        } else if (!_start) {
            // ��苗�����ɓ���ƓG�������o��
            if (transform.position.x - _camera.transform.position.x <= _startLength) {
                _start = true;
            }
        }

        // �U���^�C�~���O�̊Ǘ�
        if (_attackInterval >= _attackIntervalLimit && !_attack) {
            _attackIntervalLimit = Random.Range(_attackLimitMin, _attackLimitMax);
            _attack = true;
            _anim.SetBool("EyeAttack", true); // �U���A�j���[�V�������J�n
            _attackInterval = 0;
        }
    }

    /// <summary>
    /// �Փˎ��̏���
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Claw")) {
            HandleClawCollision();
        }
        if (collision.gameObject.CompareTag("Bullet")) {
            HandleBulletCollision(collision);
        }
    }

    /// <summary>
    /// Claw�^�O�Ƃ̏Փˎ��̏���
    /// </summary>
    private void HandleClawCollision() {
        if (_shirldCount == 0) {
            SpawnNextEnemyOrClear();
        } else {
            _objectSE.CounterSE(); // �������̌��ʉ�
            Instantiate(_barrier, transform.position, Quaternion.identity); // �o���A�G�t�F�N�g
        }
    }

    /// <summary>
    /// Bullet�^�O�Ƃ̏Փˎ��̏���
    /// </summary>
    private void HandleBulletCollision(Collider2D collision) {
        if (_shirldCount > 0) {
            _shirldCount--;
            _objectSE.ShieldBreakSE(); // �V�[���h�j��
            Instantiate(_breakEfect, transform.position, Quaternion.identity);
            if (_shirldCount == 0) {
                _auraObject.SetActive(false); // �V�[���h�I�[���𖳌���
            }
        } else {
            _objectSE.CounterSE(); // �V�[���h���Ȃ��ꍇ�̉�
            Instantiate(_barrier, transform.position, Quaternion.identity);
        }
        collision.gameObject.GetComponent<BulletControl>().HideFromStage(); // �e������
    }

    /// <summary>
    /// ���̓G�𐶐����邩�N���A�������s��
    /// </summary>
    private void SpawnNextEnemyOrClear() {
        if (_nextEnemy != null) {
            Instantiate(_nextEnemy, new Vector2(transform.position.x + _nextEnemyPositionX, _nextEnemyPositionY), Quaternion.identity);
        } else {
            _clearScript.ClearEye(); // �N���A�t���O�@�A�C�̏���
        }
        Instantiate(_efect, transform.position, Quaternion.identity); // �����G�t�F�N�g
        _objectSE.BombSE(); // ������
        gameObject.SetActive(false); // ���g���A�N�e�B�u��
    }

    /// <summary>
    /// �U���I�����̏���
    /// </summary>
    private void EyeAttackFalse() {
        _anim.SetBool("EyeAttack", false);
    }

    /// <summary>
    /// �U���������̏�ԃ��Z�b�g
    /// </summary>
    private void EyeAttackEnd() {
        _attack = false;
    }

    /// <summary>
    /// �U�����̃T�E���h�G�t�F�N�g�Đ�
    /// </summary>
    private void EyeAttackSunder() {
        _objectSE.SunderSE();
    }
}
