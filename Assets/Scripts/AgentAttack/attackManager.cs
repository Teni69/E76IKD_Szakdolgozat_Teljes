using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class attackManager : MonoBehaviour
{
    public combatState cState, target;
    public float damage;
    public GameObject projectilePrefab;
    private Transform unitTrans;
    private Rigidbody rb;

    void Start()
    {
        cState = gameObject.GetComponent<combatState>();
        unitTrans = gameObject.GetComponent<Transform>();
        InvokeRepeating("attack", 0f, 1f);
    }

    void Update()
    {
        attackPriority();
    }

    public void attackPriority()
    {
        if(cState.currentState == combatState.STATE.InCombat)
        {
            if(cState.enemies.Count != 0)
            {
                target = cState.enemies[0];
                foreach (var enemy in cState.enemies)
                {
                    enemy.currentState = combatState.STATE.InCombat;
                    //Current target
                    float dist1 = Vector3.Distance(transform.position, target.transform.position);
                    //Next target
                    float dist2 = Vector3.Distance(transform.position, enemy.transform.position);
                    //Pick closer
                    if(dist1 > dist2)
                    {
                        target = enemy;
                    }
                }
            }
        }
    }
    public void attack()
    {
        if(cState.currentState == combatState.STATE.InCombat && target != null)
        {
            if(GameObject.Find(target.gameObject.name) != null)
            {
                var targetEnemy = GameObject.Find(target.gameObject.name);
                if(targetEnemy != null)
                {
                    var healthManager = GameObject.Find("SelectManager").GetComponent<HealthManager>();
                    if (targetEnemy.GetComponent<UnitHealth>().Health > 0)
                    {
                        healthManager.UpdateHealthbarToUnit(targetEnemy, -damage);
                        unitTrans.LookAt(target.gameObject.transform);
                        rb = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                        rb.AddForce(transform.forward * 38f, ForceMode.Impulse);
                        Destroy(rb.gameObject, 0.2f);
                    }
                    else
                    {
                        healthManager.RemoveHealthbarToUnitDead(targetEnemy);
                        Destroy(targetEnemy);
                        target = null;
                    }
                }
            }
            else 
            {
                if(rb != null){
                    Destroy(rb.gameObject, 0.2f);
                }
                cState.currentState = combatState.STATE.OutOfCombat;
                target = null;
            }
        }
        else target = null;
    }

    public float getDmg()
    {
        return damage;
    }
}
