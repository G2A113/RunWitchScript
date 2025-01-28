using UnityEngine;
using TMPro;
/// <summary>
/// �Q�[���I����A�X�R�A���ʂ�\�����鏈���B
/// </summary>
public class TimeResult : MonoBehaviour {
    // ���Ԍ��ʂ�ێ�����ϐ�
    private int _timeResult;

    // UI�ɕ\�����邽�߂�TextMeshProUGUI�R���|�[�l���g
    public TextMeshProUGUI _countText;

    /// <summary>
    /// �X�N���v�g�̏������������s���܂��B
    /// FadeManager����X�R�A���擾���A6���̃[�����ߌ`���ŕ\�����܂��B
    /// </summary>
    void Start() {
        // "Carten"�^�O��GameObject����FadeManager�R���|�[�l���g���擾
        FadeManager fadeManager = GameObject.FindWithTag("Carten")?.GetComponent<FadeManager>();

        // FadeManager�����������ꍇ�A�X�R�A��ێ����Ă���ϐ����猋�ʂ��擾
        // ������Ȃ������ꍇ�̓X�R�A��0�Ƃ���
        int score = fadeManager != null ? fadeManager._keepGameTimer : 0;

        // 6���̃[�����ߌ`���ŃX�R�A��\��
        _countText.text = string.Format("{0:000000}", score);
    }
}
