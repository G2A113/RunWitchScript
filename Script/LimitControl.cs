using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// �v���C���[�̈ړ��ɉ����ă_�b�V�����x�̐�����s���N���X�ł��B
/// </summary>
public class LimitControl : MonoBehaviour {
    // �v���C���[�I�u�W�F�N�g
    [SerializeField] private GameObject _player;
    private PlayerMove _playerMove; // PlayerMove �X�N���v�g�̎Q��
    private InputAction _move; // �v���C���[�̈ړ����͂��擾���邽�߂̃A�N�V����

    /// <summary>
    /// �X�N���v�g�̏������������s���܂��B
    /// �K�v�ȃR���|�[�l���g�̐ݒ���s���܂��B
    /// </summary>
    void Start() {
        // PlayerInput �R���|�[�l���g����uMove�v�A�N�V�������擾
        _move = gameObject.GetComponent<PlayerInput>().actions["Move"];

        // �v���C���[�� PlayerMove �X�N���v�g���擾
        _playerMove = _player.GetComponent<PlayerMove>();
    }
    /// <summary>
    /// ���t���[���̍X�V����
    /// �E�����Ɉړ�����ƃ_�b�V�����x�𑝉������A�������Ɉړ�����ۂɂ̓_�b�V�����x�����������鐧��
    /// </summary>
    void Update() {
        // ���͂��ꂽ�ړ��̃x�N�g�����擾�ix���̂ݎg�p�j
        Vector2 inputMoveAxis = _move.ReadValue<Vector2>();

        // x�����������i�E�j�ɓ������Ƃ��A�_�b�V�����x�𑝉�
        if (inputMoveAxis.x > 0) {
            _playerMove.DashSpeedNow += 0.1f; // �E�����Ɉړ����A�_�b�V�����x�𑝉�
        }

        // x�����������i���j�ɓ������Ƃ��A���_�b�V�����x��1���傫���ꍇ�Ƀ_�b�V�����x������
        if ((inputMoveAxis.x < 0) && _playerMove.DashSpeedNow > 1) {
            _playerMove.DashSpeedNow -= 1.0f; // �������Ɉړ����A��ސ����Ƃ��ă_�b�V�����x������
        }
    }
}
