using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDetection : MonoBehaviour
{
   public combatState enemycState;
    void Start()
    {
        enemycState = gameObject.GetComponentInParent<combatState>();
    }

    void Update()
    {
        if(enemycState.enemies.Count > 0) 
        {
            enemycState.enemies.Clear();
        }

        foreach (combatState detectedObject in enemycState.detected)
        {
            if (detectedObject == null)
            {
                enemycState.detected.Remove(detectedObject);
            }

            if (detectedObject.tag == "Selectable" || detectedObject.tag == "MainBase"
            || detectedObject.tag == "Barrack" || detectedObject.tag == "Generator" || detectedObject.tag == "Turret")
            {
                enemycState.enemies.Add(detectedObject);
            }
        }

        if(enemycState.currentState != combatState.STATE.InCombat)
        {
            if (enemycState.enemies.Count > 0)
            {
                enemycState.ChangeState(combatState.STATE.InCombat);
            }
            else enemycState.ChangeState(combatState.STATE.OutOfCombat);
        }
        else
        {
            if(enemycState.enemies.Count <= 0) enemycState.ChangeState(combatState.STATE.OutOfCombat);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Selectable" || other.tag == "MainBase"
            || other.tag == "Barrack" || other.tag == "Generator" || other.tag == "Turret")
        {
            enemycState.detected.Add(other.GetComponent<combatState>());
        }
    }
    public void OnTriggerExit(Collider other)
    {
        enemycState.detected.Remove(other.GetComponent<combatState>());
    }
}
