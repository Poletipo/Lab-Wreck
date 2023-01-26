using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform targetTransform;
    public Vector3 offset;

    void LateUpdate()
    {
        if (targetTransform != null) {
            transform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z) + offset;
        }
    }
}
