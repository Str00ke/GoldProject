using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorEffectsScript : MonoBehaviour
{
    public float durationEffect;
    public Vector3 endScaleCursor;
    public Vector3 startScaleCursor;
    public Color startAlpha;
    // Start is called before the first frame update
    void Start()
    {
        startScaleCursor = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateCursor(GameObject cursor)
    {
        cursor.SetActive(true);
        //Debug.Log("ActiavetCursor");
        StartCoroutine(ActivateCursorCor(cursor));
    }

    public IEnumerator ActivateCursorCor(GameObject cursor)
    {
        Color startValue = new Color(cursor.GetComponent<Image>().color.r, cursor.GetComponent<Image>().color.g, cursor.GetComponent<Image>().color.b, 0);
        Color endValue = new Color(cursor.GetComponent<Image>().color.r, cursor.GetComponent<Image>().color.g, cursor.GetComponent<Image>().color.b, 1); ;
        float elapsed = 0.0f;
        float ratio = 0.0f;
        while (elapsed < durationEffect)
        {
            ratio = elapsed / durationEffect;
            cursor.GetComponent<Image>().color = Color.Lerp(startValue, endValue, ratio);
            cursor.transform.localScale = Vector3.Lerp(startScaleCursor, endScaleCursor, ratio);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    public void DeactivateCursor(GameObject cursor)
    {
        StartCoroutine(DeactivateCursorCor(cursor));
    }

    public IEnumerator DeactivateCursorCor(GameObject cursor)
    {
        startAlpha = new Color(cursor.GetComponent<Image>().color.r, cursor.GetComponent<Image>().color.g, cursor.GetComponent<Image>().color.b, 1);
        Color endValue = new Color(cursor.GetComponent<Image>().color.r, cursor.GetComponent<Image>().color.g, cursor.GetComponent<Image>().color.b, 0); ;
        float elapsed = 0.0f;
        float ratio = 0.0f;
        while (elapsed < durationEffect)
        {
            ratio = elapsed / durationEffect;
            cursor.GetComponent<Image>().color =Color.Lerp(startAlpha, endValue, ratio);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cursor.SetActive(false);
    }
}
