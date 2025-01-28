using UnityEngine;
using System;


/// <summary>
/// プレイヤーの進行に応じて地形を生成するクラス
/// </summary>
public class Generator : MonoBehaviour {
    [SerializeField] private GameObject[] _ground = new GameObject[3]; // 地形プレハブのリスト
    private float _generatorPositionX = 0;    // 現在の生成位置
    private int _generateLimit = 100;          // 地形生成の上限位置
    private GameObject _player;                // プレイヤーオブジェクト
    private int _randomPool;                   // ランダムに選ばれるインデックス
    private  const int GENERATE_RANGE = 50;           // 生成を追加する範囲
    private const int GENERATE_ADD = 100;            // 生成範囲を拡張する値

    void Start() {
        // プレイヤーオブジェクトを取得
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        // 必要に応じて地形を生成する処理を呼び出す
        GenerateGroundIfNeeded();
    }

    /// <summary>
    /// 必要に応じて地形を生成する処理
    /// </summary>
    private void GenerateGroundIfNeeded() {
        // ランダムシードを現在の時刻で初期化
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        // 生成位置が上限に達していない場合、地形を生成する
        while (_generatorPositionX < _generateLimit) {
            // ランダムに生成する地形を選ぶ
            _randomPool = UnityEngine.Random.Range(0, _ground.Length);

            // 安全な生成範囲チェック
            if (_randomPool >= 0 && _randomPool < _ground.Length) {
                // 地形を指定の位置に生成
                GameObject ground = Instantiate(
                    _ground[_randomPool],
                    new Vector2(transform.position.x + _generatorPositionX, 0f),
                    Quaternion.identity
                );

                // 生成した地形のサイズを取得し、生成位置を更新
                _generatorPositionX += ground.GetComponent<GroundSize>()._groundSize;
            }
        }

        // プレイヤーが進行した位置に応じて生成範囲を拡張
        if (_generateLimit - _player.transform.position.x < GENERATE_RANGE) {
            _generateLimit += GENERATE_ADD;
        }
    }
}
