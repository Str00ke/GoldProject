using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public static CameraScript camScript;
    public Vector3 startTransform;

    private void Awake()
    {
        if (camScript == null)
        {
            camScript = this;
        }
        else if (camScript != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        startTransform = this.transform.position;
    }
    public void CamShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    IEnumerator Shake(float duration, float magnitude)
    {

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, -10);

            elapsed += Time.deltaTime;

            yield return null;
        }
        this.transform.position = startTransform;
    }
}
