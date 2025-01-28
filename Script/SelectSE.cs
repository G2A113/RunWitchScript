using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// �I���̑���ɔ�������SE��炷��������N���X
/// </summary>
public class SelectSE : MonoBehaviour {
    // AudioSource�R���|�[�l���g�̎Q��
    private AudioSource _audioSourceSE;

    // �W�����v�A�N�V�����̓��͂�����InputAction
    InputAction _jump;

    // SE (���ʉ�) �p��AudioClip�z��
    [Header("SE")]
    [SerializeField] private AudioClip[] _audioClip = new AudioClip[5];

    // �f���Q�[�g�^se�̒�`�i���SE�̊֐����܂Ƃ߂ČĂяo�����߁j
    public delegate void se();

    // �󒆍U���̌��ʉ����i�[����z��
    public se[] _airAttackSE = new se[3];

    // �W�����v�������ꂽ���̔���t���O
    private bool _jumpBool = false;

    // Y�{�^���̓��̓t���O
    private bool _yBool = false;

    /// <summary>
    /// �X�N���v�g�̏������������s���܂��B
    /// �K�v�ȃR���|�[�l���g��ϐ��̐ݒ���s���܂��B
    /// </summary>
    private void Start() {
        // AudioSource�R���|�[�l���g�̎Q�Ƃ��擾
        _audioSourceSE = GetComponent<AudioSource>();

        // PlayerInput����"Jump"�A�N�V�������擾
        _jump = gameObject.GetComponent<PlayerInput>().actions["Jump"];
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
    /// �Q�[���p�b�h�̑���ɔ�������SE��炷����
    /// </summary>
    private void Update() {
        // �Q�[���p�b�h��L�[�{�[�h�ŏc�̓��͂�����Έړ�SE���Đ�
        if (Input.GetButtonDown("Vertical")) {
            SelectMoveSE();
        }

        // �Q�[���p�b�h�̏c�����͂ɑΉ�
        float yAxis = Input.GetAxis("VerticalPad");

        // �c���̓��͂�����ꍇ�A�ړ�SE���Đ�
        if (yAxis != 0 && !_yBool) {
            _yBool = true;
            SelectMoveSE();
        }

        // �c���̓��͂��Ȃ��ꍇ�A�t���O�����Z�b�g
        if (yAxis == 0 && _yBool) {
            _yBool = false;
        }

        // �W�����v�{�^���������ꂽ���A�W�����vSE���Đ�
        if (_jump.WasPressedThisFrame() && !_jumpBool) {
            _jumpBool = true;
            EnterSE();
        }
    }

    /// <summary>
    /// �W�����v���̌��ʉ����Đ����鏈��
    /// </summary>
    public void EnterSE() {
        _audioSourceSE.PlayOneShot(_audioClip[0]);
    }

    /// <summary>
    /// �ړ����̌��ʉ����Đ����鏈��
    /// </summary>
    public void SelectMoveSE() {
        _audioSourceSE.PlayOneShot(_audioClip[1]);
    }
}
