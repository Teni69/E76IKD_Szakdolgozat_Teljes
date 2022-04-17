using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class informationPanel : MonoBehaviour
{
    private Camera camera1;
    private Camera camera2;
    private Camera cameraSelected;

    private combatState.STATE state;
    private float hp, maxHp, dmg;
    private string unitName;
    private CanvasGroup canvasGroup;
    private Transform selection;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        camera1 = GameObject.Find("PlanetCameraBlue").GetComponent<Camera>();
        camera2 = GameObject.Find("PlanetCameraRed").GetComponent<Camera>();
        cameraSelected = Camera.main;
        Hide();
    }

    void Update()
    {
        if(camera1.enabled == true)
        {
            cameraSelected = camera1.GetComponent<Camera>();
        }
        else if(camera2.enabled == true)
        {
            cameraSelected = camera2.GetComponent<Camera>();
        }

        Ray ray = cameraSelected.ViewportPointToRay(cameraSelected.ScreenToViewportPoint(Input.mousePosition));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(!IsPointerOverUIObject())
            {
                if (Input.GetMouseButton(0))
                {
                    if(hit.transform.CompareTag("Selectable") || hit.transform.CompareTag("Enemy")
                        || hit.transform.CompareTag("MainBase") || hit.transform.CompareTag("Generator") || hit.transform.CompareTag("Barrack")
                        || hit.transform.CompareTag("Turret") || hit.transform.CompareTag("EnemyBuilding") || hit.transform.CompareTag("EnemyTurret"))
                    {   
                        selection = hit.transform;
                        Show();
                    } else Hide();
                }
            }
        }
        if(selection != null)
        { 
            refreshPanel();
        }
    }

    void refreshPanel()
    {
        var targetToGetFrom = GameObject.Find(selection.gameObject.name);

        state = targetToGetFrom.GetComponent<combatState>().currentState;
        hp    = targetToGetFrom.GetComponent<UnitHealth>().Health;
        maxHp = targetToGetFrom.GetComponent<UnitHealth>().MaxHealth;
        var dmgScript = targetToGetFrom.GetComponent<attackManager>();
        if(dmgScript != null)
        {
            dmg = dmgScript.getDmg();
        } else dmg = 0;
        unitName  = targetToGetFrom.name;

        GameObject.Find("selectedName").GetComponent<Text>().text = unitName;
        GameObject.Find("selectedHp").GetComponent<Text>().text = "Health: "+hp.ToString()+"/"+maxHp.ToString();
        GameObject.Find("selectedDmg").GetComponent<Text>().text = "Damage: "+dmg.ToString();
        GameObject.Find("selectedState").GetComponent<Text>().text = state.ToString();
    }

    void Hide()
    {
     canvasGroup.alpha = 0f;
    }

    void Show()
    {
     canvasGroup.alpha = 1f;
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
