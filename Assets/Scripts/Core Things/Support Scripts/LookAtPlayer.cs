using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Camera targetCamera;

    void Awake()
    {
        targetCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (!targetCamera) return;

        Vector3 direction = targetCamera.transform.position - transform.position;

        transform.rotation = Quaternion.LookRotation(-direction);
    }
}
