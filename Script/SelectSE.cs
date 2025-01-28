using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// 選択の操作に反応してSEを鳴らす処理するクラス
/// </summary>
public class SelectSE : MonoBehaviour {
    // AudioSourceコンポーネントの参照
    private AudioSource _audioSourceSE;

    // ジャンプアクションの入力を扱うInputAction
    InputAction _jump;

    // SE (効果音) 用のAudioClip配列
    [Header("SE")]
    [SerializeField] private AudioClip[] _audioClip = new AudioClip[5];

    // デリゲート型seの定義（後でSEの関数をまとめて呼び出すため）
    public delegate void se();

    // 空中攻撃の効果音を格納する配列
    public se[] _airAttackSE = new se[3];

    // ジャンプが押されたかの判定フラグ
    private bool _jumpBool = false;

    // Yボタンの入力フラグ
    private bool _yBool = false;

    /// <summary>
    /// スクリプトの初期化処理を行います。
    /// 必要なコンポーネントや変数の設定を行います。
    /// </summary>
    private void Start() {
        // AudioSourceコンポーネントの参照を取得
        _audioSourceSE = GetComponent<AudioSource>();

        // PlayerInputから"Jump"アクションを取得
        _jump = gameObject.GetComponent<PlayerInput>().actions["Jump"];
    }

    /// <summary>
    /// 毎フレームの更新処理を行います。
    /// ゲームパッドの操作に反応してSEを鳴らす処理
    /// </summary>
    private void Update() {
        // ゲームパッドやキーボードで縦の入力があれば移動SEを再生
        if (Input.GetButtonDown("Vertical")) {
            SelectMoveSE();
        }

        // ゲームパッドの縦軸入力に対応
        float yAxis = Input.GetAxis("VerticalPad");

        // 縦軸の入力がある場合、移動SEを再生
        if (yAxis != 0 && !_yBool) {
            _yBool = true;
            SelectMoveSE();
        }

        // 縦軸の入力がない場合、フラグをリセット
        if (yAxis == 0 && _yBool) {
            _yBool = false;
        }

        // ジャンプボタンが押された時、ジャンプSEを再生
        if (_jump.WasPressedThisFrame() && !_jumpBool) {
            _jumpBool = true;
            EnterSE();
        }
    }

    /// <summary>
    /// ジャンプ時の効果音を再生する処理
    /// </summary>
    public void EnterSE() {
        _audioSourceSE.PlayOneShot(_audioClip[0]);
    }

    /// <summary>
    /// 移動時の効果音を再生する処理
    /// </summary>
    public void SelectMoveSE() {
        _audioSourceSE.PlayOneShot(_audioClip[1]);
    }
}
