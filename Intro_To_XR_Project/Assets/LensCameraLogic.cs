using UnityEngine;

public class LensCameraLogic : MonoBehaviour
{
    public Transform playerCamera;
    public Transform lensCenter;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 viewDirection = (lensCenter.position - playerCamera.position).normalized;
        transform.rotation = Quaternion.LookRotation(viewDirection);
        transform.position = lensCenter.position;
    }
}
