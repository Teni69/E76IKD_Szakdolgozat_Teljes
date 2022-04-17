using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class BuildingSelected : MonoBehaviour
{
    private string BaseTag = "MainBase";
    private string BarrackTag = "Barrack";
    private GameObject[] barrackButtons;
    private GameObject[] mainBaseButtons;
    public GameObject Barrack;
    private Camera blueCam;

    void Start(){
        barrackButtons = GameObject.FindGameObjectsWithTag("BarrackButtons");
        mainBaseButtons = GameObject.FindGameObjectsWithTag("Buttons");
        foreach(GameObject button in barrackButtons)
        {
            button.SetActive(false);
        }
        foreach(GameObject button in mainBaseButtons)
        {
            button.SetActive(false);
        }
        blueCam = Camera.main;
    }

    void LateUpdate()
    {
        if(blueCam.enabled == true)
        {
            Ray ray = blueCam.ViewportPointToRay(blueCam.ScreenToViewportPoint(Input.mousePosition));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(!IsPointerOverUIObject())
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        var selection = hit.transform;
                        if(selection.CompareTag(BaseTag))
                        {
                             foreach(GameObject button in barrackButtons)
                            {
                                button.SetActive(false);
                            }
                            foreach(GameObject button in mainBaseButtons)
                            {
                                button.SetActive(true);
                            }
                        }
                        else if (selection.CompareTag(BarrackTag))
                        {
                            foreach(GameObject button in mainBaseButtons)
                            {
                                button.SetActive(false);
                            }
                            foreach(GameObject button in barrackButtons)
                            {
                                button.SetActive(true);
                            }
                        }
                    }
                }
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 1;
    }
}
