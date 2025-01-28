using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// プレイヤーが発射する弾丸のオブジェクトプール
/// </summary>
public class ObjectPoolController : MonoBehaviour {
    // 弾のプレファブ
    [SerializeField] private BulletControl _bullet;
    // 生成する弾の最大数
    [SerializeField] private int _maxCount;
    // 生成した弾を格納するQueue（先入れ先出し）
    [SerializeField] private Queue<BulletControl> _bulletQueue;
    // 初回生成時のポジション
    private Vector3 _setPos = new Vector3(100, 100, 0);

    /// <summary>
    /// 起動時にオブジェクトプールを初期化し、弾を所定の数だけ生成してQueueに格納する。
    /// </summary>
    private void Awake() {
        // Queueの初期化
        _bulletQueue = new Queue<BulletControl>();

        // 指定された数だけ弾を生成し、Queueに追加
        for (int i = 0; i < _maxCount; i++) {
            // 弾を生成
            BulletControl tmpBullet = Instantiate(_bullet, _setPos, Quaternion.identity, transform);
            // Queueに弾を追加
            _bulletQueue.Enqueue(tmpBullet);
        }
    }

    /// <summary>
    /// 弾をオブジェクトプールから貸し出す。
    /// </summary>
    /// <param name="pos">弾を出現させる位置</param>
    /// <returns>貸し出された弾のBulletControlオブジェクト</returns>
    public BulletControl Launch(Vector3 pos) {
        // Queueが空の場合、弾を貸し出せないのでnullを返す
        if (_bulletQueue.Count <= 0) {
            return null; // 弾がない場合はnullを返す
        }

        // Queueから弾を取り出す
        BulletControl tmpBullet = _bulletQueue.Dequeue();
        // 弾を表示
        tmpBullet.gameObject.SetActive(true);
        // 弾を指定された位置に移動
        tmpBullet.ShowInStage(pos);
        // 呼び出し元に弾を返す
        return tmpBullet;
    }

    /// <summary>
    /// 使用した弾をオブジェクトプールに回収する。
    /// </summary>
    /// <param name="bullet">回収する弾のBulletControlオブジェクト</param>
    public void Collect(BulletControl bullet) {
        // 弾のゲームオブジェクトを非表示にする
        bullet.gameObject.SetActive(false);
        // 弾をQueueに戻す
        _bulletQueue.Enqueue(bullet);
    }
}
