using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchCombat : MonoBehaviour
{
    public void LaunchCombatFunc()
    {
        SceneManager.LoadScene("SceneCombat");
    }
}
