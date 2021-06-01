using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StepScriptTuto : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float durationPop = 1.5f;
    public bool interactable;
    public AnimationCurve popUpCurve;
    // Start is called before the first frame update
    void Awake()
    {
        transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(PopUp());
    }

    IEnumerator PopUp()
    {
        Vector3 startScale = new Vector3(0, 0, 0);
        Vector3 endScale = new Vector3(1,1,1);
        float elapsed = 0.0f;
        float ratio = 0.0f;
        while (elapsed < durationPop)
        {
            ratio = elapsed / durationPop;
            ratio = popUpCurve.Evaluate(ratio);
            transform.localScale = Vector3.Lerp(startScale, endScale, ratio);
            elapsed += Time.deltaTime;
            yield return null;
        }
        interactable = true;
    }
    IEnumerator PopUpReverse()
    {
        Vector3 endScale = new Vector3(0, 0, 0);
        Vector3 startScale = new Vector3(1, 1, 1);
        float elapsed = 0.0f;
        float ratio = 0.0f;
        while (elapsed < durationPop)
        {
            ratio = elapsed / durationPop;
            ratio = popUpCurve.Evaluate(ratio);
            transform.localScale = Vector3.Lerp(startScale, endScale, ratio);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (interactable)
        {
            TutoCombat.tutoCombat.GoNextStepTuto();
            StartCoroutine(PopUpReverse());
            interactable = false;
        }

    }
}
