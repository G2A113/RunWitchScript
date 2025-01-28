using UnityEngine;
/// <summary>
/// �ǂ�����Ƃ��̏���
/// </summary>
public class ClawWall : MonoBehaviour {
    [SerializeField] private GameObject _efect; // �ǂ�����G�t�F�N�g�̃v���n�u
    private SEScript _objectSE; // SE�X�N���v�g

    /// <summary>
    /// ����������
    /// SEScript���擾����
    /// </summary>
    void Start() {
        // "SE"�^�O�����I�u�W�F�N�g����SEScript���擾
        _objectSE = GameObject.FindWithTag("SE").GetComponent<SEScript>();
    }

    /// <summary>
    /// �N���[���ǂɐڐG���Ă���Ԃ̏���
    /// </summary>
    /// <param name="collision">�Փ˂����I�u�W�F�N�g�̏��</param>
    private void OnTriggerStay2D(Collider2D collision) {
        // �ߐڍU���݂̂Ŕj��
        if (collision.gameObject.CompareTag("Claw")) {
            // �ǔj���SE���Đ�
            _objectSE.WallBreakSE();

            // �ǔj��̃G�t�F�N�g�𐶐��i������ɔz�u�j
            Instantiate(_efect, transform.position + new Vector3(0, 2, 0), Quaternion.identity);

            // �ǁi���̃Q�[���I�u�W�F�N�g�j���\���ɂ���
            gameObject.SetActive(false);
        }
    }
}

