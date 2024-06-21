using UnityEngine;

public class Cube : MonoBehaviour, ICube
{
    public bool IsStatic => _isStatic;

    [SerializeField] private bool _isStatic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cell cell) && GetComponent<Rigidbody>().isKinematic == false)
        {
            Vector3 position = new Vector3(cell.transform.position.x, transform.position.y, cell.transform.position.z);
            transform.position = position;
            transform.rotation = cell.transform.rotation;
        }
    }
}
