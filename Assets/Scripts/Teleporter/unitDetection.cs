using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitDetection : MonoBehaviour
{
    public combatState cState;
    void Start()
    {
        cState = gameObject.GetComponent<combatState>();
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

            if (detectedObject.tag == "Enemy" || detectedObject.tag == "Selectable")
            {
                cState.enemies.Add(detectedObject);
            }
        }

        if(cState.currentState != combatState.STATE.InUse)
        {
            if (cState.enemies.Count > 0)
            {
                cState.ChangeState(combatState.STATE.InUse);
            }
            else cState.ChangeState(combatState.STATE.OutOfUse);
        }
        else
        {
            if(cState.enemies.Count <= 0) cState.ChangeState(combatState.STATE.OutOfUse);
        }
    }

    //COLLISON KELL
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Selectable")
        {
            cState.detected.Add(other.GetComponent<combatState>());
        }
    }
    public void OnTriggerExit(Collider other)
    {
        cState.detected.Remove(other.GetComponent<combatState>());
    }
}
