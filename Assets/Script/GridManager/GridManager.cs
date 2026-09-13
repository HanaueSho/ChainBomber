/*
    GridManager
    20260906  hanaue sho
    盤面を管理するマネージャー
    添え字からの逆引きや当たり判定など
*/
using UnityEngine;

public enum PieceMoveDirection
{
    Right,
    Left,
    Up, 
    Down,
}

public class GridManager : MonoBehaviour
{
    // ピースの管理
    private PieceStateManager[,] _pieces;

    // ==================================================
    // ----- Grid Propaty -----
    // ==================================================
    const int _gridRows = 14; // 行
    const int _gridColumns  = 6; // 列
    private Vector3 _gridOrigin = new Vector3(0.0f, 0.2f, 0.0f); // グリッドの中心座標
    [SerializeField] private float _cellSize = 1.0f; // グリッド一つのサイズ


    // ==================================================
    // ----- Unity Event -----
    // ==================================================
    private void Awake()
    {
        // マップの生成
        _pieces = new PieceStateManager[_gridColumns, _gridRows];

    }


    // ==================================================
    // ----- Public Events -----
    // ==================================================
    public bool CanMoveTo(PieceMoveDirection direction, Vector2Int currentGridPosition)
    {
        // 向き判定
        Vector2Int dir = new Vector2Int(0, 0);
        switch(direction)
        {
            case PieceMoveDirection.Right:
                dir = new Vector2Int( 1, 0);
                break;
            case PieceMoveDirection.Left:
                dir = new Vector2Int(-1, 0);
                break;
            case PieceMoveDirection.Up:
                dir = new Vector2Int(0, 1);
                break;
            case PieceMoveDirection.Down:
                dir = new Vector2Int(0, -1);
                break;
        }
        Vector2Int moveTo = currentGridPosition + dir;
        if (moveTo.x > _gridColumns - 1 || moveTo.x < 0 || moveTo.y > _gridRows - 1 || moveTo.y < 0)
        {
            return false;
        }
        if (_pieces[moveTo.x, moveTo.y] == null)
        {
            return true;
        }
        return false;
    }
    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        float centerX = (_gridColumns - 1) * 0.5f;
        float centerY = (_gridRows - 1) * 0.5f;

        float x = (gridPosition.x - centerX) * _cellSize;
        float y = (gridPosition.y - centerY) * _cellSize;

        return _gridOrigin + new Vector3(x, y, 0.0f);
    }

    public void RegisterPiece(PieceStateManager piece)
    {
        int x = piece.GridPosition.x;
        int y = piece.GridPosition.y;

        if (_pieces[x, y] != null)
        {
            Debug.LogError("[Error] PIECE is already exist!");
        }

        _pieces[x, y] = piece;

    }

}
