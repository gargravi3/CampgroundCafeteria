using UnityEngine;

public class AlertBroadcaster : MonoBehaviour
{
    public ParkRangerAI[] parkRangers;

    public void AlertAtLocation(Vector3 location)
    {
        FindNearestRanger(location).AlertToPosition(location);
    }

    private ParkRangerAI FindNearestRanger(Vector3 alertPosition)
    {
        float minDistance = float.MaxValue;
        ParkRangerAI closestRanger = null;

        foreach (ParkRangerAI ranger in parkRangers)
        {
            float compareDistance = Vector3.Distance(alertPosition, ranger.transform.position);
            if(compareDistance < minDistance)
            {
                minDistance = compareDistance;
                closestRanger = ranger;
            }
        }

        return closestRanger;
    }
}
