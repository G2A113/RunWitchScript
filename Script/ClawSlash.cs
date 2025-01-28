using UnityEngine;
/// <summary>
/// アニメーターから近接攻撃を消す
/// </summary>
public class ClawSlash : MonoBehaviour
{
    /// <summary>
    ///近接攻撃を消す
    /// </summary>
    private void SlashEnd() {
        gameObject.SetActive(false);
    }
}
