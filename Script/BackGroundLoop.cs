using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 背景をループさせ続けるクラス
/// </summary>
public class BackGroundLoop : MonoBehaviour {
    private float _maxLength = 1f; // テクスチャの繰り返し制限
    private string _propName = "_MainTex"; // マテリアルのプロパティ名

    [SerializeField]
    private Vector2 _offsetSpeed; // テクスチャオフセットの移動速度

    private Material _material; // Imageコンポーネントから取得したマテリアル

    /// <summary>
    /// 初期化処理
    /// Imageコンポーネントを取得
    /// </summary>
    private void Start() {
        if (GetComponent<Image>() is Image i) {
            _material = i.material; // Imageコンポーネントのマテリアルを取得
        }
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
    /// </summary>
    private void Update() {
        TextureLoop();
    }
    /// <summary>
    /// 背景をループさせ続ける
    /// </summary>
    private void TextureLoop() {
        if (_material) {
            // 時間経過に応じてオフセット値を算出し、テクスチャをループ
            // Mathf.Repeat()で0～1の範囲で繰り返し動かす
            float x = Mathf.Repeat(Time.time * _offsetSpeed.x, _maxLength); // x方向のオフセット計算
            float y = Mathf.Repeat(Time.time * _offsetSpeed.y, _maxLength); // y方向のオフセット計算
            Vector2 offset = new Vector2(x, y); // オフセットをVector2に格納
            _material.SetTextureOffset(_propName, offset); // マテリアルのテクスチャオフセットを設定
        }
    }

    /// <summary>
    /// オブジェクトが破棄される時、マテリアルのオフセットをリセットします。
    /// </summary>
    private void OnDestroy() {
        // ゲーム終了後にテクスチャのオフセットをリセット
        if (_material) {
            _material.SetTextureOffset(_propName, Vector2.zero); // オフセットを元に戻す
        }
    }
}

