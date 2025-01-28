using UnityEngine;

/// <summary>
/// このクラスは、トラップが弾と衝突した際の処理を管理します。
/// 衝突時に爆発エフェクトが表示され、弾をステージから隠し、トラップ自身を無効化します。
/// </summary>
public class TrapScript : MonoBehaviour {
    // 爆発エフェクトのプレハブ
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
    /// 衝突処理: トラップが弾と衝突した時
    /// </summary>
    /// <param name="collision">衝突したオブジェクトの情報。</param>
    private void OnTriggerEnter2D(Collider2D collision) {
        // 衝突したオブジェクトが弾の場合
        if (collision.gameObject.CompareTag("Bullet")) {
            // 爆発音を再生
            _objectSE.BombSE();

            // トラップ位置に爆発エフェクトを生成
            Instantiate(_efect, transform.position, Quaternion.identity);

            // トラップを無効化
            gameObject.SetActive(false);

            // 弾をステージから隠す
            collision.gameObject.GetComponent<BulletControl>().HideFromStage();
        }
    }
}

