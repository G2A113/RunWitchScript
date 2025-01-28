using UnityEngine;
using UnityEngine.UI; // UIコンポーネントの使用
/// <summary>
/// メニュー画面でボタンの選択状態を管理するクラス
/// </summary>
public class Menu : MonoBehaviour {
    Button _cube; // ボタンの参照

    
    void Start() {
        // ボタンコンポーネントの取得
        _cube = GameObject.Find("/SelectCanvas/Button1").GetComponent<Button>();

        // 最初に選択状態にしたいボタンの設定
        _cube.Select(); // この行でボタンを選択状態にする
    }
}
