using Mirror;
using UnityEngine;

public class CubeInteraction : NetworkBehaviour
{
    [SerializeField] private float _raycastDistance = 5f;

    private GameObject _selectedCube;
    private bool _isHoldingCube;

    private void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (_isHoldingCube)
                {
                    CmdReleaseCube();
                }
                else
                {
                    PickUpCube();
                }
            }
        }
    }

    private void PickUpCube()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance))
        {
            if (hit.transform.TryGetComponent(out ICube cube))
            {
                if (!cube.IsStatic)
                {
                    _selectedCube = hit.collider.gameObject;
                    CmdPickUpCube(_selectedCube.GetComponent<NetworkIdentity>().netId);
                }
            }
        }
    }

    [Command]
    private void CmdPickUpCube(uint cubeNetId)
    {
        if (!NetworkServer.spawned.ContainsKey(cubeNetId))
        {
            Debug.LogError("Cube with id " + cubeNetId + " not found");
            return;
        }

        GameObject cube = NetworkServer.spawned[cubeNetId].gameObject;
        cube.GetComponent<Rigidbody>().isKinematic = true;
        cube.transform.SetParent(transform);
        _isHoldingCube = true;
    }


    [Command]
    private void CmdReleaseCube()
    {
        _selectedCube.GetComponent<Rigidbody>().isKinematic = false;
        _selectedCube.transform.SetParent(null);
        _selectedCube = null;
        _isHoldingCube = false;
    }
}
