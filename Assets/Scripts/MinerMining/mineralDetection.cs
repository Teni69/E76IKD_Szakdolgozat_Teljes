using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mineralDetection : MonoBehaviour
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

            if (detectedObject.tag == "Mineral")
            {
                cState.enemies.Add(detectedObject);
            }
        }

        if(cState.currentState != combatState.STATE.Mining)
        {
            if (cState.enemies.Count > 0)
            {
                cState.ChangeState(combatState.STATE.Mining);
            }
            else cState.ChangeState(combatState.STATE.OutOfCombat);
        }
        else
        {
            if(cState.enemies.Count <= 0) cState.ChangeState(combatState.STATE.OutOfCombat);
        }
    }

    //COLLISON KELL
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mineral")
        {
            cState.detected.Add(other.GetComponent<combatState>());
        }
    }
    public void OnTriggerExit(Collider other)
    {
        cState.detected.Remove(other.GetComponent<combatState>());
    }
}
