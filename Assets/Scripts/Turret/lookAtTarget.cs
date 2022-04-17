using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtTarget : MonoBehaviour
{
    public combatState cState, target;
    void Start()
    {
        cState = gameObject.GetComponentInParent<combatState>();
    }

    void Update()
    {
        if(cState != null)
        {
            if(cState.currentState == combatState.STATE.InCombat)
            {
                if(cState.enemies.Count != 0)
                {
                    target = cState.enemies[0];
                    foreach (var enemy in cState.enemies)
                    {
                        //Current target
                        float dist1 = Vector3.Distance(transform.position, target.transform.position);
                        //Next target
                        float dist2 = Vector3.Distance(transform.position, enemy.transform.position);
                        //Pick closer
                        if(dist1 > dist2) target = enemy;
                        transform.LookAt(target.transform.position);
                    }
                }
            }
        }
    }
}
