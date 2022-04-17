using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretIsActive : MonoBehaviour
{
    public Button turretButton;
    public GameObject turret;
    private int i;
    private int energyValue;

    void Start()
    {
        turret.GetComponent<attackManager>().damage = 0f;
        turretButton.GetComponentInChildren<Text>().text = "Turret: 10 Nrg/sec";
        Button btn = turretButton.GetComponent<Button>();
        btn.onClick.AddListener(activateTurret);
        InvokeRepeating("energyDrain", 0f, 1f);
    }

    void Update()
    {
        if(turret != null)
        {
            energyValue = GameObject.Find("TextInfos").GetComponent<textInfo>().energyValue;
            if(energyValue < 10)
            {
                turret.GetComponent<attackManager>().damage = 0f;
            }
            else if(energyValue >= 30 && energyValue <= 50)
            {
                turretButton.GetComponentInChildren<Text>().color = Color.red;
            }
            else if(energyValue > 50 && energyValue <= 100)
            {
                turretButton.GetComponentInChildren<Text>().color = Color.yellow;
            }
            else if(energyValue > 100)
            {
                turretButton.GetComponentInChildren<Text>().color = Color.green;
            }
            else
            {
                turretButton.GetComponentInChildren<Text>().color = Color.red;
            }
        }
        else
        {
            turretButton.GetComponentInChildren<Text>().text = "Turret destroyed";
            Button btn = turretButton.GetComponent<Button>();
            btn.interactable = false;
        }
    }

    void activateTurret(){
        if(energyValue >= 10 && turret.GetComponent<attackManager>().damage == 0f)
        {
            if(turret != null)
            {
                turret.GetComponent<attackManager>().damage = 10f;
            }
        }
        else
        {
            turret.GetComponent<attackManager>().damage = 0f;
        }
    }

    void energyDrain(){
        if(turret != null)
        {
            if(turret.GetComponent<attackManager>().damage == 10f)
            {
                if(GameObject.Find("Generator") != null)
                GameObject.Find("Generator").GetComponent<energyManager>().energy -= 10;
            }
        }
    }
}
