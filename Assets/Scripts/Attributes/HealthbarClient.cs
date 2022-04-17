using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarClient : MonoBehaviour
{
    public Sprite myHealthbarSprite;
    public SpriteManager HealthbarSpriteManager;
    private combatState ClientState;
    private GameObject Client;
    public Vector3 Height;

    private Camera camera1;
    private Camera camera2;
    private Camera cameraSelected;
    void Start()
    {
        HealthbarSpriteManager = GameObject.Find("healthbar_SpriteManager").GetComponent<SpriteManager>();
        camera1 = GameObject.Find("PlanetCameraBlue").GetComponent<Camera>();
        camera2 = GameObject.Find("PlanetCameraRed").GetComponent<Camera>();
    }

    void Update()
    {
        if(camera1.enabled == true)
        {
            cameraSelected = camera1.GetComponent<Camera>();
        }
        else if(camera2.enabled == true)
        {
            cameraSelected = camera2.GetComponent<Camera>();
        }

        if(GameObject.Find(transform.parent.gameObject.name) != null)
        {
            Client = GameObject.Find(transform.parent.gameObject.name);
            ClientState = Client.GetComponent<combatState>();
        }
        if (selectManager.units.Contains(Client) || ClientState.currentState == combatState.STATE.InCombat)
        {
            if(myHealthbarSprite != null)
            {
                if(myHealthbarSprite.hidden == true)
                {
                    HealthbarSpriteManager.ShowSprite(myHealthbarSprite);
                    HealthbarSpriteManager.UpdateBounds();
                }
                transform.rotation = cameraSelected.transform.rotation;

                if(transform.parent.transform.localPosition.y < -203f || (transform.parent.transform.localPosition.y < -36f && transform.parent.transform.localPosition.y > -50f))
                {
                    transform.localPosition = new Vector3(
                        0,
                        -2,
                        0
                    );
                }
                else
                {
                    transform.localPosition = Height;
                }

                HealthbarSpriteManager.Transform(myHealthbarSprite);
            }
            else 
            {
                if(myHealthbarSprite.hidden == false)
                {
                     HealthbarSpriteManager.HideSprite(myHealthbarSprite);
                }
            }
        }
        else if(ClientState.currentState == combatState.STATE.OutOfCombat)
        {
            if(myHealthbarSprite.hidden == false)
                HealthbarSpriteManager.HideSprite(myHealthbarSprite);
        }
    
    }
}
