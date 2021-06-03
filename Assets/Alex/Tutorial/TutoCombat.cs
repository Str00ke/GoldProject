using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
public class TutoCombat : MonoBehaviour
{
    [Header("Objects")]
    public float durationStep;
    public static TutoCombat tutoCombat = null;
    public List<AllyTuto> allies;
    public List<CharactersTuto> fightersList;
    public CharactersTuto charSelected = null;
    public GameObject stepTutoButtonPrefab;
    public AllyTuto[] chars;
    CharactersTuto charAttackedByDeath = null;
    public GameObject fadePanel;

    [Header("Enemies")]
    public List<EnemyTuto> enemies;
    public float delay = 0.0f;
    public float attackDuration = 2.0f;

    [Header("FightVariable")]
    public AllyTuto allyPlaying;
    public int turnNumber = 0;
    public int currCharAttacking = 0;


    public int currentStepTuto;

    private void Awake()
    {
        if (tutoCombat == null)
        {
            tutoCombat = this;
        }
        else if (tutoCombat != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        fadePanel = GameObject.Find("FadePanel");
        TutorialFunction();
    }
    public void TutorialFunction()
    {
        StartCoroutine(TutorialFunctionCor());
    }
    public IEnumerator TutorialFunctionCor()
    {
        switch (currentStepTuto)
        {
            case 0:
                BreakTuto("An enemy appears! Stay behind me friends.");
                    break;
            case 1:
                BreakTuto("(Hold a character to select it, use its ability!)");
                break;
            case 2:
                allyPlaying = chars[0];
                charAttackedByDeath = chars[0];
                allyPlaying.cursorPlaying.SetActive(true);
                break;
            case 3:
                //GO TO NEXT STEP WITH ABILITY LAUNCH
                chars[0].cursorPlaying.SetActive(false);
                chars[0].cursorNotPlayedYet.SetActive(false);
                allyPlaying = null;
                BreakTuto("Be careful! He is going to attack!");
                break;
            case 4:
                yield return new WaitForSeconds(durationStep);
                enemies[0].cursorPlaying.SetActive(true);
                enemies[0].LaunchAttack(charAttackedByDeath, enemies[0].abilities[0]);
                enemies[0].cursorNotPlayedYet.SetActive(false);
                enemies[0].cursorPlaying.SetActive(false);
                GoNextStepTuto();
                break;
            case 5:
                BreakTuto("Don't worry i will heal you!");
                break;
            case 6:
                allyPlaying = chars[1];
                allyPlaying.cursorPlaying.SetActive(true);
                break;
            case 7:
                chars[1].cursorPlaying.SetActive(false);
                chars[1].cursorNotPlayedYet.SetActive(false);
                allyPlaying = null;
                yield return new WaitForSeconds(durationStep);
                BreakTuto("It's our turn now!");
                break;
            case 8:
                yield return new WaitForSeconds(durationStep);
                allyPlaying = chars[2];
                allyPlaying.cursorPlaying.SetActive(true);
                break;
            case 9:
                chars[2].cursorPlaying.SetActive(false);
                chars[2].cursorNotPlayedYet.SetActive(false);
                allyPlaying = null;
                yield return new WaitForSeconds(durationStep);
                turnNumber++;
                UITuto.uiTuto.turnsText.text = "" + turnNumber;
                foreach (AllyTuto c in allies)
                {
                    if (!c.isDead)
                    {
                        c.cursorNotPlayedYet.SetActive(true);
                    }
                }
                yield return new WaitForSeconds(durationStep);
                BreakTuto("He is weaker now!");
                break;
            case 10:
                foreach(AllyTuto a in allies)
                {
                    switch (a.itemElement)
                    {
                        case CharactersTuto.ItemElement.ASH:
                            a.abilitiesCristal[0] = AbilitiesTuto.abilitiesTuto.cristalsAsh[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsAsh.Length)];
                            a.abilitiesCristal[1] = AbilitiesTuto.abilitiesTuto.cristalsAsh[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsAsh.Length)];
                            while (a.abilitiesCristal[0] == a.abilitiesCristal[1])
                            {
                                a.abilitiesCristal[0] = AbilitiesTuto.abilitiesTuto.cristalsAsh[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsAsh.Length)];
                                a.abilitiesCristal[1] = AbilitiesTuto.abilitiesTuto.cristalsAsh[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsAsh.Length)];
                            }
                            break;
                        case CharactersTuto.ItemElement.ICE:
                            a.abilitiesCristal[0] = AbilitiesTuto.abilitiesTuto.cristalsIce[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsIce.Length)];
                            a.abilitiesCristal[1] = AbilitiesTuto.abilitiesTuto.cristalsIce[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsIce.Length)];
                            while (a.abilitiesCristal[0] == a.abilitiesCristal[1])
                            {
                                a.abilitiesCristal[0] = AbilitiesTuto.abilitiesTuto.cristalsIce[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsIce.Length)];
                                a.abilitiesCristal[1] = AbilitiesTuto.abilitiesTuto.cristalsIce[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsIce.Length)];
                            }
                            break;
                        case CharactersTuto.ItemElement.MUD:
                            a.abilitiesCristal[0] = AbilitiesTuto.abilitiesTuto.cristalsMud[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsMud.Length)];
                            a.abilitiesCristal[1] = AbilitiesTuto.abilitiesTuto.cristalsMud[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsMud.Length)];
                            while (a.abilitiesCristal[0] == a.abilitiesCristal[1])
                            {
                                a.abilitiesCristal[0] = AbilitiesTuto.abilitiesTuto.cristalsMud[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsMud.Length)];
                                a.abilitiesCristal[1] = AbilitiesTuto.abilitiesTuto.cristalsMud[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsMud.Length)];
                            }
                            break;
                        case CharactersTuto.ItemElement.PSY:
                            a.abilitiesCristal[0] = AbilitiesTuto.abilitiesTuto.cristalsPsy[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsPsy.Length)];
                            a.abilitiesCristal[1] = AbilitiesTuto.abilitiesTuto.cristalsPsy[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsPsy.Length)];
                            while (a.abilitiesCristal[0] == a.abilitiesCristal[1])
                            {
                                a.abilitiesCristal[0] = AbilitiesTuto.abilitiesTuto.cristalsPsy[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsPsy.Length)];
                                a.abilitiesCristal[1] = AbilitiesTuto.abilitiesTuto.cristalsPsy[Random.Range(0, AbilitiesTuto.abilitiesTuto.cristalsPsy.Length)];
                            }
                            break;
                    }
                }
                chars[0].abilities[0] = AbilitiesTuto.abilitiesTuto.weaponAbilities[0];
                chars[0].abilities[1] = AbilitiesTuto.abilitiesTuto.weaponAbilities[2];

                chars[1].abilities[0] = AbilitiesTuto.abilitiesTuto.weaponAbilities[1];
                chars[1].abilities[1] = AbilitiesTuto.abilitiesTuto.weaponAbilities[4];

                chars[2].abilities[0] = AbilitiesTuto.abilitiesTuto.weaponAbilities[1];
                chars[2].abilities[1] = AbilitiesTuto.abilitiesTuto.weaponAbilities[3];


                allyPlaying = chars[0];
                allyPlaying.cursorPlaying.SetActive(true);
                yield return new WaitForSeconds(durationStep);
                BreakTuto("The crystals seem to give us random powers... We have to use them wisely!");
                break;
            case 11:
                //yield return new WaitForSeconds(durationStep);
                break;
            case 12:
                allyPlaying.cursorPlaying.SetActive(false);
                allyPlaying.cursorNotPlayedYet.SetActive(false);
                allyPlaying = null;
                yield return new WaitForSeconds(durationStep);
                BreakTuto("We can do it!");
                break;
            case 13:
                yield return new WaitForSeconds(durationStep);
                allyPlaying = chars[1];
                allyPlaying.cursorPlaying.SetActive(true);
                break;
            case 14:
                allyPlaying.cursorPlaying.SetActive(false);
                allyPlaying.cursorNotPlayedYet.SetActive(false);
                allyPlaying = null;
                yield return new WaitForSeconds(durationStep);
                BreakTuto("It's just a beast!");
                break;
            case 15:
                yield return new WaitForSeconds(durationStep);
                allyPlaying = chars[2];
                allyPlaying.cursorPlaying.SetActive(true);
                break;
            case 16:
                allyPlaying.cursorPlaying.SetActive(false);
                allyPlaying.cursorNotPlayedYet.SetActive(false);
                allyPlaying = null;
                yield return new WaitForSeconds(durationStep);
                FindObjectOfType<CameraScript>().CamShake(durationStep, 0.3f);
                BreakTuto("ENOUGH!!");
                break;
            case 17:
                BreakTuto("I will not let you go any further.");
                break;
            case 18:
                BreakTuto("This .. is MY domain.");
                break;
            case 19:
                enemies[0].damageRange = new Vector2(1000, 1500);
                BreakTuto("Die now...");
                break;
            case 20:
                yield return new WaitForSeconds(durationStep);
                FindObjectOfType<CameraScript>().CamShake(durationStep, 0.3f);
                enemies[0].cursorPlaying.SetActive(true);
                for (int i = allies.Count - 1; i >= 0; i--)
                {
                    enemies[0].LaunchAttack(allies[i], enemies[0].abilities[0]);
                }
                enemies[0].cursorNotPlayedYet.SetActive(false);
                enemies[0].cursorPlaying.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                GoNextStepTuto();
                break;
            case 21:
                float elapsed = 0.0f;
                float ratio = 0.0f;
                while (elapsed < durationStep)
                {
                    ratio = elapsed / durationStep;
                    fadePanel.GetComponent<Image>().color = Color.Lerp(new Color(fadePanel.GetComponent<Image>().color.r, fadePanel.GetComponent<Image>().color.g, fadePanel.GetComponent<Image>().color.b, 0), new Color(fadePanel.GetComponent<Image>().color.r, fadePanel.GetComponent<Image>().color.g, fadePanel.GetComponent<Image>().color.b, 1), ratio);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                yield return new WaitForSeconds(durationStep);
                PlayerPrefs.SetString("TutoEnded", "true");
                SceneManager.LoadScene("FScene");
                break;

        }
        yield return null;
    }
    public void disableChar(CharactersTuto c)
    {
        c.cursorPlaying.SetActive(false);
        c.cursorNotPlayedYet.SetActive(false);
    }
    public void BreakTuto(string textTuto)
    {
        GameObject tutoPrefab = Instantiate(stepTutoButtonPrefab);
        tutoPrefab.GetComponentInChildren<Text>().text = textTuto;
        tutoPrefab.transform.SetParent(GameObject.Find("Canvas").transform);
    }
    public void GoNextStepTuto()
    {
        currentStepTuto++;
        TutorialFunction();
    }
    public void RemoveAlly(AllyTuto a)
    {

        allies.Remove(a);
        fightersList.Remove(a);
    }
    void EndFight<T>()
    {
        if (typeof(T) == typeof(Enemy))
            FindObjectOfType<LevelManager>().WinFight();
        else if (typeof(T) == typeof(Ally))
            FindObjectOfType<LevelManager>().LoseFight();
    }

}
