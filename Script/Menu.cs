using UnityEngine;
using UnityEngine.UI; // UI�R���|�[�l���g�̎g�p
/// <summary>
/// ���j���[��ʂŃ{�^���̑I����Ԃ��Ǘ�����N���X
/// </summary>
public class Menu : MonoBehaviour {
    Button _cube; // �{�^���̎Q��

    
    void Start() {
        // �{�^���R���|�[�l���g�̎擾
        _cube = GameObject.Find("/SelectCanvas/Button1").GetComponent<Button>();

        // �ŏ��ɑI����Ԃɂ������{�^���̐ݒ�
        _cube.Select(); // ���̍s�Ń{�^����I����Ԃɂ���
    }
}
