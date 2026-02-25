using UnityEngine;

public class LensCameraLogic : MonoBehaviour
{
    public Transform playerCamera;
    public Transform lensCenter;

    // Update is called once per frame
    void LateUpdate()
    {
        // Position the lens camera at the center of the lens and rotate it to match the player's view direction
        transform.position = lensCenter.position;
        Vector3 viewDirection = (lensCenter.position - playerCamera.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(viewDirection);
        
        // Without this, when the lens is tilted, the picture seen through the lens would also tilt, which can be disorienting.
        // This keeps the image upright relative to the player's view.
        float lensRoll = lensCenter.eulerAngles.z;
        float flip = (Vector3.Dot(lensCenter.forward, viewDirection) > 0) ? 1f : -1f;
        transform.rotation =targetRotation * Quaternion.Euler(0, 0, lensRoll * flip);

        // Alternatively, we can directly calculate the angle offset between the lens's up vector and the player's view direction, 
        // and rotate the camera accordingly to keep the image upright.
        // This should fix the issue of compensation going the wrong way when looking from the opposite side of the lens.

        // float angleOffset = Vector3.SignedAngle(transform.up, lensCenter.up, viewDirection);
        // transform.Rotate(Vector3.forward, -angleOffset, Space.Self);
        
        // This didn't work so I modified the first approach to include a flip factor based on the dot product
        // between the lens's forward vector and the view direction, which should correctly handle the compensation
        // regardless of the viewing angle.
    }
}
