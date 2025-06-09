using System.Collections;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
    public AnimationCurve curve;
    public Transform cameraTransform;
    public float shakeDuration = 0.5f;
    Vector3 startPosition;
    void Start()
    {
        Vector3 startPosition = cameraTransform.position;
    }

    // Update is called once per frame
    IEnumerator chickenHitShaking()
    {
        if (cameraTransform != null)
        {
            float elapsedTime = 0f;

            while (elapsedTime < shakeDuration)
            {
                elapsedTime += Time.deltaTime;
                float strength = curve.Evaluate(elapsedTime / shakeDuration);
                cameraTransform.position = startPosition + Random.insideUnitSphere;
                yield return null;
            }

            cameraTransform.position = startPosition;
            StopCameraShakingAfterDelay();
        }
    }

    public void StartCameraShaking()
    {
        StartCoroutine(chickenHitShaking());
    }

    public void StopCameraShakingAfterDelay()
    {
        StartCoroutine(DelayedCall(0.5f, StopCameraShaking));
    }

    public void StopCameraShaking()
    {
        StopCoroutine(chickenHitShaking());
    }

    private IEnumerator DelayedCall(float delaySeconds, System.Action methodToCall)
    {
        yield return new WaitForSeconds(delaySeconds);
        methodToCall?.Invoke();
    }
}
