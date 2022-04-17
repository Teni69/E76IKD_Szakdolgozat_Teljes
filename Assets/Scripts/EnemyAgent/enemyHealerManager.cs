using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyHealerManager : MonoBehaviour
{
    private Vector3 targetDestination;
    private NavMeshAgent agent;
    public Dictionary<NavMeshAgent, Vector3> healerDestinations = new Dictionary<NavMeshAgent, Vector3>();
    public Material defaultMaterial;
    private bool toHealRoutineBool = true;
    private GameObject wounded;
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        var renderer = gameObject.GetComponent<Renderer>();
        renderer.material = defaultMaterial;
    }

    void Update()
    {
        EnemyList();
        SetTargetDestination();
        CheckOffMeshLink();
    }

    void EnemyList(){
        if(toHealRoutineBool)
        {
            StartCoroutine(toHealRoutine());
            toHealRoutineBool = false;
        }
    }

    private IEnumerator toHealRoutine()
    {
        toHealRoutineBool = false;
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < enemyInstantiate.enemies.Count; i++)
        {
            var currentHp = enemyInstantiate.enemies[i].GetComponent<UnitHealth>().Health;
            var maxHp = enemyInstantiate.enemies[i].GetComponent<UnitHealth>().MaxHealth;
            var tempName = enemyInstantiate.enemies[i].transform.gameObject.name.Substring(0,10);
            if(maxHp > currentHp && tempName != "MedicEnemy")
            {
                NavMeshAgent toHealAgent = enemyInstantiate.enemies[i].gameObject.GetComponent<NavMeshAgent>();
                targetDestination = toHealAgent.nextPosition;
                wounded = enemyInstantiate.enemies[i];
            }
        }
        toHealRoutineBool = true;
    }

    void SetTargetDestination()
    {
        OffMeshLinkData data = agent.nextOffMeshLinkData;
        if(agent != null && wounded != null)
        {
            if(!healerDestinations.ContainsKey(agent) && data.valid)
            {
                healerDestinations.Add(agent, targetDestination);
            }
            agent.SetDestination(targetDestination);
        }
    }

    void CheckOffMeshLink()
    {
        OffMeshLinkData nextData = agent.nextOffMeshLinkData;
        if(agent.isOnOffMeshLink)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(data.endPos, out hit, 10f, NavMesh.AllAreas))
            {
                Vector3 endpoint = hit.position + Vector3.up * agent.baseOffset;
                Vector3 distanceBetween = endpoint - agent.transform.position;
                agent.transform.position = Vector3.MoveTowards(agent.transform.position, endpoint, agent.speed * Time.deltaTime);
                if (distanceBetween.magnitude < 0.01f)
                {
                    agent.Warp(endpoint);
                    agent.CompleteOffMeshLink();
                    ContinuePath(nextData);
                }
            }
        }
    }

    void ContinuePath(OffMeshLinkData data)
    {
        foreach (var item in healerDestinations)
        {
            item.Key.SetDestination(item.Value);
            item.Key.updateUpAxis = true;
        }
        if(!data.valid) healerDestinations.Clear();
    }
}
