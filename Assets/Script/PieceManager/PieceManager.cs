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
    [SerializeField] private PieceStateManager _currentPiece;

    [Header("ピースの初期位置")]
    [SerializeField] private Vector2Int _initPiecePosition = new Vector2Int(2, 9);

    // ==================================================
    // ----- Manager Propaty -----
    // ==================================================
    private GridManager _gridManager;

    // ==================================================
    // ----- DropPiece Propaty -----
    // ==================================================
    [Header("ピースの自由落下時間インターバル")]
    [SerializeField] private float _dropInterval = 1.5f;
    [SerializeField] private float _dropIntervalQuick = 0.1f;
    private float _dropElapsedTime = 0.0f;
    private bool _isQuickDrop;


    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    private void Awake()
    {
        // 取得
        _gridManager = FindAnyObjectByType<GridManager>();
    }
    private void OnEnable()
    {
        _inputAction = new InputAction_Piece();
        _inputAction.Enable();
    }
    private void Start()
    {
        // とりあえずここで生成（実際は LevelManager で管理するよ）
        CreatePiece();

    }
    private void Update()
    {
        // ----- 自由落下指示 -----
        UpdateQuickDrop();
        UpdateDropPieceTime();

        // ----- 入力受けつけ -----
        InputAction();
    }

    // ==================================================
    // ----- Drop Piece -----
    // 一定時間ごとにピースを１マス下げる
    // ==================================================
    private void UpdateQuickDrop()
    {
        bool inputQuickDrop = _inputAction.Piece.QuickDrop.IsPressed();
        // 入力した瞬間
        if (inputQuickDrop && !_isQuickDrop)
        {
            _isQuickDrop = true;
            _dropElapsedTime = 0.0f;

            TryDropPiece();
        }
        // 離した瞬間
        if (!inputQuickDrop && _isQuickDrop) 
        {
            _isQuickDrop = false;
            _dropElapsedTime = 0.0f;
        }

    }
    private void UpdateDropPieceTime()
    {
        _dropElapsedTime += Time.deltaTime;

        float currentInterval = _isQuickDrop ? _dropIntervalQuick : _dropInterval;

        if (_dropElapsedTime >= currentInterval)
        {
            _dropElapsedTime -= currentInterval;
            TryDropPiece();
        }
    }
    private void TryDropPiece()
    {
        if (_gridManager.CanMoveTo(PieceMoveDirection.Down, _currentPiece.GridPosition))
        {
            Vector2Int targetGridPosition = _currentPiece.GridPosition + new Vector2Int(0, -1);
            _currentPiece.MoveTo(targetGridPosition, _gridManager.GridToWorld(targetGridPosition));
        }
    }

    // ==================================================
    // ----- Input Action -----
    // PM => GM に問い合わせして動けるなら動かす
    // ==================================================
    private void InputAction()
    {
        if (_inputAction.Piece.MoveLeft.WasPressedThisFrame())
        {
            Debug.Log("左移動");
            if (_gridManager.CanMoveTo(PieceMoveDirection.Left, _currentPiece.GridPosition))
            {
                Vector2Int targetGridPosition = _currentPiece.GridPosition + new Vector2Int(-1, 0);
                _currentPiece.MoveTo(targetGridPosition, _gridManager.GridToWorld(targetGridPosition));
            }
        }
        if (_inputAction.Piece.MoveRight.WasPressedThisFrame())
        {
            Debug.Log("右移動");
            if (_gridManager.CanMoveTo(PieceMoveDirection.Right, _currentPiece.GridPosition))
            {
                Vector2Int targetGridPosition = _currentPiece.GridPosition + new Vector2Int(1, 0);
                _currentPiece.MoveTo(targetGridPosition, _gridManager.GridToWorld(targetGridPosition));
            }
        }
        if (_inputAction.Piece.HardDrop.WasPressedThisFrame())
        {
            Debug.Log("ハード");
        }
    }

    // ==================================================
    // ----- Create Piece -----
    // ==================================================
    private void CreatePiece()
    {
        GameObject clone = Instantiate(_piecePrefab);
        _currentPiece = clone.GetComponent<PieceStateManager>();
        clone.transform.SetParent(transform);
        clone.transform.position = _gridManager.GridToWorld(_initPiecePosition);
        _currentPiece.GridPosition = _initPiecePosition;
    }

}
