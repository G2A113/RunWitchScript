using UnityEngine;
/// <summary>
/// 壁が壊れるときの処理
/// </summary>
public class ClawWall : MonoBehaviour {
    [SerializeField] private GameObject _efect; // 壁が壊れるエフェクトのプレハブ
    private SEScript _objectSE; // SEスクリプト

    /// <summary>
    /// 初期化処理
    /// SEScriptを取得する
    /// </summary>
    void Start() {
        // "SE"タグを持つオブジェクトからSEScriptを取得
        _objectSE = GameObject.FindWithTag("SE").GetComponent<SEScript>();
    }

    /// <summary>
    /// クローが壁に接触している間の処理
    /// </summary>
    /// <param name="collision">衝突したオブジェクトの情報</param>
    private void OnTriggerStay2D(Collider2D collision) {
        // 近接攻撃のみで破壊
        if (collision.gameObject.CompareTag("Claw")) {
            // 壁破壊のSEを再生
            _objectSE.WallBreakSE();

            // 壁破壊のエフェクトを生成（少し上に配置）
            Instantiate(_efect, transform.position + new Vector3(0, 2, 0), Quaternion.identity);

            // 壁（このゲームオブジェクト）を非表示にする
            gameObject.SetActive(false);
        }
    }
}

