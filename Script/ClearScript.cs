using UnityEngine;
/// <summary>
/// �N���A�����̃G�l�~�[�̔j��̋L�^
/// </summary>
public class ClearScript : MonoBehaviour {
    [SerializeField] private GameObject _player;  // �v���C���[�I�u�W�F�N�g
    private bool _clearGhost;        // �S�[�X�g���N���A�����𖞂����Ă��邩�̃t���O
    private bool _clearEye;        // �A�C���N���A�����𖞂����Ă��邩�̃t���O
    private bool _clearNow;      // ���݃N���A��Ԃ��ǂ����̃t���O

    /// <summary>
    /// ���t���[���̍X�V����
    /// �N���A�����𖞂����������`�F�b�N���A�N���A�������Ăяo���܂��B
    /// </summary>
    private void Update() {
        // �S�[�X�g�ƃA�C�̗������N���A���ꂽ�ꍇ�ɃN���A���������s
        if (_clearEye && _clearGhost && !_clearNow) {
            _clearNow = true; // �N���A��������x�������s���邽�߃t���O��ݒ�
            _player.GetComponent<PlayerMove>().ClearChange(); // �v���C���[�ɃN���A������ʒm
        }
    }

    /// <summary>
    /// �S�[�X�g���N���A�����𖞂��������ɌĂ΂�郁�\�b�h
    /// </summary>
    public void ClearGhorst() {
        _clearGhost = true; // �S�[�X�g�̃N���A��Ԃ�^�ɂ���
    }

    /// <summary>
    /// �A�C���N���A�����𖞂��������ɌĂ΂�郁�\�b�h
    /// </summary>
    public void ClearEye() {
        _clearEye = true; // �A�C�̃N���A��Ԃ�^�ɂ���
    }
}
