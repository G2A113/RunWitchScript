using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �w�i�����[�v����������N���X
/// </summary>
public class BackGroundLoop : MonoBehaviour {
    private float _maxLength = 1f; // �e�N�X�`���̌J��Ԃ�����
    private string _propName = "_MainTex"; // �}�e���A���̃v���p�e�B��

    [SerializeField]
    private Vector2 _offsetSpeed; // �e�N�X�`���I�t�Z�b�g�̈ړ����x

    private Material _material; // Image�R���|�[�l���g����擾�����}�e���A��

    /// <summary>
    /// ����������
    /// Image�R���|�[�l���g���擾
    /// </summary>
    private void Start() {
        if (GetComponent<Image>() is Image i) {
            _material = i.material; // Image�R���|�[�l���g�̃}�e���A�����擾
        }
    }

    /// <summary>
    /// ���t���[���̍X�V�������s���܂��B
    /// </summary>
    private void Update() {
        TextureLoop();
    }
    /// <summary>
    /// �w�i�����[�v����������
    /// </summary>
    private void TextureLoop() {
        if (_material) {
            // ���Ԍo�߂ɉ����ăI�t�Z�b�g�l���Z�o���A�e�N�X�`�������[�v
            // Mathf.Repeat()��0�`1�͈̔͂ŌJ��Ԃ�������
            float x = Mathf.Repeat(Time.time * _offsetSpeed.x, _maxLength); // x�����̃I�t�Z�b�g�v�Z
            float y = Mathf.Repeat(Time.time * _offsetSpeed.y, _maxLength); // y�����̃I�t�Z�b�g�v�Z
            Vector2 offset = new Vector2(x, y); // �I�t�Z�b�g��Vector2�Ɋi�[
            _material.SetTextureOffset(_propName, offset); // �}�e���A���̃e�N�X�`���I�t�Z�b�g��ݒ�
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g���j������鎞�A�}�e���A���̃I�t�Z�b�g�����Z�b�g���܂��B
    /// </summary>
    private void OnDestroy() {
        // �Q�[���I����Ƀe�N�X�`���̃I�t�Z�b�g�����Z�b�g
        if (_material) {
            _material.SetTextureOffset(_propName, Vector2.zero); // �I�t�Z�b�g�����ɖ߂�
        }
    }
}

