using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �t�F�[�h�A�E�g�p�̖��̓��ߏ���
/// �V�[���Ԃ��܂����@���ۑ��p
/// </summary>
public class FadeManager : MonoBehaviour {
    public static bool _isFadeInstance = false;  // Canvas�C���X�^���X����x������������Ȃ��悤�ɂ���t���O
    public int _keepGameTimer;                          // �v���C���Ԃ̕ۑ��p
    private bool _isFadeIn = false;             // �t�F�[�h�C�������s�����ǂ����̃t���O
    private bool _isFadeOut = false;            // �t�F�[�h�A�E�g�����s�����ǂ����̃t���O

    private  float _alpha = 0.0f;                // ���ߗ��i0.0f: ���S����, 1.0f: ���S�s�����j
    private const float FADE_SPEED = 0.2f;            // �t�F�[�h�̑����i���Ԃ��ǂꂭ�炢�����ĕω������邩�j

    /// <summary>
    /// �I�u�W�F�N�g�̋N�����ɌĂ΂��
    /// </summary>
    void Start() {
        if (!_isFadeInstance) // ���Ƀt�F�[�h�C���X�^���X���Ȃ��ꍇ
        {
            DontDestroyOnLoad(this);  // �V�[���J�ڎ��ɂ����̃I�u�W�F�N�g��j�����Ȃ��悤�ɂ���
            _isFadeInstance = true;   // �t�F�[�h�C���X�^���X����x�������������悤�Ƀt���O�𗧂Ă�
        } else // ���łɃt�F�[�h�C���X�^���X�����݂���ꍇ
        {
            Destroy(this); // 2�ڂ̃C���X�^���X�͔j������
        }
    }

    /// <summary>
    /// ���t���[���Ă΂��X�V����
    /// �t�F�[�h�C���܂��̓t�F�[�h�A�E�g�̏������s���܂�
    /// </summary>
    void Update() {
        if (_isFadeIn) {
            // �t�F�[�h�C���̏����i���ߗ�������������j
            _alpha -= Time.deltaTime / FADE_SPEED;
            if (_alpha <= 0.0f) // ���ߗ���0�ɂȂ�����t�F�[�h�C���I��
            {
                _isFadeIn = false;
                _alpha = 0.0f;
            }
            // Canvas�̉摜�̃A���t�@�l���X�V���ăt�F�[�h�C��������
            this.GetComponentInChildren<Image>().color = new Color(0.0f, 0.0f, 0.0f, _alpha);
        } else if (_isFadeOut) {
            // �t�F�[�h�A�E�g�̏����i���ߗ��𑝉�������j
            _alpha += Time.deltaTime / FADE_SPEED;
            if (_alpha >= 1.0f) // ���ߗ���1�ɂȂ�����t�F�[�h�A�E�g�I��
            {
                _isFadeOut = false;
                _alpha = 1.0f;
            }
            // Canvas�̉摜�̃A���t�@�l���X�V���ăt�F�[�h�A�E�g������
            this.GetComponentInChildren<Image>().color = new Color(0.0f, 0.0f, 0.0f, _alpha);
        }
    }

    /// <summary>
    /// �t�F�[�h�C�����J�n���郁�\�b�h
    /// </summary>
    public void FadeIn() {
        _isFadeIn = true;   // �t�F�[�h�C����L���ɂ���
        _isFadeOut = false; // �t�F�[�h�A�E�g�͖����ɂ���
    }

    /// <summary>
    /// �t�F�[�h�A�E�g���J�n���郁�\�b�h
    /// </summary>
    public void FadeOut() {
        _isFadeOut = true;  // �t�F�[�h�A�E�g��L���ɂ���
        _isFadeIn = false;  // �t�F�[�h�C���͖����ɂ���
    }
}
