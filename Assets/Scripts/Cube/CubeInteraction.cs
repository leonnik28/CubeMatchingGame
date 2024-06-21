using UnityEngine;

public class CubeInteraction : MonoBehaviour
{
    [SerializeField] private float _raycastDistance = 5f;

    private GameObject _selectedCube;
    private bool _isHoldingCube;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (_isHoldingCube)
            {
                ReleaseCube();
            }
            else
            {
                PickUpCube();
            }
        }
    }

    private void ReleaseCube()
    {
        _selectedCube.GetComponent<Rigidbody>().isKinematic = false;
        _selectedCube.transform.SetParent(null);
        _selectedCube = null;
        _isHoldingCube = false;
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
                    _selectedCube.GetComponent<Rigidbody>().isKinematic = true;
                    _selectedCube.transform.SetParent(transform);
                    _isHoldingCube = true;
                }
            }
        }
    }
}
