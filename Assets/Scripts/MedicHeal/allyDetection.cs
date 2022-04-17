using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allyDetection : MonoBehaviour
{
    public combatState allycState;
    void Start()
    {
        allycState = gameObject.GetComponentInParent<combatState>();
    }

    void Update()
    {
        if(allycState.enemies.Count > 0) 
        {
            allycState.enemies.Clear();
        }

        foreach (combatState detectedObject in allycState.detected)
        {
            if (detectedObject == null)
            {
                allycState.detected.Remove(detectedObject);
            }

            if (detectedObject.tag == "Selectable")
            {
                allycState.enemies.Add(detectedObject);
            }
        }

        if(allycState.currentState != combatState.STATE.Healing)
        {
            if (allycState.enemies.Count > 0)
            {
                allycState.ChangeState(combatState.STATE.Healing);
            }
            else allycState.ChangeState(combatState.STATE.OutOfCombat);
        }
        else
        {
            if(allycState.enemies.Count <= 0) allycState.ChangeState(combatState.STATE.OutOfCombat);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Selectable" || other.tag == "MainBase"
            || other.tag == "Barrack" || other.tag == "Generator")
        {
            allycState.detected.Add(other.GetComponent<combatState>());
        }
    }
    public void OnTriggerExit(Collider other)
    {
        allycState.detected.Remove(other.GetComponent<combatState>());
    }
}
