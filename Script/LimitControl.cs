using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// プレイヤーの移動に応じてダッシュ速度の制御を行うクラスです。
/// </summary>
public class LimitControl : MonoBehaviour {
    // プレイヤーオブジェクト
    [SerializeField] private GameObject _player;
    private PlayerMove _playerMove; // PlayerMove スクリプトの参照
    private InputAction _move; // プレイヤーの移動入力を取得するためのアクション

    /// <summary>
    /// スクリプトの初期化処理を行います。
    /// 必要なコンポーネントの設定を行います。
    /// </summary>
    void Start() {
        // PlayerInput コンポーネントから「Move」アクションを取得
        _move = gameObject.GetComponent<PlayerInput>().actions["Move"];

        // プレイヤーの PlayerMove スクリプトを取得
        _playerMove = _player.GetComponent<PlayerMove>();
    }
    /// <summary>
    /// 毎フレームの更新処理
    /// 右方向に移動するとダッシュ速度を増加させ、左方向に移動する際にはダッシュ速度を減少させる制限
    /// </summary>
    void Update() {
        // 入力された移動のベクトルを取得（x軸のみ使用）
        Vector2 inputMoveAxis = _move.ReadValue<Vector2>();

        // x軸方向が正（右）に動いたとき、ダッシュ速度を増加
        if (inputMoveAxis.x > 0) {
            _playerMove.DashSpeedNow += 0.1f; // 右方向に移動中、ダッシュ速度を増加
        }

        // x軸方向が負（左）に動いたとき、かつダッシュ速度が1より大きい場合にダッシュ速度を減少
        if ((inputMoveAxis.x < 0) && _playerMove.DashSpeedNow > 1) {
            _playerMove.DashSpeedNow -= 1.0f; // 左方向に移動時、後退制限としてダッシュ速度を減少
        }
    }
}
