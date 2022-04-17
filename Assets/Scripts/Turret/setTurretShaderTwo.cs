using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setTurretShaderTwo : MonoBehaviour
{
    public Material activeShader;
    public Material inactiveShader;
    private Renderer turretRenderer;
    public GameObject turret;
    void Start()
    {
        turretRenderer = GameObject.Find("TBasic_lvl1_top2").GetComponent<Renderer>();
    }

    void Update()
    {
        if(turretRenderer != null)
        {
            if(turret.GetComponent<attackManager>().damage == 0f)
            {
                turretRenderer.material = inactiveShader;
            }
            else turretRenderer.material = activeShader;
        }
    }
}