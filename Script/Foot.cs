using UnityEngine;
/// <summary>
/// 着地判定
/// </summary>
public class Foot : MonoBehaviour {
    [SerializeField] private GameObject _player;         // プレイヤーオブジェクトへの参照
    private PlayerMove _playerMove;                       // プレイヤーの処理を管理するスクリプトへの参照

    /// <summary>
    /// スクリプトの初期化処理を行います。
    /// </summary>
    void Start() {
        // PlayerMoveスクリプトのインスタンスを取得
        _playerMove = _player.GetComponent<PlayerMove>();
    }

    /// <summary>
    /// プレイヤーの足が地面に触れている間に呼ばれる
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision) {
        // 衝突したオブジェクトが"Ground"タグを持っている場合
        if (collision.gameObject.CompareTag("Ground")) {
            // プレイヤーが地面に接触している場合、ジャンプカウントをリセット
            _playerMove.JumpCount = 0;
        }
    }
}
