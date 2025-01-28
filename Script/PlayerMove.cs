using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの動き、ジャンプ、攻撃、スコア管理、シーン遷移を制御するクラス。
/// </summary>
public class PlayerMove : MonoBehaviour {
    #region 変数
    [Header("シーン遷移設定")]
    [SerializeField] private string _clearScene; // クリア時のシーン名
    [SerializeField] private string _gameOverScene = default;  // ゲームオーバー時のシーン名

    [Header("オブジェクトプールとエフェクト")]
    [SerializeField] private ObjectPoolController _objectPool = default; // 弾丸のオブジェクトプール
    [SerializeField] private GameObject _sceneChangeManager = null;   // シーン変更を管理するオブジェクト
    [SerializeField] private GameObject _clawSlashEffect = null;      // 特殊攻撃のエフェクト
    

    [Header("プレイヤーの能力設定")]
    [SerializeField] private float _defaltJumpPower = 700f; // ジャンプの力
    [SerializeField] private  float _specialJumpPowerChange = 0.85f; // 特殊攻撃ジャンプの力
    private const float TIMER_INCREASE_RATE = 1f; // 毎秒タイムが増加する量

    private const float DASH_SPEED_MIN = 1f;// ダッシュ速度の最小値
    private const float DASH_SPEED_MAX = 10f;// ダッシュ速度の最大値isusing UnityEngine;
    [SerializeField]
    private float _primaryDashSpeed = 6;
    [Header("UI設定"), SerializeField]
    private TextMeshProUGUI _timerText = default; // タイム表示用のUIテキスト
    private float _keepTimer = default;                // 決定のタイム

    // コンポーネントや状態管理用変数
    private Rigidbody2D _playerRigidbody = default;  // プレイヤーのRigidbody2D
    private SEScript _objectSE = null;//SE
    private Animator _playerAnimator = default;      // プレイヤーのアニメーションコントローラー
    private bool _animeGround = true;

    // 状態管理変数
    private float _currentTimer = 0f;                // 現在のタイム
    private float _currentDashSpeed = 0f;            // 現在のダッシュ速度
    private int _currentJumpCount = 0;               // 現在のジャンプ回数
    private const int JUMP_LIMIT = 3;                // 最大ジャンプ回数

    // 入力アクション
    private InputAction _moveAction = default;       // 移動アクション
    private InputAction _jumpAction = default;       // ジャンプアクション
    private InputAction _bulletAction = default;     // 弾丸アクション
    private InputAction _swingAction = default;      // 特殊攻撃アクション

    // プレイヤーの現在の状態
    private PlayerState _currentState = PlayerState.Idle; // プレイヤーの状態を管理する

    
    
   
    #endregion

    #region プロパティ
    public int JumpCount {
        set {
            _currentJumpCount = value;
        }
        get {
            return _currentJumpCount;
        }
    }
    public float DashSpeedNow {
        set {
            _currentDashSpeed = value;
        }
        get {
            return _currentDashSpeed;
        }
    }// 横に移動する速度
    #endregion
    #region メソッド
    /// <summary>
    /// スクリプトの初期化処理を行います。
    /// 必要なコンポーネントや変数の設定を行います。
    /// </summary>
    private void Start() {
        InitializeInputActions(); // 入力アクションの初期化
        InitializeComponents();  // コンポーネントの初期化
        UpdateTimerUI();         // スコアUIの初期化
    }

    /// <summary>
    /// 入力アクションを初期化します。
    /// PlayerInputコンポーネントからアクションを取得します。
    /// </summary>
    private void InitializeInputActions() {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        _moveAction = playerInput.actions["Move"];
        _jumpAction = playerInput.actions["Jump"];
        _bulletAction = playerInput.actions["Bullet"];
        _swingAction = playerInput.actions["Swing"];
    }

    /// <summary>
    /// RigidbodyやAnimatorなどのコンポーネントを取得して初期化します。
    /// </summary>
    private void InitializeComponents() {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        //SEScriptの取得
        _objectSE = GameObject.FindWithTag("SE").GetComponent<SEScript>();
        _currentDashSpeed = _primaryDashSpeed;
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
    /// ゲームロジックや入力処理、スコア更新を実装します。
    /// </summary>
    private void Update() {
        HandleTimer();    // スコアの管理
        HandleMovement(); // プレイヤーの移動処理
        HandleActions();  // プレイヤーのアクション処理
    }

    /// <summary>
    /// 毎フレームのスコア加算処理とUIの更新を行います。
    /// </summary>
    private void HandleTimer() {
        _currentTimer += Time.deltaTime; // 時間に応じてスコアを加算
        if (_currentTimer >= 1 && _currentState != PlayerState.Dead) {
            _currentTimer -= 1;
            _keepTimer += 1;
        }
        UpdateTimerUI();                 // スコアUIを更新
    }

    /// <summary>
    /// スコアUIを更新します。
    /// </summary>
    private void UpdateTimerUI() {
        _timerText.text = $"{Mathf.FloorToInt(_keepTimer):000000}"; // スコアをフォーマットして表示
    }

    /// <summary>
    /// プレイヤーの移動とジャンプの入力を処理します。
    /// </summary>
    private void HandleMovement() {
        // 入力値を取得
        Vector2 inputMoveAxis = _moveAction.ReadValue<Vector2>();
        //着地判定とジャンプ回数の制限
        if (_currentJumpCount == 0 && _animeGround == false) {
            _animeGround = true;
            _playerAnimator.SetBool("isGround", _animeGround);
        } else if (_currentJumpCount > 0 && _animeGround == true) {
            _animeGround = false;
            _playerAnimator.SetBool("isGround", _animeGround);
        }

        // ダッシュ速度の範囲を制限
        _currentDashSpeed = Mathf.Clamp(_currentDashSpeed, DASH_SPEED_MIN, DASH_SPEED_MAX);
        
        // ジャンプ処理
        if (_currentState == PlayerState.Idle && _jumpAction.WasPressedThisFrame() && _currentJumpCount < JUMP_LIMIT) {
            Jump();
        }
    }

    /// <summary>
    /// プレイヤーのジャンプ処理を実行します。
    /// </summary>
    private void Jump() {
        _currentJumpCount++; // ジャンプ回数を加算
        _animeGround = false;
        _objectSE.JumpSE();
        _playerAnimator.SetBool("isGround", _animeGround);
        _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, 0);
        _playerRigidbody.AddForce(transform.up * _defaltJumpPower);// 上方向に力を加える
    }

    /// <summary>
    /// プレイヤーの攻撃や特殊アクションの入力を処理します。
    /// </summary>
    private void HandleActions() {
        if (_currentState == PlayerState.Idle && _bulletAction.WasPressedThisFrame()) {
            StartAttack(); // 通常攻撃を開始
        } else if (_currentState == PlayerState.Idle && _swingAction.WasPressedThisFrame()) {
            StartSpecialAttack(); // 特殊攻撃を開始
        }
    }

    /// <summary>
    /// 通常攻撃を開始します。
    /// </summary>
    private void StartAttack() {
        _currentState = PlayerState.Attacking; // 状態をAttackingに設定
        _playerAnimator.SetBool("attack", true); // アニメーションをトリガー
        
    }

    /// <summary>
    /// 特殊攻撃を開始します。
    /// </summary>
    private void StartSpecialAttack() {
        _currentState = PlayerState.SpecialAttacking; // 状態をSpecialAttackingに設定
        _playerAnimator.SetBool("attackAnother", true); // アニメーションをトリガー
        _playerRigidbody.AddForce(Vector2.up * _defaltJumpPower * _specialJumpPowerChange); // 特殊ジャンプを実行
    }

    /// <summary>
    /// 固定フレームごとに物理演算関連の更新処理を行います。
    /// プレイヤーが死亡状態でない場合、横移動の速度を設定します。
    /// </summary>
    private void FixedUpdate() {
        if (_currentState != PlayerState.Dead) {
            // Rigidbody2D に横移動の速度を設定（現在のダッシュ速度, 現在の縦方向速度）
            _playerRigidbody.velocity = new Vector2(_currentDashSpeed, _playerRigidbody.velocity.y);
        }
    }


    /// <summary>
    /// プレイヤーが他のオブジェクトと衝突した際の処理。
    /// </summary>
    /// <param name="collision">衝突したオブジェクトの情報。</param>
    private void OnTriggerEnter2D(Collider2D collision) {
        //敵の攻撃および範囲外への移動による死亡
        if ((collision.gameObject.CompareTag("Dead") || collision.gameObject.CompareTag("Sunder")) &&
            _currentState != PlayerState.Dead) {
            _currentState = PlayerState.Dead;//死亡状態に変更
            transform.Find("BGM").gameObject.SetActive(false);//BGMの停止
            _objectSE.ImpactSE();
            _objectSE.DeadSE(); 
            _playerAnimator.SetBool("dead", true);//アニメーターで死亡状態に変更
            GameObject.FindWithTag("Carten").GetComponent<FadeManager>()._keepGameTimer = (int)_keepTimer;//プレイタイムの保存
            _sceneChangeManager.GetComponent<SceneChange>().ChangeScene(_gameOverScene);//シーン遷移　ゲームオーバー
        }
        //毒の罠による死亡
        if (collision.gameObject.CompareTag("Poison") && _currentState != PlayerState.Dead) {
            _currentState = PlayerState.Dead;//死亡状態に変更
            transform.Find("BGM").gameObject.SetActive(false);//BGMの停止
            _objectSE.PoisonSE();          
            _playerAnimator.SetBool("dead", true);//アニメーターで死亡状態に変更
            GameObject.FindWithTag("Carten").GetComponent<FadeManager>()._keepGameTimer = (int)_keepTimer;//プレイタイムの保存
            _sceneChangeManager.GetComponent<SceneChange>().ChangeScene(_gameOverScene);//シーン遷移　ゲームオーバー
        }
    }
    /// <summary>
    /// ゲームをクリアした際の処理。
    /// </summary>
    public void ClearChange() {
        GameObject.FindWithTag("Carten").GetComponent<FadeManager>()._keepGameTimer = (int)_keepTimer;
        _sceneChangeManager.GetComponent<SceneChange>().ChangeScene(_clearScene);
    }
    /// <summary>
    /// 弾丸攻撃アニメーターフラグ用
    /// </summary>
    private void AnimeAttackFalse() {

        _playerAnimator.SetBool("attack", false);

    }
    /// <summary>
    /// 弾丸攻撃発射
    /// </summary>
    private void AttackBullet() {
        _currentState = PlayerState.Idle;
        _objectSE.BulletSE();
        //オブジェクトプールのLaunch関数呼び出し
        _objectPool.Launch(transform.position + new Vector3(1, 0, 0));
    }
    /// <summary>
    /// 近接攻撃攻撃アニメーターフラグ
    /// </summary>
    private void AnimeAttackAnotherFalse() {

        _playerAnimator.SetBool("attackAnother", false);
    }
    /// <summary>
    /// 近接攻撃攻撃出現
    /// </summary>
    private void ClawAttack() {
        _objectSE.SwingSE();
        _currentState = PlayerState.Idle;
        _clawSlashEffect.SetActive(true);
    }
    #endregion
}

/// <summary>
/// プレイヤーの状態を表す列挙型。
/// </summary>
public enum PlayerState {
    Idle,             // 待機中
    Attacking,        // 通常攻撃中
    SpecialAttacking, // 特殊攻撃中
    Dead              // 死亡状態
}
