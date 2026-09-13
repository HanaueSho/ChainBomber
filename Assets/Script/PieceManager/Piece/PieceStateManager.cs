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
    [SerializeField] private Vector2Int _gridPosition; // 現在の論理位置
    private Coroutine _moveCoroutine;

    // ==================================================
    // ----- Public Propaty -----
    // ==================================================
    public Vector2Int GridPosition { get => _gridPosition; set => _gridPosition = value; }


    // ==================================================
    // ----- State -----
    // ==================================================
    public enum PieceState
    {
        None,
        Drop, // 落下中
        Stay, // 待ち
    }
    [SerializeField] private PieceState _currentState = PieceState.Drop;

    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void Start()
    {

    }
    private void Update()
    {
        switch (_currentState)
        {
            case PieceState.None:
                break;
            case PieceState.Drop:
                break;
            case PieceState.Stay:
                break;
        }
    }

    // ==================================================
    // ----- Public Event -----
    // ==================================================
    public void MoveTo(Vector2Int targetGridPosition, Vector3 targetWorldPosition)
    {
        _gridPosition = targetGridPosition;

        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

        _moveCoroutine = StartCoroutine(MoveCoroutine(targetWorldPosition));
    }
    public void TryHardDrop()
    {

    }
    public void TryQuickDrop()
    {

    }

    // ==================================================
    // ----- Move -----
    // ==================================================
    private IEnumerator MoveCoroutine(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float duration = 0.1f;
        float elapsedTime = 0.0f;

        while (elapsedTime <= duration)
        {
            // 時間経過
            elapsedTime += Time.deltaTime;

            // 遷移
            float t = Mathf.Clamp01(elapsedTime / duration);

            // 左右移動
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }
        transform.position = targetPosition;

        _moveCoroutine = null;
    }

    // ==================================================
    // ----- ChangeState -----
    // ==================================================
    public void ChangeState(PieceState newState)
    {
        if (_currentState == newState)
        {
            return;
        }

        // 終了処理
        switch (_currentState)
        {
            case PieceState.None:
                break;
            case PieceState.Drop:
                break;
            case PieceState.Stay:
                break;
        }

        _currentState = newState;

        // 開始処理
        switch (_currentState)
        {
            case PieceState.None:
                break;
            case PieceState.Drop:
                break;
            case PieceState.Stay:
                break;
        }
    }


}
