using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyInstantiate : MonoBehaviour
{
    public GameObject enemyMiner;
    public GameObject enemyUnit;
    public GameObject enemyRanged;
    public GameObject enemyMedic;
    private int minerI;
    private int unitI;
    private int rangedI;
    private int medicI;
    private int allCurrency;
    private bool spawnerBool = true;
    private bool enemyListRoutineBool = true;
    private int rnd;
    public static List<GameObject> enemies = new List<GameObject>();
    void Start()
    {
        allCurrency = GameObject.Find("EnemyBase").GetComponent<currency>().currentCurrency;
        createEnemyMiner();
        createEnemyUnit();
        createEnemyUnit();
        createEnemyRanged();
        createEnemyMedic();
    }

    void Update()
    {
        allCurrency = GameObject.Find("EnemyBase").GetComponent<currency>().currentCurrency;
        var minerAlive = GameObject.Find("EnemyMiner"+minerI);
        if(minerAlive == null) createEnemyMiner();
        Spawner();
    }

    void Spawner()
    {
        if(spawnerBool)
        {
            StartCoroutine(SpawnerRoutine());
            spawnerBool = false;
        }
    }

    private IEnumerator SpawnerRoutine()
    {
        yield return new WaitForSeconds(20);
        rnd = Random.Range(0,3);
        if(rnd == 1 && allCurrency >= 50)
        {
            createEnemyUnit();
        }
        else if(rnd == 0 && allCurrency >= 100)
        {
            createEnemyRanged();
        }
        else if(rnd == 2 && allCurrency >= 400)
        {
            createEnemyMedic();
        }
        spawnerBool = true;
    }

    void EnemyList(){
        if(enemyListRoutineBool)
        {
            StartCoroutine(EnemyListRoutine());
            enemyListRoutineBool = false;
        }
    }

    private IEnumerator EnemyListRoutine()
    {
        yield return new WaitForSeconds(1);

        for(var i = enemies.Count - 1; i > -1; i--)
        {
           if (enemies[i] == null)
              enemies.RemoveAt(i);
        }
        enemyListRoutineBool = true;
    }

    void createEnemyMiner()
    {
            Vector3 spawnDest = (GameObject.Find("EnemyBase").transform.position - new Vector3(0, 0, -5));
            var clone = Instantiate(enemyMiner, spawnDest, Quaternion.identity);
            minerI = minerI+1;
            clone.name = "EnemyMiner"+minerI;
            UnitController.CallUnitSpawn(clone);
            enemies.Add(clone);
    }

    void createEnemyUnit()
    {
        if(allCurrency >= 50)
        {
            Vector3 spawnDest = (GameObject.Find("EnemyBase").transform.position - new Vector3(3, 0, -5));
            var clone = Instantiate(enemyUnit, spawnDest, Quaternion.identity);
            unitI = unitI+1;
            clone.name = "EnemyUnit"+unitI;
            UnitController.CallUnitSpawn(clone);
            GameObject.Find("EnemyBase").GetComponent<currency>().currentCurrency -= 50;
            enemies.Add(clone);
        }
    }

    void createEnemyRanged()
    {
        if(allCurrency >= 100)
        {
            Vector3 spawnDest = (GameObject.Find("EnemyBase").transform.position - new Vector3(6, 0, -5));
            var clone = Instantiate(enemyRanged, spawnDest, Quaternion.identity);
            rangedI = rangedI+1;
            clone.name = "EnemyRanged"+rangedI;
            UnitController.CallUnitSpawn(clone);
            GameObject.Find("EnemyBase").GetComponent<currency>().currentCurrency -= 100;
            enemies.Add(clone);
        }
    }

    void createEnemyMedic()
    {
        if(allCurrency >= 400)
        {
            Vector3 spawnDest = (GameObject.Find("EnemyBase").transform.position - new Vector3(0, 0, 5));
            var clone = Instantiate(enemyMedic, spawnDest, Quaternion.identity);
            medicI = medicI+1;
            clone.name = "EnemyMedic"+medicI;
            UnitController.CallUnitSpawn(clone);
            GameObject.Find("EnemyBase").GetComponent<currency>().currentCurrency -= 400;
            enemies.Add(clone);
        }
    }
}
