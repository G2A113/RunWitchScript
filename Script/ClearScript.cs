using UnityEngine;
/// <summary>
/// クリア条件のエネミーの破壊の記録
/// </summary>
public class ClearScript : MonoBehaviour {
    [SerializeField] private GameObject _player;  // プレイヤーオブジェクト
    private bool _clearGhost;        // ゴーストがクリア条件を満たしているかのフラグ
    private bool _clearEye;        // アイがクリア条件を満たしているかのフラグ
    private bool _clearNow;      // 現在クリア状態かどうかのフラグ

    /// <summary>
    /// 毎フレームの更新処理
    /// クリア条件を満たしたかをチェックし、クリア処理を呼び出します。
    /// </summary>
    private void Update() {
        // ゴーストとアイの両方がクリアされた場合にクリア処理を実行
        if (_clearEye && _clearGhost && !_clearNow) {
            _clearNow = true; // クリア処理を一度だけ実行するためフラグを設定
            _player.GetComponent<PlayerMove>().ClearChange(); // プレイヤーにクリア処理を通知
        }
    }

    /// <summary>
    /// ゴーストがクリア条件を満たした時に呼ばれるメソッド
    /// </summary>
    public void ClearGhorst() {
        _clearGhost = true; // ゴーストのクリア状態を真にする
    }

    /// <summary>
    /// アイがクリア条件を満たした時に呼ばれるメソッド
    /// </summary>
    public void ClearEye() {
        _clearEye = true; // アイのクリア状態を真にする
    }
}
