using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedicSpawn : MonoBehaviour
{
    public Button medicButton;
    public GameObject medicPrefab;
    private int i;
    private int currencyValue;

    void Start()
    {
        medicButton.GetComponentInChildren<Text>().text = "Medic: 1000";
        Button btn = medicButton.GetComponent<Button>();
        btn.onClick.AddListener(createMedic);
    }

    void Update()
    {
        currencyValue = GameObject.Find("MainBase").GetComponent<currency>().currentCurrency;
        if(currencyValue >= 1000)
        {
            medicButton.GetComponentInChildren<Text>().color = Color.green;
        } else  medicButton.GetComponentInChildren<Text>().color = Color.red;
    }

    void createMedic(){
        if(currencyValue >= 1000)
        {
            var clone = Instantiate(medicPrefab, new Vector3(-413, 49, 7), Quaternion.identity);
            i = i+1;
            clone.name = "Medic"+i;
            UnitController.CallUnitSpawn(clone);
            GameObject.Find("MainBase").GetComponent<currency>().currentCurrency -= 1000;
        }
    }
}
