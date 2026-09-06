/*
    PieceManager
    20260906  hanaue sho
    ピースを管理するマネージャー
    主にピース周りの制御をおこなう
*/
using UnityEngine;
using UnityEngine.InputSystem;

public class PieceManager : MonoBehaviour
{
    // ピースの生成
    // ピースの操作
    // ピースの連鎖チェック
    // ピースの爆発チェック
    // ピースのデッドラインチェック

    // ==================================================
    // ----- Propaty -----
    // ==================================================
    [Header("ピースのプレファブ")]
    [SerializeField] private GameObject _piecePrefab;
    [Header("InputAction")]
    [SerializeField] private InputAction_Piece _inputAction;

    [Header("操作対象のピース参照")]
    [SerializeField] private GameObject _currentPiece;


    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    private void OnEnable()
    {
        _inputAction = new InputAction_Piece();
        _inputAction.Enable();
    }
    private void Start()
    {
        CreatePiece();

    }
    private void Update()
    {
        InputAction();

    }

    // ==================================================
    // ----- Input Action -----
    // ==================================================
    private void InputAction()
    {
        if (_inputAction.Piece.MoveLeft.WasPressedThisFrame())
        {
            Debug.Log("左移動");
            _currentPiece.GetComponent<PieceStateManager>().TryMove(-1.0f);
        }
        if (_inputAction.Piece.MoveRight.WasPressedThisFrame())
        {
            Debug.Log("右移動");
            _currentPiece.GetComponent<PieceStateManager>().TryMove(1.0f);
        }
        if (_inputAction.Piece.HardDrop.WasPressedThisFrame())
        {
            Debug.Log("ハード");
        }
        if (_inputAction.Piece.QuickDrop.IsPressed())
        {
            Debug.Log("クイック");
        }

    }

    // ==================================================
    // ----- Create Piece -----
    // ==================================================
    private void CreatePiece()
    {
        GameObject clone = Instantiate(_piecePrefab);
        _currentPiece = clone;

    }

}
