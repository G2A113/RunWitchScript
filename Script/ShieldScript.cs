using UnityEngine;

/// <summary>
/// このクラスは、シールドが弾と衝突した際の処理を管理します。
/// 衝突時にシールドが破壊され、エフェクトが表示され、弾をステージから隠します。
/// </summary>
public class ShieldScript : MonoBehaviour {
    // シールド破壊時に表示するエフェクトのプレハブ
    [SerializeField] private GameObject _efect;

    // 効果音を管理するSEScriptの参照
    private SEScript _objectSE;
    /// <summary>
    /// スクリプトの初期化処理を行います。
    /// 必要なコンポーネントや変数の設定を行います。
    /// </summary>
    void Start() {
        // SEScriptのインスタンスを取得
        _objectSE = GameObject.FindWithTag("SE").GetComponent<SEScript>();
    }


    /// <summary>
    /// 衝突処理: シールドが弾と衝突した時
    /// </summary>
    /// <param name="collision">衝突したオブジェクトの情報。</param>
    private void OnTriggerEnter2D(Collider2D collision) {
        // 衝突したオブジェクトが弾の場合
        if (collision.gameObject.CompareTag("Bullet")) {
            // シールド破壊の効果音を再生
            _objectSE.ShieldBreakSE();

            // シールドの位置にエフェクトを生成
            Instantiate(_efect, transform.position, Quaternion.identity);

            // シールドを非表示にして無効化
            gameObject.SetActive(false);

            // 弾をステージから隠す
            collision.gameObject.GetComponent<BulletControl>().HideFromStage();
        }
    }
}

