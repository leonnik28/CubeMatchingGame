using Mirror;
using UnityEngine;

public class BoardSpawner : NetworkBehaviour
{
    public Vector2 CellSize => _cellSize;
    public Vector2Int CellCount => new Vector2Int(_countX, _countY);

    [SerializeField] private Vector2 _cellSize;
    [SerializeField] private GameObject _cellObject;

    [SerializeField] private int _countX;
    [SerializeField] private int _countY;

    private Cell[,] _cells;

    public Cell GetCell(int x, int y)
    {
        return NetworkServer.spawned[_cells[x, y].GetComponent<NetworkIdentity>().netId].GetComponent<Cell>();
    }

    public void CreateBoard(NetworkIdentity parent)
    {
        _cells = new Cell[_countX, _countY];

        for (int x = 0; x < _countX; x++)
        {
            for (int y = 0; y < _countY; y++)
            {
                float posX = x * _cellSize.x - (_countX * _cellSize.x / 2) + _cellSize.x / 2;
                float posY = 0.01f;
                float posZ = y * _cellSize.y - (_countY * _cellSize.y / 2) + _cellSize.y / 2;

                GameObject cellObject = Instantiate(_cellObject, new Vector3(posX, posY, posZ), Quaternion.identity);
                cellObject.transform.SetParent(parent.transform, false);
                NetworkServer.Spawn(cellObject, parent.connectionToClient);
                _cells[x, y] = cellObject.GetComponent<Cell>();
            }
        }
    }
}
