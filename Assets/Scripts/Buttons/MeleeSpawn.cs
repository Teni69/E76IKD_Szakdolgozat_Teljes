using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeSpawn : MonoBehaviour
{
    public Button meleeButton;
    public GameObject meleePrefab;
    private int i;
    private int currencyValue;

    void Start()
    {
        meleeButton.GetComponentInChildren<Text>().text = "Melee: 100";
        Button btn = meleeButton.GetComponent<Button>();
        btn.onClick.AddListener(createMelee);
    }

    void Update()
    {
        currencyValue = GameObject.Find("MainBase").GetComponent<currency>().currentCurrency;
        if(currencyValue >= 100)
        {
            meleeButton.GetComponentInChildren<Text>().color = Color.green;
        } else  meleeButton.GetComponentInChildren<Text>().color = Color.red;
    }

    void createMelee(){
        if(currencyValue >= 100)
        {
            var clone = Instantiate(meleePrefab, new Vector3(-406, 49, 7), Quaternion.identity);
            i = i+1;
            clone.name = "Melee"+i;
            UnitController.CallUnitSpawn(clone);
            GameObject.Find("MainBase").GetComponent<currency>().currentCurrency -= 100;
        }
    }
}
