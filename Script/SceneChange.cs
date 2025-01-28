using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

/// <summary>
/// �V�[���J�ڂƃQ�[���I���������Ǘ�����N���X
/// </summary>
public class SceneChange : MonoBehaviour {
    // �t�F�[�h�p��Canvas�v���n�u
    [SerializeField] private GameObject _fadePrefab;
    // ���삷��Canvas
    private GameObject _fadeCanvas;
    // �{�^�������\�t���O
    private bool _canButton = true;

    // �t�F�[�h�A�E�g�L�����o�X��T���܂ł̑ҋ@����
    private const float FADE_CANVAS_SEARCH_DELAY = 0.2f;
    // �t�F�[�h�A�E�g�܂ł̑ҋ@����
    private const int FADE_OUT_DELAY = 1000;
    // �t�F�[�h�A�E�g���̑ҋ@����
    private const int FADE_OUT_NOW_DELAY = 300;
    // �Q�[���I���܂ł̑ҋ@����
    private const int GAME_END_DELAY = 500;

    /// <summary>
    /// �����������B
    /// �t�F�[�hCanvas�����݂��Ȃ��ꍇ�A�C���X�^���X�𐶐����A�����҂��Ă���T���n�߂�B
    /// </summary>
    private void Start() {
        // �t�F�[�hCanvas�����݂��Ȃ���ΐ���
        if (!FadeManager._isFadeInstance) {
            Instantiate(_fadePrefab);
        }
        // Canvas�������҂��Ă���T��
        Invoke(nameof(FindFadeObject), FADE_CANVAS_SEARCH_DELAY);
    }

    /// <summary>
    /// �w�肳�ꂽ�V�[���ɑJ�ڂ���
    /// </summary>
    /// <param name="sceneName">�J�ڐ�V�[����</param>
    public async void ChangeScene(string sceneName) {
        if (_canButton) {
            _canButton = false;
            // �t�F�[�h�A�E�g�O�ɑҋ@
            await Task.Delay(FADE_OUT_DELAY);
            // �t�F�[�h�A�E�g�J�n
            _fadeCanvas.GetComponent<FadeManager>().FadeOut();
            // �t�F�[�h�A�E�g���ɑҋ@
            await Task.Delay(FADE_OUT_NOW_DELAY);
            // �V�[���J��
            SceneManager.LoadScene(sceneName);
        }
    }

    /// <summary>
    /// �Q�[�����I������
    /// </summary>
    public async void EndGame() {
        if (_canButton) {
            _canButton = false;
            // �t�F�[�h�A�E�g�J�n
            _fadeCanvas.GetComponent<FadeManager>().FadeOut();
            // �t�F�[�h�A�E�g���ɑҋ@
            await Task.Delay(GAME_END_DELAY);
            // �Q�[���I��
            Application.Quit();

#if UNITY_EDITOR
            // �G�f�B�^�ł̒�~����
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    /// <summary>
    /// �t�F�[�hCanvas��T���A�t�F�[�h�C�����������s����
    /// </summary>
    private void FindFadeObject() {
        // �w�肳�ꂽ�^�O��Canvas��T��
        _fadeCanvas = GameObject.FindGameObjectWithTag("Carten");
        if (_fadeCanvas == null) {
            Debug.LogError("�t�F�[�hCanvas��������܂���B");
            return;
        }

        // Canvas��FadeManager�R���|�[�l���g���A�^�b�`����Ă��邩�m�F
        FadeManager fadeManager = _fadeCanvas.GetComponent<FadeManager>();
        if (fadeManager == null) {
            Debug.LogError("FadeManager��Canvas�ɃA�^�b�`����Ă��܂���B");
            return;
        }

        // �t�F�[�h�C�����������s
        fadeManager.FadeIn();
    }
}
