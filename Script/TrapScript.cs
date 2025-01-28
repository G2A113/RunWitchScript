using UnityEngine;

/// <summary>
/// ���̃N���X�́A�g���b�v���e�ƏՓ˂����ۂ̏������Ǘ����܂��B
/// �Փˎ��ɔ����G�t�F�N�g���\������A�e���X�e�[�W����B���A�g���b�v���g�𖳌������܂��B
/// </summary>
public class TrapScript : MonoBehaviour {
    // �����G�t�F�N�g�̃v���n�u
    [SerializeField] private GameObject _efect;

    // ���ʉ����Ǘ�����SEScript�̎Q��
    private SEScript _objectSE;

    /// <summary>
    /// �X�N���v�g�̏������������s���܂��B
    /// �K�v�ȃR���|�[�l���g��ϐ��̐ݒ���s���܂��B
    /// </summary>
    void Start() {
        // SEScript�̃C���X�^���X���擾
        _objectSE = GameObject.FindWithTag("SE").GetComponent<SEScript>();
    }

    
    /// <summary>
    /// �Փˏ���: �g���b�v���e�ƏՓ˂�����
    /// </summary>
    /// <param name="collision">�Փ˂����I�u�W�F�N�g�̏��B</param>
    private void OnTriggerEnter2D(Collider2D collision) {
        // �Փ˂����I�u�W�F�N�g���e�̏ꍇ
        if (collision.gameObject.CompareTag("Bullet")) {
            // ���������Đ�
            _objectSE.BombSE();

            // �g���b�v�ʒu�ɔ����G�t�F�N�g�𐶐�
            Instantiate(_efect, transform.position, Quaternion.identity);

            // �g���b�v�𖳌���
            gameObject.SetActive(false);

            // �e���X�e�[�W����B��
            collision.gameObject.GetComponent<BulletControl>().HideFromStage();
        }
    }
}

