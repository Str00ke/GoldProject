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


    [Header("Enemies")]
    public List<EnemyTuto> enemies;
    public float delay = 0.0f;
    public float attackDuration = 2.0f;

    [Header("FightVariable")]
    public AllyTuto allyPlaying;
    public int turnNumber = 0;
    public int currCharAttacking = 0;

    Coroutine nextCharCor;

    public string[] TutoStepTexts;
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
                BreakTuto(TutoStepTexts[0]);
                    break;
            case 1:
                BreakTuto(TutoStepTexts[1]);
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
                StatusTuto1 s1 = new StatusTuto1(chars[0], 0, chars[0].abilities[0], StatusTuto1.StatusTypes.DEFENCE);
                StatusTuto1 s2 = new StatusTuto1(chars[0], chars[0].armor * 0.6f, chars[0].abilities[0], StatusTuto1.StatusTypes.ARMORBONUS);
                allyPlaying = null;
                BreakTuto(TutoStepTexts[2]);
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
                allyPlaying = chars[0];
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
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
