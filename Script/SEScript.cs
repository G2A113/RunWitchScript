using UnityEngine;
/// <summary>
/// SEの格納
/// </summary>
public class SEScript : MonoBehaviour {
    // AudioSourceコンポーネントの参照
    private AudioSource _audioSourceSE;

    // SE (効果音) 用のAudioClip配列
    [Header("SE")]
    [SerializeField] private AudioClip[] _audioClip = new AudioClip[5];

    // SEを再生するためのデリゲート型seの定義
    public delegate void se();

    // 空中攻撃の効果音を格納する配列
    public se[] _airAttackSE = new se[3];

    // 初期化時の処理
    void Start() {
        // AudioSourceコンポーネントを取得
        _audioSourceSE = GetComponent<AudioSource>();
    }

    /// <summary>
    /// ジャンプ時の効果音を再生する処理
    /// </summary>
    public void JumpSE() {
        _audioSourceSE.PlayOneShot(_audioClip[0]);
    }

    /// <summary>
    /// 弾の発射時の効果音を再生する処理
    /// </summary>
    public void BulletSE() {
        _audioSourceSE.PlayOneShot(_audioClip[1]);
    }

    /// <summary>
    /// 杖を振った時の効果音を再生する処理
    /// </summary>
    public void SwingSE() {
        _audioSourceSE.PlayOneShot(_audioClip[2]);
    }

    /// <summary>
    /// 死亡時の効果音を再生する処理
    /// </summary>
    public void DeadSE() {
        _audioSourceSE.PlayOneShot(_audioClip[3]);
    }

    /// <summary>
    /// 罠時の効果音を再生する処理
    /// </summary>
    public void PoisonSE() {
        _audioSourceSE.PlayOneShot(_audioClip[4]);
    }

    /// <summary>
    /// 爆発時の効果音を再生する処理
    /// </summary>
    public void BombSE() {
        _audioSourceSE.PlayOneShot(_audioClip[5]);
    }

    /// <summary>
    /// 壁破壊時の効果音を再生する処理
    /// </summary>
    public void WallBreakSE() {
        _audioSourceSE.PlayOneShot(_audioClip[6]);
    }

    /// <summary>
    /// シールド破壊時の効果音を再生する処理
    /// </summary>
    public void ShieldBreakSE() {
        _audioSourceSE.PlayOneShot(_audioClip[7]);
    }

    /// <summary>
    /// エネミー　アイ攻撃時の効果音を再生する処理
    /// </summary>
    public void SunderSE() {
        _audioSourceSE.PlayOneShot(_audioClip[8]);
    }

    /// <summary>
    /// エネミー　ゴースト攻撃時の効果音を再生する処理
    /// </summary>
    public void GhostSE() {
        _audioSourceSE.PlayOneShot(_audioClip[9]);
    }

    /// <summary>
    /// プレイヤー被弾時時の効果音を再生する処理
    /// </summary>
    public void ImpactSE() {
        _audioSourceSE.PlayOneShot(_audioClip[10]);
    }

    /// <summary>
    /// エネミーの攻撃無効時の効果音を再生する処理
    /// </summary>
    public void CounterSE() {
        _audioSourceSE.PlayOneShot(_audioClip[11]);
    }
}
