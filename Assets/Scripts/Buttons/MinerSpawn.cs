using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinerSpawn : MonoBehaviour
{
    public Button minerButton;
    public GameObject minerPrefab;
    private int i;
    private int currencyValue;

    void Start()
    {
        minerButton.GetComponentInChildren<Text>().text = "Miner: 500";
        Button btn = minerButton.GetComponent<Button>();
        btn.onClick.AddListener(createMiner);
    }

    void Update()
    {
        currencyValue = GameObject.Find("MainBase").GetComponent<currency>().currentCurrency;
        if(currencyValue >= 500)
        {
            minerButton.GetComponentInChildren<Text>().color = Color.green;
        } else  minerButton.GetComponentInChildren<Text>().color = Color.red;
    }

    void createMiner(){
        if(currencyValue >= 500)
        {
            var clone = Instantiate(minerPrefab, new Vector3(-403, 49, 7), Quaternion.identity);
            i = i+1;
            clone.name = "Miner"+i;
            UnitController.CallUnitSpawn(clone);
            GameObject.Find("MainBase").GetComponent<currency>().currentCurrency -= 500;
        }
    }
}
