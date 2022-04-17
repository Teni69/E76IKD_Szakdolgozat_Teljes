using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyManager : MonoBehaviour
{
    public int energy = 0;
    void Start()
    {
        InvokeRepeating("makeEnergy", 0f, 1f);
    }

    public void makeEnergy()
    {
        if(GetComponent<Renderer>() != null)
        {
            energy += 5;
        }
    }
}
