using UnityEngine;

/// <summary>
/// ���̃N���X�́A�V�[���h���e�ƏՓ˂����ۂ̏������Ǘ����܂��B
/// �Փˎ��ɃV�[���h���j�󂳂�A�G�t�F�N�g���\������A�e���X�e�[�W����B���܂��B
/// </summary>
public class ShieldScript : MonoBehaviour {
    // �V�[���h�j�󎞂ɕ\������G�t�F�N�g�̃v���n�u
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
    /// �Փˏ���: �V�[���h���e�ƏՓ˂�����
    /// </summary>
    /// <param name="collision">�Փ˂����I�u�W�F�N�g�̏��B</param>
    private void OnTriggerEnter2D(Collider2D collision) {
        // �Փ˂����I�u�W�F�N�g���e�̏ꍇ
        if (collision.gameObject.CompareTag("Bullet")) {
            // �V�[���h�j��̌��ʉ����Đ�
            _objectSE.ShieldBreakSE();

            // �V�[���h�̈ʒu�ɃG�t�F�N�g�𐶐�
            Instantiate(_efect, transform.position, Quaternion.identity);

            // �V�[���h���\���ɂ��Ė�����
            gameObject.SetActive(false);

            // �e���X�e�[�W����B��
            collision.gameObject.GetComponent<BulletControl>().HideFromStage();
        }
    }
}

