using Mirror;
using UnityEngine;

public class CubeSpawner : NetworkBehaviour
{
    public CubeData[,] Data => _cubeData;

    [SerializeField] private BoardSpawner _boardSpawner;
    [SerializeField] private Transform _spawnZone;

    [SerializeField] private GameObject _cubeObject;
    [SerializeField] private GameObject _cubeObjectNonInteraction;

    private CubeData[,] _cubeData;
    private int _count = 0;

    private void Start()
    {
        if (isServer)
        {
            NetworkIdentity networkIdentity = GetComponent<NetworkIdentity>();
            _boardSpawner.CreateBoard(networkIdentity);
            SpawnInBoard();
            SpawnOutBoard();
        }
    }


    private void SpawnInBoard()
    {
        _cubeData = new CubeData[_boardSpawner.CellCount.x, _boardSpawner.CellCount.y];

        int cubeCount = Random.Range(1, _cubeData.Length);
        while (_count != cubeCount)
        {
            Vector2Int randomIndex = new Vector2Int(Random.Range(0, _cubeData.GetLength(0)), Random.Range(0, _cubeData.GetLength(1)));

            if (_cubeData[randomIndex.x, randomIndex.y].Cube == null)
            {
                float posX = randomIndex.x * _boardSpawner.CellSize.x - (_cubeData.GetLength(0) * _boardSpawner.CellSize.x / 2) + _boardSpawner.CellSize.x / 2;
                float posY = 0.5f;
                float posZ = randomIndex.y * _boardSpawner.CellSize.y - (_cubeData.GetLength(1) * _boardSpawner.CellSize.y / 2) + _boardSpawner.CellSize.y / 2;

                GameObject cube = Instantiate(_cubeObjectNonInteraction, new Vector3(posX, posY, posZ), Quaternion.identity);
                _cubeData[randomIndex.x, randomIndex.y].Cube = cube.GetComponent<Cube>();
                NetworkServer.Spawn(cube, connectionToClient);
                _count++;
            }
        }
    }

    private void SpawnOutBoard()
    {
        int spawnedCubes = 0;

        for (int x = 0; x < _boardSpawner.CellCount.x && spawnedCubes < _count; x++)
        {
            for (int y = 0; y < _boardSpawner.CellCount.y && spawnedCubes < _count; y++)
            {
                float posX = x * _boardSpawner.CellSize.x - (_boardSpawner.CellCount.x * _boardSpawner.CellSize.x / 2) + _boardSpawner.CellSize.x / 2;
                float posY = 0.5f;
                float posZ = y * _boardSpawner.CellSize.y - (_boardSpawner.CellCount.y * _boardSpawner.CellSize.y / 2) + _boardSpawner.CellSize.y / 2;

                GameObject cube = Instantiate(_cubeObject, _spawnZone.TransformPoint(new Vector3(posX, posY, posZ)), Quaternion.identity);
                NetworkServer.Spawn(cube, connectionToClient);
                spawnedCubes++;
            }
        }
    }

    [System.Serializable]
    public struct CubeData
    {
        public Vector2 Position;
        public ICube Cube;
    }
}
