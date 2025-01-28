using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// フェードアウト用の幕の透過処理
/// シーン間をまたぐ　情報保存用
/// </summary>
public class FadeManager : MonoBehaviour {
    public static bool _isFadeInstance = false;  // Canvasインスタンスが一度しか生成されないようにするフラグ
    public int _keepGameTimer;                          // プレイ時間の保存用
    private bool _isFadeIn = false;             // フェードインを実行中かどうかのフラグ
    private bool _isFadeOut = false;            // フェードアウトを実行中かどうかのフラグ

    private  float _alpha = 0.0f;                // 透過率（0.0f: 完全透明, 1.0f: 完全不透明）
    private const float FADE_SPEED = 0.2f;            // フェードの速さ（時間をどれくらいかけて変化させるか）

    /// <summary>
    /// オブジェクトの起動時に呼ばれる
    /// </summary>
    void Start() {
        if (!_isFadeInstance) // 他にフェードインスタンスがない場合
        {
            DontDestroyOnLoad(this);  // シーン遷移時にもこのオブジェクトを破棄しないようにする
            _isFadeInstance = true;   // フェードインスタンスが一度だけ生成されるようにフラグを立てる
        } else // すでにフェードインスタンスが存在する場合
        {
            Destroy(this); // 2つ目のインスタンスは破棄する
        }
    }

    /// <summary>
    /// 毎フレーム呼ばれる更新処理
    /// フェードインまたはフェードアウトの処理を行います
    /// </summary>
    void Update() {
        if (_isFadeIn) {
            // フェードインの処理（透過率を減少させる）
            _alpha -= Time.deltaTime / FADE_SPEED;
            if (_alpha <= 0.0f) // 透過率が0になったらフェードイン終了
            {
                _isFadeIn = false;
                _alpha = 0.0f;
            }
            // Canvasの画像のアルファ値を更新してフェードインを実現
            this.GetComponentInChildren<Image>().color = new Color(0.0f, 0.0f, 0.0f, _alpha);
        } else if (_isFadeOut) {
            // フェードアウトの処理（透過率を増加させる）
            _alpha += Time.deltaTime / FADE_SPEED;
            if (_alpha >= 1.0f) // 透過率が1になったらフェードアウト終了
            {
                _isFadeOut = false;
                _alpha = 1.0f;
            }
            // Canvasの画像のアルファ値を更新してフェードアウトを実現
            this.GetComponentInChildren<Image>().color = new Color(0.0f, 0.0f, 0.0f, _alpha);
        }
    }

    /// <summary>
    /// フェードインを開始するメソッド
    /// </summary>
    public void FadeIn() {
        _isFadeIn = true;   // フェードインを有効にする
        _isFadeOut = false; // フェードアウトは無効にする
    }

    /// <summary>
    /// フェードアウトを開始するメソッド
    /// </summary>
    public void FadeOut() {
        _isFadeOut = true;  // フェードアウトを有効にする
        _isFadeIn = false;  // フェードインは無効にする
    }
}
