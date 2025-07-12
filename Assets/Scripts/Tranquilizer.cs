using UnityEngine;

public class Tranquilizer : MonoBehaviour
{
    [SerializeField] private GameObject _dartPrefab;
    [SerializeField] private float _dartSpeed;

    public void Fire(Vector3 tranquilizerDestination)
    {
        GameObject firedDart = GameObject.Instantiate(_dartPrefab, transform.position, transform.rotation);
        firedDart.GetComponent<Rigidbody>().linearVelocity = (tranquilizerDestination - transform.position) * _dartSpeed;
    }
}
