/// <summary>
/// エネミーなどの破壊エフェクト
/// アニメーターからエフェクトを消す
/// </summary>
using UnityEngine;

public class AttackEnd : MonoBehaviour
{
    /// <summary>
    /// エフェクトを消す
    /// </summary>
    private void AnimeAttackEnd() {
        gameObject.SetActive(false);
    }
 
}   

