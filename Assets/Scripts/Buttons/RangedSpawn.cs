using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedSpawn : MonoBehaviour
{
    public Button rangedButton;
    public GameObject rangedPrefab;
    private int i;
    private int currencyValue;

    void Start()
    {
        rangedButton.GetComponentInChildren<Text>().text = "Ranged: 200";
        Button btn = rangedButton.GetComponent<Button>();
        btn.onClick.AddListener(createRanged);
    }

        void Update()
    {
        currencyValue = GameObject.Find("MainBase").GetComponent<currency>().currentCurrency;
        if(currencyValue >= 200)
        {
            rangedButton.GetComponentInChildren<Text>().color = Color.green;
        } else  rangedButton.GetComponentInChildren<Text>().color = Color.red;
    }

    void createRanged(){
        if(currencyValue >= 200)
        {
            var clone = Instantiate(rangedPrefab, new Vector3(-406, 49, 10), Quaternion.identity);
            i = i+1;
            clone.name = "Ranged"+i;
            UnitController.CallUnitSpawn(clone);
            GameObject.Find("MainBase").GetComponent<currency>().currentCurrency -= 200;
        }
    }
}
