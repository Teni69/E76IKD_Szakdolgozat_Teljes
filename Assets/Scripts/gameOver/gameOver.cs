using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour
{
    private float playerHealth;
    private float enemyHealth;
    public string gameName;
    void Start()
    {
        playerHealth = GameObject.Find("MainBase").GetComponent<UnitHealth>().Health;
        enemyHealth = GameObject.Find("EnemyBase").GetComponent<UnitHealth>().Health;
    }

    void Update()
    {
        playerHealth = GameObject.Find("MainBase").GetComponent<UnitHealth>().Health;
        enemyHealth = GameObject.Find("EnemyBase").GetComponent<UnitHealth>().Health;
        if(playerHealth <= 0 || enemyHealth <= 0) SceneManager.LoadScene(gameName);
    }
}
