using UnityEngine;

/// <summary>
/// �G�l�~�[�@�S�[�X�g�̓����A�U���A�V�[���h�������Ǘ�����N���X�B
/// </summary>
public class GhostMove : MonoBehaviour {
    // �O���X�N���v�g�ƃI�u�W�F�N�g�̎Q��
    private ClearScript _clear; // �Q�[���N���A���Ǘ�����X�N���v�g
    private SEScript _objectSE; // ���ʉ����Ǘ�����X�N���v�g
    [SerializeField] private GameObject _barrier; // �������̃o���A�G�t�F�N�g
    [SerializeField] private GameObject _efect; // �����G�t�F�N�g
    [SerializeField] private GameObject _eyeEnemy; // ���ɐ��������ڂ̓G
    [SerializeField] private GameObject _nextEnemy; // ���ɐ��������S�[�X�g�̓G

    // �ړ��ƍU���̊Ǘ�
    private bool _start; // �G�������n�߂����ǂ���
    [SerializeField] private float _startLength = 5; // �G�������n�߂鋗��
    private float _uniquTime = 0; // �c�����̈ړ��p�^�C�}�[
    private Vector3 _initialPosition; // �����ʒu

    [Header("�c�����̐ݒ�")]
    [SerializeField] private float _speedVertical = 1; // �c�����̑��x
    [SerializeField] private float _rangeVertical = 1; // �c�����̈ړ��͈�

    [Header("�������̐ݒ�")]
    [SerializeField] private float _speedHorizontal = 1; // �������̑��x
    [SerializeField] private float _rangeHorizontal = 9; // �������̈ړ��͈�

    private GameObject _camera; // ���C���J�����̎Q��

    private bool _attack; // �U�������ǂ���
    private float _attackInterval; // �U���C���^�[�o��
    private float _attackIntervalLimit = 3; // �U���C���^�[�o���̌��E�l

    // �A�j���[�V�����Ǘ�
    private Animator _anim; // �A�j���[�^�[�R���|�[�l���g
    private string[] _animeBool = { "Attack", "Attack1", "Attack2", "Attack3" }; // �U���A�j���[�V������
    private int _randomStoc; // �����_���őI�����ꂽ�A�j���[�V����
    private float _stopTime; // �������̈ړ�����
    private bool _goAhead = true; // �������̈ړ������itrue:�O�i, false:��ށj

    // �V�[���h�Ǘ�
    [SerializeField] private GameObject _breakEfect; // �V�[���h�j�󎞂̃G�t�F�N�g
    [SerializeField] private GameObject _auraObject; // �V�[���h�I�[��
    [SerializeField] private int _shirldCount = 3; // �V�[���h�ϋv�l

    /// <summary>
    /// �X�N���v�g�̏������������s���܂��B
    /// �K�v�ȃR���|�[�l���g�̐ݒ���s���܂��B
    /// </summary>
    void Start() {
        // �K�v�ȃR���|�[�l���g�̎擾
        _camera = GameObject.FindWithTag("MainCamera");
        _anim = gameObject.GetComponent<Animator>();
        _objectSE = GameObject.FindWithTag("SE").GetComponent<SEScript>();
        _clear = GameObject.FindWithTag("Clear").GetComponent<ClearScript>();
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
    /// �ړ��C�U������
    /// </summary>
    void Update() {
        if (_start && !_attack) {
            // �c�����̈ړ�����
            _uniquTime += Time.deltaTime * _speedVertical;
            _attackInterval += Time.deltaTime;
            transform.position = new Vector3(
                _camera.transform.position.x + _startLength,
                (Mathf.Sin(_uniquTime) * _rangeVertical) + _initialPosition.y,
                _initialPosition.z);
        } else if (_attack) {
            // �U�����̉������ړ�����
            if (_goAhead) {
                _stopTime += Time.deltaTime * _speedHorizontal;
            } else {
                _stopTime -= Time.deltaTime * _speedHorizontal;
            }
            transform.position = new Vector3(
                -(Mathf.Sin(_stopTime) * _rangeHorizontal) + _camera.transform.position.x + _startLength,
                (Mathf.Sin(_uniquTime) * _rangeVertical) + _initialPosition.y,
                _initialPosition.z);
            if (_stopTime < 0) {
                _attack = false;
                _stopTime = 0;
            }
        } else if (!_start) {
            // ��苗�����ɓ������ꍇ�A�����n�߂�
            if (transform.position.x - _camera.transform.position.x <= _startLength) {
                _start = true;
                _initialPosition = transform.position;
            }
        }

        // �U���C���^�[�o�������E�ɒB�����ꍇ�A�U�����J�n
        if (_attackInterval >= _attackIntervalLimit - 1 && !_attack && !transform.Find("area").gameObject.activeSelf) {
            transform.Find("area").gameObject.SetActive(true);
        }
        if (_attackInterval >= _attackIntervalLimit && !_attack) {
            transform.Find("area").gameObject.SetActive(false);
            _goAhead = true;
            _attackIntervalLimit = Random.Range(6, 10);
            _attack = true;
            _randomStoc = Random.Range(1, 3);
            _anim.SetBool(_animeBool[_randomStoc], true);
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
            HandleEnemyDefeat();
        } else {
            _objectSE.CounterSE();
            Instantiate(_barrier, transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Bullet�^�O�Ƃ̏Փˎ��̏���
    /// </summary>
    private void HandleBulletCollision(Collider2D collision) {
        if (_shirldCount > 0) {
            _shirldCount--;
            _objectSE.ShieldBreakSE();
            Instantiate(_breakEfect, transform.position, Quaternion.identity);
            if (_shirldCount == 0) {
                _auraObject.SetActive(false);
            }
        }
        collision.gameObject.GetComponent<BulletControl>().HideFromStage();
    }

    /// <summary>
    /// �G�̌��j����
    /// </summary>
    private void HandleEnemyDefeat() {
        _objectSE.BombSE();
        Instantiate(_efect, transform.position, Quaternion.identity);

        // ���ɃS�[�X�g���ڂ̓G�𐶐�
        if (_nextEnemy != null) {
            Instantiate(_nextEnemy, new Vector2(transform.position.x + 50, 3), Quaternion.identity);
        } else {
            _clear.ClearGhorst();
        }
        if (_eyeEnemy != null) {
            Instantiate(_eyeEnemy, new Vector2(transform.position.x + 50, 3), Quaternion.identity);
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �U���I�����̏���
    /// </summary>
    private void AttackFalse() {
        _goAhead = true;
        _anim.SetBool(_animeBool[_randomStoc], false);
    }

    /// <summary>
    /// �U�����̃T�E���h�Đ�
    /// </summary>
    private void AttackSound() {
        _objectSE.GhostSE();
    }
}
