using UnityEngine;
/// <summary>
/// エネミー　アイの動き、攻撃、シールド管理、破壊処理を行うクラス。
/// </summary>
public class EyeEnemyMove : MonoBehaviour {
    // 次の敵に関する設定
    private ClearScript _clearScript; // ゲームクリア状態を管理するスクリプト
    [SerializeField] private GameObject _nextEnemy; // 次に生成される敵のプレハブ
    [SerializeField] private int _nextEnemyPositionX = 50; // 次の敵の生成位置（X軸）
    [SerializeField] private int _nextEnemyPositionY = 3;  // 次の敵の生成位置（Y軸）

    // 敵の動きに関する設定
    [SerializeField] private GameObject _barrier;  // 無効時に表示するバリア
    [SerializeField] private GameObject _efect;    // 爆発エフェクト
    private const float INITIAL_POSITION_Y = 3;          // 初期位置のY座標
    [Header("上下幅")] [SerializeField] private float _range = 1; // 上下に動く範囲
    private float _uniquTime = 0;                 // 動きのタイミング
    [Header("速度")] [SerializeField] private float _speed = 1; // 上下移動の速度

    // 攻撃に関する設定
    private float _attackInterval;                // 現在の攻撃インターバル時間
    private float _attackIntervalLimit;           // 攻撃インターバルの限界値
    [SerializeField] private float _attackLimitMin = 3; // 攻撃間隔の最小値
    [SerializeField] private float _attackLimitMax = 5; // 攻撃間隔の最大値

    // その他の設定
    private GameObject _camera;                   // メインカメラ
    [SerializeField] private float _startLength = 5; // 敵が動き出す距離
    private Animator _anim;                       // 敵のアニメーター
    private bool _start;                          // 敵が動き始めたか
    private SEScript _objectSE;                   // 効果音スクリプト
    private bool _attack;                         // 攻撃中かどうか
    [SerializeField] private GameObject _breakEfect; // シールドが壊れた際のエフェクト
    [SerializeField] private GameObject _auraObject; // シールドのオーラ表示
    [SerializeField] private int _shirldCount = 3;   // シールドの耐久値

    void Start() {
        // 敵を親オブジェクトから切り離す
        gameObject.transform.parent = null;

        // 攻撃間隔をランダムで初期化
        _attackIntervalLimit = Random.Range(_attackLimitMin, _attackLimitMax);

        // 必要なコンポーネントを取得
        _anim = gameObject.GetComponent<Animator>();
        _camera = GameObject.FindWithTag("MainCamera");
        _objectSE = GameObject.FindWithTag("SE").GetComponent<SEScript>();
        _clearScript = GameObject.FindWithTag("Clear").GetComponent<ClearScript>();
    }

    void Update() {
        if (_start && !_attack) {
            // 上下移動処理
            _uniquTime += Time.deltaTime * _speed;
            _attackInterval += Time.deltaTime;
            transform.position = new Vector2(
                _camera.transform.position.x + _startLength,
                (Mathf.Sin(_uniquTime) * _range) + INITIAL_POSITION_Y);
        } else if (_attack) {
            // 攻撃中も位置を維持
            transform.position = new Vector2(
                _camera.transform.position.x + _startLength,
                (Mathf.Sin(_uniquTime) * _range) + INITIAL_POSITION_Y);
        } else if (!_start) {
            // 一定距離内に入ると敵が動き出す
            if (transform.position.x - _camera.transform.position.x <= _startLength) {
                _start = true;
            }
        }

        // 攻撃タイミングの管理
        if (_attackInterval >= _attackIntervalLimit && !_attack) {
            _attackIntervalLimit = Random.Range(_attackLimitMin, _attackLimitMax);
            _attack = true;
            _anim.SetBool("EyeAttack", true); // 攻撃アニメーションを開始
            _attackInterval = 0;
        }
    }

    /// <summary>
    /// 衝突時の処理
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Claw")) {
            HandleClawCollision();
        }
        if (collision.gameObject.CompareTag("Bullet")) {
            HandleBulletCollision(collision);
        }
    }

    /// <summary>
    /// Clawタグとの衝突時の処理
    /// </summary>
    private void HandleClawCollision() {
        if (_shirldCount == 0) {
            SpawnNextEnemyOrClear();
        } else {
            _objectSE.CounterSE(); // 無効時の効果音
            Instantiate(_barrier, transform.position, Quaternion.identity); // バリアエフェクト
        }
    }

    /// <summary>
    /// Bulletタグとの衝突時の処理
    /// </summary>
    private void HandleBulletCollision(Collider2D collision) {
        if (_shirldCount > 0) {
            _shirldCount--;
            _objectSE.ShieldBreakSE(); // シールド破壊音
            Instantiate(_breakEfect, transform.position, Quaternion.identity);
            if (_shirldCount == 0) {
                _auraObject.SetActive(false); // シールドオーラを無効化
            }
        } else {
            _objectSE.CounterSE(); // シールドがない場合の音
            Instantiate(_barrier, transform.position, Quaternion.identity);
        }
        collision.gameObject.GetComponent<BulletControl>().HideFromStage(); // 弾を消去
    }

    /// <summary>
    /// 次の敵を生成するかクリア処理を行う
    /// </summary>
    private void SpawnNextEnemyOrClear() {
        if (_nextEnemy != null) {
            Instantiate(_nextEnemy, new Vector2(transform.position.x + _nextEnemyPositionX, _nextEnemyPositionY), Quaternion.identity);
        } else {
            _clearScript.ClearEye(); // クリアフラグ　アイの処理
        }
        Instantiate(_efect, transform.position, Quaternion.identity); // 爆発エフェクト
        _objectSE.BombSE(); // 爆発音
        gameObject.SetActive(false); // 自身を非アクティブ化
    }

    /// <summary>
    /// 攻撃終了時の処理
    /// </summary>
    private void EyeAttackFalse() {
        _anim.SetBool("EyeAttack", false);
    }

    /// <summary>
    /// 攻撃完了時の状態リセット
    /// </summary>
    private void EyeAttackEnd() {
        _attack = false;
    }

    /// <summary>
    /// 攻撃中のサウンドエフェクト再生
    /// </summary>
    private void EyeAttackSunder() {
        _objectSE.SunderSE();
    }
}
