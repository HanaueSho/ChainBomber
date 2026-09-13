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
    [SerializeField] private PieceStateManager _currentPieceUpper; // 上側
    [SerializeField] private PieceStateManager _currentPieceLower; // 下側

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
    // ----- LockDelay Propaty -----
    // ==================================================
    [SerializeField] private float _lockDelayTime = 0.5f;
    [SerializeField] private float _lockDelayTimeQuick = 0.1f;
    private float _lockElapsedTime = 0.0f;
    private bool _isGrounded = false;

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
        // ----- 入力受けつけ -----
        UpdateQuickDrop();
        InputAction();

        // ----- 自由落下 -----
        UpdateDropPieceTime();

        // ----- 着地固定判定 -----
        UpdateLockDelay();
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
        if (_gridManager.CanMoveTo(PieceMoveDirection.Down, _currentPieceLower.GridPosition))
        {
            MoveCurrentPieces(Vector2Int.down);
            _isGrounded = false;
            _lockElapsedTime = 0.0f;
        }
        else
        {
            _isGrounded = true;
            _dropElapsedTime = 0.0f;
        }
    }

    // ==================================================
    // ----- Lock Piece -----
    // 設置時にピースを固定する猶予
    // ==================================================
    private void UpdateLockDelay()
    {
        if (!_isGrounded)
        {
            return;
        }

        _lockElapsedTime += Time.deltaTime;

        float currentLockDelay = _isQuickDrop ? _lockDelayTimeQuick : _lockDelayTime;

        if (_lockElapsedTime >= currentLockDelay)
        {
            LockCurrentPiece();
        }
    }
    private void LockCurrentPiece()
    {
        _gridManager.RegisterPiece(_currentPieceUpper);
        _gridManager.RegisterPiece(_currentPieceLower);

        _currentPieceUpper = null;
        _currentPieceLower = null;

        _isGrounded = false;
        _lockElapsedTime = 0.0f;
        _dropElapsedTime = 0.0f;

        CreatePiece();
    }
    private void UpdateGroundedState()
    {
        bool canDrop = _gridManager.CanMoveTo(PieceMoveDirection.Down, _currentPieceLower.GridPosition);

        if (canDrop)
        {
            _isGrounded = false;
            _lockElapsedTime = 0.0f;
        }
        else
        {
            _isGrounded = true;
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
            if (_gridManager.CanMoveTo(PieceMoveDirection.Left, _currentPieceLower.GridPosition))
            {
                MoveCurrentPieces(Vector2Int.left);
                _lockElapsedTime = 0.0f;
                UpdateGroundedState();
            }
        }
        if (_inputAction.Piece.MoveRight.WasPressedThisFrame())
        {
            Debug.Log("右移動");
            if (_gridManager.CanMoveTo(PieceMoveDirection.Right, _currentPieceLower.GridPosition))
            {
                MoveCurrentPieces(Vector2Int.right);
                _lockElapsedTime = 0.0f;
                UpdateGroundedState();
            }
        }
        if (_inputAction.Piece.HardDrop.WasPressedThisFrame())
        {
            Debug.Log("ハード");
        }
    }
    private void MoveCurrentPieces(Vector2Int direction)
    {
        Vector2Int targetGridPositionUpper = _currentPieceUpper.GridPosition + direction;
        Vector2Int targetGridPositionLower = _currentPieceLower.GridPosition + direction;
        _currentPieceUpper.MoveTo(targetGridPositionUpper, _gridManager.GridToWorld(targetGridPositionUpper));
        _currentPieceLower.MoveTo(targetGridPositionLower, _gridManager.GridToWorld(targetGridPositionLower));
    }

    // ==================================================
    // ----- Create Piece -----
    // ==================================================
    private void CreatePiece()
    {
        { // Upper
            GameObject clone = Instantiate(_piecePrefab);
            _currentPieceUpper = clone.GetComponent<PieceStateManager>();
            clone.transform.SetParent(transform);

            Vector2Int gridPos = _initPiecePosition + new Vector2Int(0, 1);
            clone.transform.position = _gridManager.GridToWorld(gridPos);
            _currentPieceUpper.GridPosition = gridPos;
        }
        { // Lower
            GameObject clone = Instantiate(_piecePrefab);
            _currentPieceLower = clone.GetComponent<PieceStateManager>();
            clone.transform.SetParent(transform);
            clone.transform.position = _gridManager.GridToWorld(_initPiecePosition);
            _currentPieceLower.GridPosition = _initPiecePosition;

        }
    }

}
