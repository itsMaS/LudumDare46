using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    Vector2 HorizontalClamp;
    [SerializeField]
    Vector2 VerticalClamp;
    [SerializeField]
    [Range(0,1)]
    float cameraFollowSpeed;
    [SerializeField]
    Camera cam;

    Vector3 cameraTarget;
    static CameraController instance;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        initialPosition = cam.transform.position;
        instance = this;
    }
    private void LateUpdate()
    {
        cameraTarget = target.position;
        cameraTarget.x = Mathf.Clamp(cameraTarget.x, HorizontalClamp.x, HorizontalClamp.y);
        cameraTarget.y = Mathf.Clamp(cameraTarget.y, VerticalClamp.x, VerticalClamp.y);


        transform.position = Vector3.Lerp(transform.position, cameraTarget, cameraFollowSpeed);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(Mathf.Lerp(HorizontalClamp.x, HorizontalClamp.y, 0.5f),
            Mathf.Lerp(VerticalClamp.x, VerticalClamp.y, 0.5f)),
            new Vector3(Mathf.Abs(HorizontalClamp.y - HorizontalClamp.x), Mathf.Abs(VerticalClamp.y - VerticalClamp.x)));
    }

    public static void ShakeCamera(float magnitude = 0.5f,float duration = 0.1f, float smoothing = 0.8f, float damping = 0)
    {
        instance.Shake(magnitude,duration,smoothing, damping);
    }
    void Shake(float magnitude, float duration = 0.1f, float smoothing = 0.8f, float damping = 0)
    {
        shakeDamping = damping;
        shakeMagnitude = magnitude;
        if (shakeDuration < duration)
        {
            shakeDuration = duration;
        }
        this.smoothing = smoothing;
    }

    // A measure of magnitude for the shake. Tweak based on your preference
    private float shakeMagnitude = 0.7f;
    private float smoothing = 0.8f;
    private float shakeDuration = 0;
    private float shakeDamping = 0;

    // The initial position of the GameObject
    Vector3 initialPosition;
    void Update()
    {
        if (shakeDuration > 0)
        {
            cam.transform.localPosition = Vector3.Lerp(cam.transform.position,initialPosition + Random.insideUnitSphere * shakeMagnitude, smoothing+shakeDamping*shakeDuration);

            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
            smoothing = 0.8f;
            cam.transform.localPosition = Vector3.Lerp(cam.transform.position, initialPosition, smoothing);
        }
    }
}
