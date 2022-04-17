using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMedicDetection : MonoBehaviour
{
    public combatState enemyHealerState;
    void Start()
    {
        enemyHealerState = gameObject.GetComponentInParent<combatState>();
    }

    void Update()
    {
        if(enemyHealerState.enemies.Count > 0) 
        {
            enemyHealerState.enemies.Clear();
        }

        foreach (combatState detectedObject in enemyHealerState.detected)
        {
            if (detectedObject == null)
            {
                enemyHealerState.detected.Remove(detectedObject);
            }

            if (detectedObject.tag == "Enemy")
            {
                enemyHealerState.enemies.Add(detectedObject);
            }
        }

        if(enemyHealerState.currentState != combatState.STATE.Healing)
        {
            if (enemyHealerState.enemies.Count > 0)
            {
                enemyHealerState.ChangeState(combatState.STATE.Healing);
            }
            else enemyHealerState.ChangeState(combatState.STATE.OutOfCombat);
        }
        else
        {
            if(enemyHealerState.enemies.Count <= 0) enemyHealerState.ChangeState(combatState.STATE.OutOfCombat);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemyHealerState.detected.Add(other.GetComponent<combatState>());
        }
    }
    public void OnTriggerExit(Collider other)
    {
        enemyHealerState.detected.Remove(other.GetComponent<combatState>());
    }
}
