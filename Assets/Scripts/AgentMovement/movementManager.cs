using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class movementManager : MonoBehaviour
{
    private Vector3 targetDestination;
    private NavMeshAgent currentAgent;
    public Dictionary<NavMeshAgent, Vector3> destinations = new Dictionary<NavMeshAgent, Vector3>();
    private Camera camera2;
    private Camera camera1;
    private Camera cameraSelected;
    private OffMeshLinkData nextData;

    void Start()
    {
        camera1 = GameObject.Find("PlanetCameraBlue").GetComponent<Camera>();
        camera2 = GameObject.Find("PlanetCameraRed").GetComponent<Camera>();
        cameraSelected = Camera.main;
        currentAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        SetTargetDestination();
        //Változó a pathon levő következő offmeshlinkhezw
        nextData = currentAgent.nextOffMeshLinkData;
        CheckOffMeshLink();
    }

    void SetTargetDestination()
    {
        if(camera1.enabled == true)
        {
            cameraSelected = camera1;
        }
        else if(camera2.enabled == true)
        {
            cameraSelected = camera2;
        }
        
        Ray ray = cameraSelected.ViewportPointToRay(cameraSelected.ScreenToViewportPoint(Input.mousePosition));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if(Input.GetMouseButton(1))
            {
                if(selection.CompareTag("planet1") || selection.CompareTag("planet2"))
                {
                    targetDestination = hit.point;
                    //Ciklus a kijelölt egységekhez
                    for (int i = 0; i < selectManager.units.Count; i++)
                    {
                        //Navmeshagent komponens kinyerése a gameobjectből
                        var selectedUnit = selectManager.units[i].GetComponent<NavMeshAgent>();

                        //Ha a dictionary nem tartalmazza az egységet, és az út tartalmaz
                        //offmeshlink áthaladást, hozzáadom a dictionaryhez
                        if(!destinations.ContainsKey(selectedUnit) && nextData.valid)
                        {
                            //Azért csak akkor, mert különben eltárolja a helyi navmesh pontját és törölnöm kéne a következő kattintással, mozgatás előtt.
                            //Ekkor viszont az offmeshlink után nem lenne meg az egység-pont páros, ugyanis utána be kell set-elni
                            destinations.Add(selectedUnit, targetDestination);
                        }
                        //Sima setdestination elég, hiszen nem haladok át linken
                        selectedUnit.SetDestination(targetDestination);
                    }
                }
            }
        }
    }

    void CheckOffMeshLink()
    {
        //Állandóan nézem hogy lesz e offmeshlink az út során, ugyanis csak mindig egyet (a következőt érzékeli)
        if(currentAgent.isOnOffMeshLink)
        {
            //Eltárolom az aktuális offmeshlink adatait
            OffMeshLinkData currentdata = currentAgent.currentOffMeshLinkData;

            //Navmeshhit az offmeshlink végpontjából a legközelebbi navmesh surface ponthoz
            NavMeshHit hit;
            if (NavMesh.SamplePosition(currentdata.endPos, out hit, 10f, NavMesh.AllAreas))
            {
                //Eltárolom a navmeshit által megjelölt pontot, plusz az currentAgent magasságát, ez a pontos érkezési pont.
                Vector3 endpoint = hit.position + Vector3.up * currentAgent.baseOffset;
                //Megnézem hogy a végpont és currentAgent közt mennyi a távolság
                Vector3 distanceBetween = endpoint - currentAgent.transform.position;
                //Menjen a navmeshpoint által meghatározott helyre általános sebességgel
                currentAgent.transform.position = Vector3.MoveTowards(currentAgent.transform.position, endpoint, currentAgent.speed * Time.deltaTime);
                //Ha a distancebetwwen kisebb mint 0.1 egység
                if (distanceBetween.magnitude < 0.01f)
                {
                    //warpolja le az currentAgentet a talajra, ezáltal nem akad be
                    currentAgent.Warp(endpoint);
                    //Completeoffmeshlink leszedi az offmeshlinkről, lehet mozogni tovább
                    currentAgent.CompleteOffMeshLink();
                    ContinuePath(nextData);
                }
            }
        }
    }

    void ContinuePath(OffMeshLinkData data)
    {
        //Miután lejött a linkről, végigmegyek a dictionaryn, és minden egységet az előre eltárolt pontjára küldök
        foreach (var item in destinations)
        {
            item.Key.SetDestination(item.Value);
            item.Key.updateUpAxis = true;
        }
        //Ha nincs a link után egy másik link, törölhetem a dictionaryt. Ha ez kimaradna, 
        //a Második linken fennakadna, ugyanis az első link után nem tudná settelni a célt, mert nem lennének értékpárok
        if(!nextData.valid) destinations.Clear();
    }
}
