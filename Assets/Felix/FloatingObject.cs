using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    private RectTransform rectTransform;

    public bool isUi = false;
    [Space]
    public bool isPlaying = false;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private float load = 0f;

    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    private void Start()
    {
        if (isUi)
        {
            rectTransform = GetComponent<RectTransform>();
            posOffset = rectTransform.anchoredPosition;
        }
        else
        {
            posOffset = transform.localPosition;

            load = Random.Range(0f, 5f);
        }
    }

    void Update()
    {
        if (isPlaying)
        {
            load += Time.deltaTime;

            tempPos = posOffset;
            tempPos.y += Mathf.Sin(load * Mathf.PI * frequency) * amplitude;

            if (isUi)
                rectTransform.anchoredPosition = tempPos;
            else
                transform.localPosition = tempPos;
        }
    }
}
