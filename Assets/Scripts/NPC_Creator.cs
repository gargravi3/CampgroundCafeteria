using UnityEngine;

public class NPC_Creator : MonoBehaviour
{
    [SerializeField] private GameObject[] _npcOptions;
    [SerializeField] private MeshRenderer _markerRender;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject newNPC = Instantiate(_npcOptions[Random.Range(0, _npcOptions.Length-1)], transform);
        _markerRender.enabled = false;
    }
}
