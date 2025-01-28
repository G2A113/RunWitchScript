using UnityEngine;

/// <summary>
/// エネミー　ゴーストの動き、攻撃、シールド処理を管理するクラス。
/// </summary>
public class GhostMove : MonoBehaviour {
    // 外部スクリプトとオブジェクトの参照
    private ClearScript _clear; // ゲームクリアを管理するスクリプト
    private SEScript _objectSE; // 効果音を管理するスクリプト
    [SerializeField] private GameObject _barrier; // 無効時のバリアエフェクト
    [SerializeField] private GameObject _efect; // 爆発エフェクト
    [SerializeField] private GameObject _eyeEnemy; // 次に生成される目の敵
    [SerializeField] private GameObject _nextEnemy; // 次に生成されるゴーストの敵

    // 移動と攻撃の管理
    private bool _start; // 敵が動き始めたかどうか
    [SerializeField] private float _startLength = 5; // 敵が動き始める距離
    private float _uniquTime = 0; // 縦方向の移動用タイマー
    private Vector3 _initialPosition; // 初期位置

    [Header("縦方向の設定")]
    [SerializeField] private float _speedVertical = 1; // 縦方向の速度
    [SerializeField] private float _rangeVertical = 1; // 縦方向の移動範囲

    [Header("横方向の設定")]
    [SerializeField] private float _speedHorizontal = 1; // 横方向の速度
    [SerializeField] private float _rangeHorizontal = 9; // 横方向の移動範囲

    private GameObject _camera; // メインカメラの参照

    private bool _attack; // 攻撃中かどうか
    private float _attackInterval; // 攻撃インターバル
    private float _attackIntervalLimit = 3; // 攻撃インターバルの限界値

    // アニメーション管理
    private Animator _anim; // アニメーターコンポーネント
    private string[] _animeBool = { "Attack", "Attack1", "Attack2", "Attack3" }; // 攻撃アニメーション名
    private int _randomStoc; // ランダムで選択されたアニメーション
    private float _stopTime; // 横方向の移動時間
    private bool _goAhead = true; // 横方向の移動方向（true:前進, false:後退）

    // シールド管理
    [SerializeField] private GameObject _breakEfect; // シールド破壊時のエフェクト
    [SerializeField] private GameObject _auraObject; // シールドオーラ
    [SerializeField] private int _shirldCount = 3; // シールド耐久値

    /// <summary>
    /// スクリプトの初期化処理を行います。
    /// 必要なコンポーネントの設定を行います。
    /// </summary>
    void Start() {
        // 必要なコンポーネントの取得
        _camera = GameObject.FindWithTag("MainCamera");
        _anim = gameObject.GetComponent<Animator>();
        _objectSE = GameObject.FindWithTag("SE").GetComponent<SEScript>();
        _clear = GameObject.FindWithTag("Clear").GetComponent<ClearScript>();
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
    /// 移動，攻撃処理
    /// </summary>
    void Update() {
        if (_start && !_attack) {
            // 縦方向の移動処理
            _uniquTime += Time.deltaTime * _speedVertical;
            _attackInterval += Time.deltaTime;
            transform.position = new Vector3(
                _camera.transform.position.x + _startLength,
                (Mathf.Sin(_uniquTime) * _rangeVertical) + _initialPosition.y,
                _initialPosition.z);
        } else if (_attack) {
            // 攻撃中の横方向移動処理
            if (_goAhead) {
                _stopTime += Time.deltaTime * _speedHorizontal;
            } else {
                _stopTime -= Time.deltaTime * _speedHorizontal;
            }
            transform.position = new Vector3(
                -(Mathf.Sin(_stopTime) * _rangeHorizontal) + _camera.transform.position.x + _startLength,
                (Mathf.Sin(_uniquTime) * _rangeVertical) + _initialPosition.y,
                _initialPosition.z);
            if (_stopTime < 0) {
                _attack = false;
                _stopTime = 0;
            }
        } else if (!_start) {
            // 一定距離内に入った場合、動き始める
            if (transform.position.x - _camera.transform.position.x <= _startLength) {
                _start = true;
                _initialPosition = transform.position;
            }
        }

        // 攻撃インターバルが限界に達した場合、攻撃を開始
        if (_attackInterval >= _attackIntervalLimit - 1 && !_attack && !transform.Find("area").gameObject.activeSelf) {
            transform.Find("area").gameObject.SetActive(true);
        }
        if (_attackInterval >= _attackIntervalLimit && !_attack) {
            transform.Find("area").gameObject.SetActive(false);
            _goAhead = true;
            _attackIntervalLimit = Random.Range(6, 10);
            _attack = true;
            _randomStoc = Random.Range(1, 3);
            _anim.SetBool(_animeBool[_randomStoc], true);
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
            HandleEnemyDefeat();
        } else {
            _objectSE.CounterSE();
            Instantiate(_barrier, transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Bulletタグとの衝突時の処理
    /// </summary>
    private void HandleBulletCollision(Collider2D collision) {
        if (_shirldCount > 0) {
            _shirldCount--;
            _objectSE.ShieldBreakSE();
            Instantiate(_breakEfect, transform.position, Quaternion.identity);
            if (_shirldCount == 0) {
                _auraObject.SetActive(false);
            }
        }
        collision.gameObject.GetComponent<BulletControl>().HideFromStage();
    }

    /// <summary>
    /// 敵の撃破処理
    /// </summary>
    private void HandleEnemyDefeat() {
        _objectSE.BombSE();
        Instantiate(_efect, transform.position, Quaternion.identity);

        // 次にゴーストか目の敵を生成
        if (_nextEnemy != null) {
            Instantiate(_nextEnemy, new Vector2(transform.position.x + 50, 3), Quaternion.identity);
        } else {
            _clear.ClearGhorst();
        }
        if (_eyeEnemy != null) {
            Instantiate(_eyeEnemy, new Vector2(transform.position.x + 50, 3), Quaternion.identity);
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 攻撃終了時の処理
    /// </summary>
    private void AttackFalse() {
        _goAhead = true;
        _anim.SetBool(_animeBool[_randomStoc], false);
    }

    /// <summary>
    /// 攻撃中のサウンド再生
    /// </summary>
    private void AttackSound() {
        _objectSE.GhostSE();
    }
}
