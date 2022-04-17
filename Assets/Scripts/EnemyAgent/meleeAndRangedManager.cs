using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class meleeAndRangedManager : MonoBehaviour
{
    private Vector3 targetDestination;
    private NavMeshAgent agent;
    public Dictionary<NavMeshAgent, Vector3> unitDestinations = new Dictionary<NavMeshAgent, Vector3>();
    public Material defaultMaterial;
    private combatState enemycState, target;
    private bool randomPointBool = true;
    private bool followAttackerBool = true;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        var renderer = gameObject.GetComponent<Renderer>();
        renderer.material = defaultMaterial;
        enemycState = gameObject.GetComponent<combatState>();
    }

    void Update()
    {
        enemycState = gameObject.GetComponent<combatState>();
        SetTargetDestination();
        CheckOffMeshLink();
    }

    void SetTargetDestination()
    {
        OffMeshLinkData data = agent.nextOffMeshLinkData;
        if(enemycState.currentState == combatState.STATE.OutOfCombat && randomPointBool)
        {
            StartCoroutine(RandomPoint());
            var dist = Vector3.Distance(agent.transform.position, targetDestination);
            if(agent != null)
            {
                if(!unitDestinations.ContainsKey(agent) && data.valid)
                {
                    unitDestinations.Add(agent, targetDestination);
                }
                agent.SetDestination(targetDestination);
            }
            randomPointBool = false;
        }
        else if(enemycState.currentState == combatState.STATE.InCombat && followAttackerBool)
        {
            StartCoroutine(FollowAttacker());
            var dist = Vector3.Distance(agent.transform.position, targetDestination);
            if(agent != null)
            {
                if(!unitDestinations.ContainsKey(agent) && data.valid)
                {
                    unitDestinations.Add(agent, targetDestination);
                }
                agent.SetDestination(targetDestination);
            }
            followAttackerBool = false;
        }
    }

    public Vector3 RandomNavmeshLocation(float radius) {
         Vector3 randomDirection = Random.insideUnitSphere * radius;
         randomDirection += transform.position;
         NavMeshHit hit;
         Vector3 finalPosition = Vector3.zero;
         if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
             finalPosition = hit.position;            
         }
         return finalPosition;
    }

    private IEnumerator RandomPoint()
    {
        if(enemycState.currentState == combatState.STATE.OutOfCombat)
        {
            randomPointBool = false;
            yield return new WaitForSeconds(5);
            targetDestination = RandomNavmeshLocation(15f);
            randomPointBool = true;
        }
    }

    private IEnumerator FollowAttacker()
    {
        if(enemycState.currentState == combatState.STATE.InCombat)
        {
            followAttackerBool = false;
            yield return new WaitForSeconds(0.1f);
            if(enemycState.enemies.Count != 0)
            {
                target = enemycState.enemies[0];
                NavMeshAgent enemyAgent =  target.gameObject.GetComponent<NavMeshAgent>();
                targetDestination = enemyAgent.nextPosition;
            }
            followAttackerBool = true;
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
        foreach (var item in unitDestinations)
        {
            item.Key.SetDestination(item.Value);
            item.Key.updateUpAxis = true;
        }
        if(!data.valid) unitDestinations.Clear();
    }
}
