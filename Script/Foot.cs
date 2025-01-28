using UnityEngine;
/// <summary>
/// ���n����
/// </summary>
public class Foot : MonoBehaviour {
    [SerializeField] private GameObject _player;         // �v���C���[�I�u�W�F�N�g�ւ̎Q��
    private PlayerMove _playerMove;                       // �v���C���[�̏������Ǘ�����X�N���v�g�ւ̎Q��

    /// <summary>
    /// �X�N���v�g�̏������������s���܂��B
    /// </summary>
    void Start() {
        // PlayerMove�X�N���v�g�̃C���X�^���X���擾
        _playerMove = _player.GetComponent<PlayerMove>();
    }

    /// <summary>
    /// �v���C���[�̑����n�ʂɐG��Ă���ԂɌĂ΂��
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision) {
        // �Փ˂����I�u�W�F�N�g��"Ground"�^�O�������Ă���ꍇ
        if (collision.gameObject.CompareTag("Ground")) {
            // �v���C���[���n�ʂɐڐG���Ă���ꍇ�A�W�����v�J�E���g�����Z�b�g
            _playerMove.JumpCount = 0;
        }
    }
}
