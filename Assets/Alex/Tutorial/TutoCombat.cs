using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
public class TutoCombat : MonoBehaviour
{
    [Header("Objects")]
    public static TutoCombat tutoCombat = null;
    public List<AllyTuto> allies;
    public List<CharactersTuto> fightersList;
    public CharactersTuto charSelected = null;
    public GameObject stepTutoButtonPrefab;
    public AllyTuto[] chars;


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
        switch (currentStepTuto)
        {
            case 0:
                BreakTuto(TutoStepTexts[0]);
                    break;
            case 1:
                allyPlaying = chars[0];
                allyPlaying.cursorPlaying.SetActive(true);
                break;
            case 2:
                chars[0].cursorPlaying.SetActive(false);
                chars[0].cursorNotPlayedYet.SetActive(false);
                allyPlaying = null;
                BreakTuto(TutoStepTexts[1]);
                break;
            case 3:
                enemies[0].cursorPlaying.SetActive(true);
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
        }
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
