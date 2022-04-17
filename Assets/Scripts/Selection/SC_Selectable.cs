using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Selectable : MonoBehaviour
{
    public Renderer[] renderers; //Assign all child Mesh Renderers

    public Bounds GetObjectBounds()
    {
        Bounds totalBounds = new Bounds();

        for(int i = 0; i < renderers.Length; i++)
        {
            if(totalBounds.center == Vector3.zero)
            {
                totalBounds = renderers[i].bounds;
            }
            else
            {
                totalBounds.Encapsulate(renderers[i].bounds);
            }
        }

        return totalBounds;
    }

    void OnEnable()
    {
        //Add this Object to global list
        if (!selectManager.selectables.Contains(this))
        {
            selectManager.selectables.Add(this);
        }
    }

    void OnDisable()
    {
        //Remove this Object from global list
        if (selectManager.selectables.Contains(this))
        {
            selectManager.selectables.Remove(this);
        }
    }
}