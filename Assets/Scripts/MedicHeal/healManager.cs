using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healManager : MonoBehaviour
{
    public combatState cState, target;
    public float heal;

    void Start()
    {
        cState = gameObject.GetComponent<combatState>();
        InvokeRepeating("healing", 0f, 1f);
    }

    void Update()
    {
        healingPriority();
    }

    public void healingPriority(){
        if(cState.currentState == combatState.STATE.Healing)
        {
            if(cState.enemies.Count != 0)
            {
                target = cState.enemies[0];
                var targetEnemy = GameObject.Find(target.gameObject.name);
                float hp = targetEnemy.GetComponent<UnitHealth>().Health;
                foreach (var enemy in cState.enemies)
                {
                    //Current target
                    float hp2 = enemy.GetComponent<UnitHealth>().Health;
                    //Pick closer
                    if(hp2 < hp) target = enemy;
                }
            }
        }
    }

    public void healing()
    {
        if(cState.currentState == combatState.STATE.Healing && target != null)
        {
            var targetEnemy = GameObject.Find(target.gameObject.name);
            var healthManager = GameObject.Find("SelectManager").GetComponent<HealthManager>();
            if (targetEnemy.GetComponent<UnitHealth>().Health < 100)
            {
                healthManager.UpdateHealthbarToUnit(targetEnemy, heal);
            }
        }
    }
}
