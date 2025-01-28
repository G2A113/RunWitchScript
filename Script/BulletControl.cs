using UnityEngine;
/// <summary>
/// �I�u�W�F�N�g�v�[���̑���
/// </summary>
public class BulletControl : MonoBehaviour {
    [SerializeField]
    private float _speed = 5; // �e�̈ړ����x��ݒ�i�I�u�W�F�N�g�v�[���p�R���g���[���[�i�[�p�ϐ��j

    private ObjectPoolController _objectPool; // �I�u�W�F�N�g�v�[���̎Q�Ƃ��i�[����ϐ�

    /// <summary>
    /// �e�̏���������
    /// �I�u�W�F�N�g�v�[����e�I�u�W�F�N�g����擾
    /// </summary>
    void Start() {
        // �e�I�u�W�F�N�g����I�u�W�F�N�g�v�[�����擾
        _objectPool = transform.parent.GetComponent<ObjectPoolController>();
    }

    /// <summary>
    /// ���t���[���̍X�V����
    /// �e���E�����Ɉړ����鏈�������s
    /// </summary>
    void Update() {
        // �e���ړ����鑬�x���v�Z���Ĉʒu���X�V
        transform.position += transform.right * _speed * Time.deltaTime;
    }

    /// <summary>
    /// �e���J�����̎��E�O�ɏo���Ƃ��ɌĂ΂��
    /// �e��������鏈��
    /// </summary>
    private void OnBecameInvisible() {
        // �J�����̊O�ɏo���ꍇ�A�e���������
        // ���L�̉���������Ăяo��
        HideFromStage();
    }

    /// <summary>
    /// �e���X�e�[�W��ɔz�u���鏈��
    /// �I�u�W�F�N�g�v�[������؂�Ă����ʒu��ݒ�
    /// </summary>
    public void ShowInStage(Vector3 pos) {
        // �e���w�肳�ꂽ�ʒu�ɕ\��
        transform.position = pos;
    }

    /// <summary>
    /// �e��������鏈��
    /// �I�u�W�F�N�g�v�[���ɖ߂�
    /// </summary>
    public void HideFromStage() {
        // �e���I�u�W�F�N�g�v�[���ɉ��
        // Collect�֐����Ăяo���ăI�u�W�F�N�g�v�[���ɖ߂�
        _objectPool.Collect(this);
    }
}

