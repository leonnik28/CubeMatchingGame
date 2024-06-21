using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Action OnCubePlaced;
    public Cube Cube => _cube;

    private bool _isFree = true;
    private Cube _cube;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cube cube) && _isFree)
        {
            if (cube.GetComponent<Rigidbody>().isKinematic == false)
            {
                _isFree = false;
                _cube = cube;
                OnCubePlaced?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Cube _) && !_isFree)
        {
            _isFree = true;
            _cube = null;
        }
    }
}
