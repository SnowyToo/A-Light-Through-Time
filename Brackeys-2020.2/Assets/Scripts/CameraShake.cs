using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool shaking;

    public void Shake(float dur, float mag)
    {
        StartCoroutine(_CameraShake(dur, mag));
    }

    public IEnumerator _CameraShake(float duration, float magnitude)
    {
        if (shaking) yield break;

        shaking = true;

        magnitude = magnitude * PlayerData.options.cameraShake;

        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = originalPos + new Vector3(x, y, 0);

            yield return null;
            elapsed += Time.deltaTime;
        }

        transform.position = originalPos;

        shaking = false;
    }
}
