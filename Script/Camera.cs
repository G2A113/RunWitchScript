using UnityEngine;

/// <summary>
/// カメラの動きとプレイヤーのスコアに応じた速度管理を行うクラス
/// </summary>
public class Camera : MonoBehaviour {

    [Header("カメラの動き設定")]
    public Rigidbody2D _rigidbody; // カメラのリジッドボディ（動きの制御に使用）
    [SerializeField] private float _speed = 6; // 速度（横方向にカメラを動かす速度）

    /// <summary>
    /// 初期化処理
    /// カメラのリジッドボディを取得
    /// </summary>
    void Start() {
        // カメラのリジッドボディを取得し、変数に格納
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 毎フレームの更新処理
    /// カメラを一定の速度で移動させる
    /// </summary>
    void Update() {
        // 横方向（X軸）の速度を設定し、垂直方向（Y軸）はそのままにする
        _rigidbody.velocity = new Vector2(_speed, _rigidbody.velocity.y);
    }
}
