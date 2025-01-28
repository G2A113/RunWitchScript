using UnityEngine;
/// <summary>
/// オブジェクトプールの操作
/// </summary>
public class BulletControl : MonoBehaviour {
    [SerializeField]
    private float _speed = 5; // 弾の移動速度を設定（オブジェクトプール用コントローラー格納用変数）

    private ObjectPoolController _objectPool; // オブジェクトプールの参照を格納する変数

    /// <summary>
    /// 弾の初期化処理
    /// オブジェクトプールを親オブジェクトから取得
    /// </summary>
    void Start() {
        // 親オブジェクトからオブジェクトプールを取得
        _objectPool = transform.parent.GetComponent<ObjectPoolController>();
    }

    /// <summary>
    /// 毎フレームの更新処理
    /// 弾が右方向に移動する処理を実行
    /// </summary>
    void Update() {
        // 弾が移動する速度を計算して位置を更新
        transform.position += transform.right * _speed * Time.deltaTime;
    }

    /// <summary>
    /// 弾がカメラの視界外に出たときに呼ばれる
    /// 弾を回収する処理
    /// </summary>
    private void OnBecameInvisible() {
        // カメラの外に出た場合、弾を回収する
        // 下記の回収処理を呼び出し
        HideFromStage();
    }

    /// <summary>
    /// 弾をステージ上に配置する処理
    /// オブジェクトプールから借りてきた位置を設定
    /// </summary>
    public void ShowInStage(Vector3 pos) {
        // 弾を指定された位置に表示
        transform.position = pos;
    }

    /// <summary>
    /// 弾を回収する処理
    /// オブジェクトプールに戻す
    /// </summary>
    public void HideFromStage() {
        // 弾をオブジェクトプールに回収
        // Collect関数を呼び出してオブジェクトプールに戻す
        _objectPool.Collect(this);
    }
}

