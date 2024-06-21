using UnityEngine;

public class CellInteraction : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    [SerializeField] private BoardSpawner _boardSpawner;

    private void Start()
    {
        _boardSpawner.CreateBoard(transform);
        SubscribeToCells();
    }

    private void SubscribeToCells()
    {
        for (int x = 0; x < _boardSpawner.CellCount.x; x++)
        {
            for (int y = 0; y < _boardSpawner.CellCount.y; y++)
            {
                Cell cell = _boardSpawner.GetCell(x, y);
                cell.OnCubePlaced += CheckWin;
            }
        }
    }

    private void CheckWin()
    {
        if (IsWin())
        {
            Debug.Log("Win!");
        }
    }

    private bool IsWin()
    {
        for (int x = 0; x < _boardSpawner.CellCount.x; x++)
        {
            for (int y = 0; y < _boardSpawner.CellCount.y; y++)
            {
                Cell cell = _boardSpawner.GetCell(x, y);
                if (_cubeSpawner.Data[x, y].Cube != null)
                {
                    if (cell.Cube == null)
                    {
                        return false;
                    }
                }
                else
                {
                    if (cell.Cube != null)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}
