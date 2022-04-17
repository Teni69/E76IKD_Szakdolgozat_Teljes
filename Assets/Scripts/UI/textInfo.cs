using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textInfo : MonoBehaviour
{
    public Text currencyText;
    public Text energyText;
    public int currencyValue;
    public int energyValue;

    void Update()
    {
        if(GameObject.Find("MainBase") != null)
        {
            currencyValue = GameObject.Find("MainBase").GetComponent<currency>().currentCurrency;
        }
        if(GameObject.Find("Generator") != null)
        {
            energyValue = GameObject.Find("Generator").GetComponent<energyManager>().energy;
        }
        energyText.text = "Nrg: "+energyValue;
        currencyText.text = "Currency: "+currencyValue;
    }
}
