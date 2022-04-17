using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miningManager : MonoBehaviour
{
    public combatState cState;

    void Start()
    {
        cState = gameObject.GetComponent<combatState>();
        InvokeRepeating("mining", 0f, 2f);
    }

    public void mining()
    {
        if(cState.currentState == combatState.STATE.Mining && cState != null)
        {
           GameObject.Find("MainBase").GetComponent<currency>().currentCurrency += 10;
        }
    }
}
