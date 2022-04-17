using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyMiningManager : MonoBehaviour
{
    private Vector3 targetDestination;
    public NavMeshAgent agent;
    public Dictionary<NavMeshAgent, Vector3> minerDestinations = new Dictionary<NavMeshAgent, Vector3>();
    public combatState cState;
    public Material defaultMaterial;

    public int currency = 750;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        var renderer = gameObject.GetComponent<Renderer>();
        renderer.material = defaultMaterial;
        int rnd = Random.Range(0,2);
        var min1 = GameObject.Find("destinationCrystal").transform.position;
        var min2 = GameObject.Find("destinationCrystal2").transform.position;
        if(rnd == 0)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(min1, out hit, 10f, NavMesh.AllAreas))
            {
                targetDestination = hit.position;
            }
        }
        else if(rnd == 1)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(min2, out hit, 10f, NavMesh.AllAreas))
            {
                targetDestination = hit.position;
            }
        }
        cState = gameObject.GetComponent<combatState>();
        InvokeRepeating("mining", 0f, 2f);
    }

    void Update()
    {
        SetTargetDestination();
        CheckOffMeshLink();
    }

    void mining()
    {
        if(cState.currentState == combatState.STATE.Mining && cState != null)
        {
           GameObject.Find("EnemyBase").GetComponent<currency>().currentCurrency += 10;
        }
    }

    void SetTargetDestination()
    {
        OffMeshLinkData data = agent.nextOffMeshLinkData;
        var dist = Vector3.Distance(agent.transform.position, targetDestination);
        if(dist > 10f && agent != null)
        {
            if(!minerDestinations.ContainsKey(agent) && data.valid)
            {
                minerDestinations.Add(agent, targetDestination);
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
        foreach (var item in minerDestinations)
        {
            item.Key.SetDestination(item.Value);
            item.Key.updateUpAxis = true;
        }
        if(!data.valid) minerDestinations.Clear();
    }
}
