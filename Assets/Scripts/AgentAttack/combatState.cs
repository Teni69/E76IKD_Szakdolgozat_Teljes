using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatState : MonoBehaviour
{
    public enum STATE {InCombat, OutOfCombat, Mining, InUse, OutOfUse, Healing};
    public STATE currentState;
    public List<combatState> detected, enemies;
    void Start()
    {
        ChangeState(STATE.OutOfCombat);
    }

    public void ChangeState(STATE state)
    {
        currentState = state;
    }

    public STATE GetState()
    {
        return currentState;
    }
}
