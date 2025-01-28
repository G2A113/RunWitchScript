using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移とゲーム終了処理を管理するクラス
/// </summary>
public class SceneChange : MonoBehaviour {
    // フェード用のCanvasプレハブ
    [SerializeField] private GameObject _fadePrefab;
    // 操作するCanvas
    private GameObject _fadeCanvas;
    // ボタン押下可能フラグ
    private bool _canButton = true;

    // フェードアウトキャンバスを探すまでの待機時間
    private const float FADE_CANVAS_SEARCH_DELAY = 0.2f;
    // フェードアウトまでの待機時間
    private const int FADE_OUT_DELAY = 1000;
    // フェードアウト中の待機時間
    private const int FADE_OUT_NOW_DELAY = 300;
    // ゲーム終了までの待機時間
    private const int GAME_END_DELAY = 500;

    /// <summary>
    /// 初期化処理。
    /// フェードCanvasが存在しない場合、インスタンスを生成し、少し待ってから探し始める。
    /// </summary>
    private void Start() {
        // フェードCanvasが存在しなければ生成
        if (!FadeManager._isFadeInstance) {
            Instantiate(_fadePrefab);
        }
        // Canvasを少し待ってから探す
        Invoke(nameof(FindFadeObject), FADE_CANVAS_SEARCH_DELAY);
    }

    /// <summary>
    /// 指定されたシーンに遷移する
    /// </summary>
    /// <param name="sceneName">遷移先シーン名</param>
    public async void ChangeScene(string sceneName) {
        if (_canButton) {
            _canButton = false;
            // フェードアウト前に待機
            await Task.Delay(FADE_OUT_DELAY);
            // フェードアウト開始
            _fadeCanvas.GetComponent<FadeManager>().FadeOut();
            // フェードアウト中に待機
            await Task.Delay(FADE_OUT_NOW_DELAY);
            // シーン遷移
            SceneManager.LoadScene(sceneName);
        }
    }

    /// <summary>
    /// ゲームを終了する
    /// </summary>
    public async void EndGame() {
        if (_canButton) {
            _canButton = false;
            // フェードアウト開始
            _fadeCanvas.GetComponent<FadeManager>().FadeOut();
            // フェードアウト中に待機
            await Task.Delay(GAME_END_DELAY);
            // ゲーム終了
            Application.Quit();

#if UNITY_EDITOR
            // エディタでの停止処理
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    /// <summary>
    /// フェードCanvasを探し、フェードイン処理を実行する
    /// </summary>
    private void FindFadeObject() {
        // 指定されたタグでCanvasを探す
        _fadeCanvas = GameObject.FindGameObjectWithTag("Carten");
        if (_fadeCanvas == null) {
            Debug.LogError("フェードCanvasが見つかりません。");
            return;
        }

        // CanvasにFadeManagerコンポーネントがアタッチされているか確認
        FadeManager fadeManager = _fadeCanvas.GetComponent<FadeManager>();
        if (fadeManager == null) {
            Debug.LogError("FadeManagerがCanvasにアタッチされていません。");
            return;
        }

        // フェードイン処理を実行
        fadeManager.FadeIn();
    }
}
