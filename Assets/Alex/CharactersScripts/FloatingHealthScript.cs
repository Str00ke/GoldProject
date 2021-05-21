using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingHealthScript : MonoBehaviour
{
    public float destroyDelay;
    public float offsetX;
    public AnimationCurve animText;
    public Transform endPoint;
    public float startY;
    public float startAlpha;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        offsetX = Random.Range(-offsetX, offsetX);
        startAlpha /= 255;
        startY = this.transform.position.y;
    }


    public IEnumerator AnimateFloatingTextCor(float duration)
    {
        float elapsed = 0.0f;
        float posY = 0;
        float alpha = startAlpha;
        float ratio = 0.0f;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            ratio = animText.Evaluate(ratio);
            alpha = Mathf.Lerp(startAlpha, 0, ratio);
            posY = Mathf.Lerp(startY, endPoint.transform.position.y, ratio);
            this.transform.position = new Vector3(this.transform.position.x, posY, this.transform.position.z);
            GetComponent<TextMesh>().color = new Color(GetComponent<TextMesh>().color.r, GetComponent<TextMesh>().color.g, GetComponent<TextMesh>().color.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(parent, destroyDelay);
    }
}
