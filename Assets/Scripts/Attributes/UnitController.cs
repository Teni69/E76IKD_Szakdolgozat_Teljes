using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public delegate void UnitEventHandler(GameObject unit);

    public static event UnitEventHandler onUnitSpawn;
    public static event UnitEventHandler onUnitDead;

    public static void CallUnitSpawn(GameObject unit)
    {
        if(onUnitSpawn != null)
            onUnitSpawn(unit);
    }

    public static void CallUnitDead(GameObject unit)
    {
        if(onUnitDead != null)
            onUnitDead(unit);
    }
}
