using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagedBarScript : MonoBehaviour
{
    public Slider damagedBar;
    public Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        damagedBar = GetComponent<Slider>();
        damagedBar.maxValue = healthBar.maxValue;
        damagedBar.value = healthBar.value;
    }
    private void Update()
    {
        damagedBar.maxValue = healthBar.maxValue;
    }

    public void UpdateDamagedBar(float value, float duration, bool heal)
    {
        if (heal)
        {
            damagedBar.value = healthBar.value;
            damagedBar.gameObject.GetComponentInChildren<Image>().color = Color.green;
            StartCoroutine(TakeHealingCor(value, duration/2));
        }
        else
        {
            damagedBar.gameObject.GetComponentInChildren<Image>().color = Color.grey;
            StartCoroutine(TakeDamageCor(value, duration/2));
        }
    }
    public IEnumerator TakeDamageCor(float value, float duration)
    {
        float startValue = damagedBar.value;
        float endValue = value;
        float elapsed = 0.0f;
        float ratio = 0.0f;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            damagedBar.value = Mathf.Lerp(startValue, endValue, ratio);
            if (damagedBar.value <= 0)
            {
                break;
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
        damagedBar.value = endValue;
        yield return new WaitForSeconds(duration);
    }
    public IEnumerator TakeHealingCor(float value, float duration)
    {
        float startValue = damagedBar.value;
        float endValue = value;
        damagedBar.value = endValue;
        yield return null;
        /*float startValue = damagedBar.value;
        
        endValue = Mathf.Round(endValue);
        float elapsed = 0.0f;
        float ratio = 0.0f;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            damagedBar.value = Mathf.Lerp(startValue, endValue, ratio);
            elapsed += Time.deltaTime;
            yield return null;
        }
        damagedBar.value = endValue;
        yield return new WaitForSeconds(duration);*/
    }
}
