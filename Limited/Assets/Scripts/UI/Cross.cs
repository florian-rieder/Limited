using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour
{
    public void MoveTo(Vector3 destination)
    {
        Vector3 adjustedDestination = new Vector3(destination.x + 0.5f, destination.y + 0.5f, destination.z);
        transform.position = adjustedDestination;
    }
}
