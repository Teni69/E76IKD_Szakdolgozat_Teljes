using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class teleportToTarget : MonoBehaviour
{
    public combatState cState;
    public GameObject teleportTo;

    void Start()
    {
        cState = gameObject.GetComponent<combatState>();
        InvokeRepeating("teleport", 0f, 2f);
    }

    public void teleport()
    {
        if(cState.currentState == combatState.STATE.InUse)
        {
            if(cState.enemies.Count != 0)
            {
                foreach (var enemy in cState.enemies)
                {
                    NavMeshAgent toTeleport = GameObject.Find(enemy.gameObject.name).GetComponent<NavMeshAgent>();
                    toTeleport.Warp(teleportTo.transform.position - new Vector3(0, 0, 5));
                    if(GameObject.Find(enemy.gameObject.name).GetComponent<movementManager>().destinations.ContainsKey(toTeleport))
                        {
                            GameObject.Find(enemy.gameObject.name).GetComponent<movementManager>().destinations.Clear();
                        }
                }
            }
        }
    }
}
