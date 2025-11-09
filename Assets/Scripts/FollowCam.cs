using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target != null)
        {
            // Desired position based on target
            Vector3 desiredPosition = target.position + offset;

            // Smooth movement
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            transform.position = smoothedPosition;
        }
    }
}