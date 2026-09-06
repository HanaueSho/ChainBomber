/*
    PieceStateManager
    20260906  hanaue sho
    ピースのステートマネージャー
*/
using System.Collections;
using UnityEngine;

public class PieceStateManager : MonoBehaviour
{
    // ステート　落下　待機　爆発待機　爆発
    // 連鎖数
    // 移動処理
    // PM


    // ==================================================
    // ----- Propaty -----
    // ==================================================
    private PieceManager _pieceManager;

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    public PieceManager PieceManager { get; set; }


    // ==================================================
    // ----- State -----
    // ==================================================
    public enum PieceState
    {
        None,
        Drop, // 落下中
        Stay, // 待ち
    }
    [SerializeField] private PieceState _state = PieceState.Drop;

    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void Update()
    {
        switch (_state)
        {
            case PieceState.None:
                break;
            case PieceState.Drop:
                Drop();
                break;
            case PieceState.Stay:
                break;
        }
    }

    // ==================================================
    // ----- Public Event -----
    // ==================================================
    public void TryMove(float moveXDirection)
    {
        StartCoroutine(Move(moveXDirection));
    }
    public void TryHardDrop()
    {

    }
    public void TryQuickDrop()
    {

    }

    // ==================================================
    // ----- Drop -----
    // ==================================================
    private void Drop()
    {
        // 落下処理
        Vector2 position = transform.position;
        position.y += Time.deltaTime * -2.0f;
        transform.position = position;
    }

    // ==================================================
    // ----- Move -----
    // ==================================================
    private IEnumerator Move(float moveXDirection)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime <= 0.5f)
        {
            // 時間経過
            elapsedTime += Time.deltaTime;

            // 左右移動
            Vector2 position = transform.position;
            position.x += Time.deltaTime * 1.0f * moveXDirection;
            transform.position = position;

            yield return null;
        }

        yield break;
    }


}
