using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StepScriptTuto : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject parentCanvas;
    public GameObject backgroundTuto;
    public float initialAlpha;
    public float durationPop = 1.5f;
    public bool interactable;
    public AnimationCurve popUpCurve;
    public Text textTuto;
    private string currentText = "";
    string previousText = "";
    public float showTextDelay;
    public float hideTextDelay;
    // Start is called before the first frame update
    void Awake()
    {
        parentCanvas.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        parentCanvas.GetComponent<Canvas>().worldCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        parentCanvas.transform.SetParent(GameObject.Find("CombatSceneTuto").transform);
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
            backgroundTuto.GetComponent<Image>().color = Color.Lerp(new Color(backgroundTuto.GetComponent<Image>().color.r, backgroundTuto.GetComponent<Image>().color.g, backgroundTuto.GetComponent<Image>().color.b, 0),new Color(backgroundTuto.GetComponent<Image>().color.r, backgroundTuto.GetComponent<Image>().color.g, backgroundTuto.GetComponent<Image>().color.b, initialAlpha), ratio);
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
            backgroundTuto.GetComponent<Image>().color = Color.Lerp(new Color(backgroundTuto.GetComponent<Image>().color.r, backgroundTuto.GetComponent<Image>().color.g, backgroundTuto.GetComponent<Image>().color.b, initialAlpha), new Color(backgroundTuto.GetComponent<Image>().color.r, backgroundTuto.GetComponent<Image>().color.g, backgroundTuto.GetComponent<Image>().color.b, 0), ratio);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(parentCanvas);
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

    public void ChangeText(string text)
    {
        StartCoroutine(HideText(text));
    }
    public IEnumerator ShowText(string text)
    {
        previousText = text;
        for (int i = 0; i <= text.Length; i++)
        {

            currentText = text.Substring(0, i);
            textTuto.text = currentText;
            yield return new WaitForSeconds(showTextDelay);
        }
    }

    public IEnumerator HideText(string text)
    {
        for (int j = previousText.Length - 1; j >= 0; j--)
        {
            currentText = previousText.Substring(0, j);
            textTuto.text = currentText;
            yield return new WaitForSeconds(hideTextDelay);
        }
        StartCoroutine(ShowText(text));
    }

    public void ChangePos(GameObject g)
    {
        transform.position = new Vector3(g.transform.position.x, g.transform.position.y + 1.5f, g.transform.position.z);
    }
}
