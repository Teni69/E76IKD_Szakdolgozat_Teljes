using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDetection : MonoBehaviour
{
    public combatState cState;
    void Start()
    {
        cState = gameObject.GetComponentInParent<combatState>();
    }

    void Update()
    {
        if(cState.enemies.Count > 0) 
        {
            cState.enemies.Clear();
        }

        foreach (combatState detectedObject in cState.detected)
        {
            if (detectedObject == null)
            {
                cState.detected.Remove(detectedObject);
            }

            if (detectedObject.tag == "Enemy" || detectedObject.tag == "EnemyBuilding" || detectedObject.tag == "EnemyTurret")
            {
                cState.enemies.Add(detectedObject);
            }
        }

        if(cState.currentState != combatState.STATE.InCombat)
        {
            if (cState.enemies.Count > 0)
            {
                cState.ChangeState(combatState.STATE.InCombat);
            }
            else cState.ChangeState(combatState.STATE.OutOfCombat);
        }
        else
        {
            if(cState.enemies.Count <= 0) cState.ChangeState(combatState.STATE.OutOfCombat);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "EnemyBuilding" || other.tag == "EnemyTurret")
        {
            cState.detected.Add(other.GetComponent<combatState>());
        }
    }
    public void OnTriggerExit(Collider other)
    {
        cState.detected.Remove(other.GetComponent<combatState>());
    }
}
