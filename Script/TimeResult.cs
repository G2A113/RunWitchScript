using UnityEngine;
using TMPro;
/// <summary>
/// ゲーム終了後、スコア結果を表示する処理。
/// </summary>
public class TimeResult : MonoBehaviour {
    // 時間結果を保持する変数
    private int _timeResult;

    // UIに表示するためのTextMeshProUGUIコンポーネント
    public TextMeshProUGUI _countText;

    /// <summary>
    /// スクリプトの初期化処理を行います。
    /// FadeManagerからスコアを取得し、6桁のゼロ埋め形式で表示します。
    /// </summary>
    void Start() {
        // "Carten"タグのGameObjectからFadeManagerコンポーネントを取得
        FadeManager fadeManager = GameObject.FindWithTag("Carten")?.GetComponent<FadeManager>();

        // FadeManagerが見つかった場合、スコアを保持している変数から結果を取得
        // 見つからなかった場合はスコアを0とする
        int score = fadeManager != null ? fadeManager._keepGameTimer : 0;

        // 6桁のゼロ埋め形式でスコアを表示
        _countText.text = string.Format("{0:000000}", score);
    }
}
