using UnityEngine;
using UnityEngine.SceneManagement;

public class FoodCollector : MonoBehaviour
{
    public int foodNeeded = 5;
    public int foodCollected = 0;
    public bool foodInInventory = false;
    public int foodDelivered = 0;
    private bool loggedFirst = false;
    private bool loggedDelivered = false;
    private TelemetryLogger tm;

    public void Start()
    {
        tm = GameObject.Find("TelemetryLogger").GetComponent<TelemetryLogger>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("Food") && !foodInInventory)
        {
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);

            // Increment the count of "PickUp" objects collected.
            foodCollected++;
            foodInInventory = true;

            // Record time of first pick up for playtesting
            if (loggedFirst == false)
            {
                tm.LogEvent("Got first food item");
                loggedFirst = true;
            }
        }
        else if (other.gameObject.CompareTag("Cave") && foodCollected > 0)
        {
            foodInInventory = false;
            foodDelivered++;
            foodCollected = 0;
            // Record time of first delivery for playtesting
            if (loggedDelivered == false)
            {
                tm.LogEvent("First food item delivered");
                loggedDelivered = true;
            }
            if (foodDelivered == foodNeeded)
            {
                tm.LogEvent("Player won");
                Debug.Log("Game Complete!");
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("EndGame");

            }
        }
    }
}
