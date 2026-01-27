using UnityEngine;

// This script makes this GameObject spin around the Y axis, making the children orbit around it.
public class Orbitting : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 5 * Time.deltaTime, 0);
    }
}
