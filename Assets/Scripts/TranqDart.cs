using UnityEngine;

public class TranqDart : MonoBehaviour
{
    public float flightTime = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        // handle if hitting the bear
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("<color=red>The bear was tranquilized!</color>");
            // TODO: Actual game logic
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // handle if hitting the bear
        if (collision.collider.gameObject.tag == "Player")
        {
            Debug.Log("<color=red>The bear was tranquilized!</color>");
            TelemetryLogger tm = GameObject.Find("TelemetryLogger").GetComponent<TelemetryLogger>();
            //tm = tmObj.GetComponent<TelemetryLogger>();
            tm.LogEvent("Player lost");
            GameObject.Find("PauseMenu").GetComponent<PauseMenuManager>().DisplayGameover();
        }
    }

    private void Update()
    {
        flightTime -= Time.deltaTime;
        if (flightTime < 0) Destroy(gameObject);
    }
}
