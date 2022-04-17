using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cameramovement : MonoBehaviour
{
    public GameObject planet;
    private Vector3 distanceBetween;
    public Camera map, planetBlueCam, planetRedCam;
    private float scrollAmount;

    void Start() {
        transform.position = planet.transform.position - new Vector3(0, 0, 120);
        //planetBlueCam.transform.LookAt (planet.transform.position);
        map.enabled = false;
        planetRedCam.enabled = false;
    }

    void LateUpdate()
    {
        if(transform.gameObject.GetComponent<Camera>().enabled == true)
        {
            scrollAmount += Input.GetAxis("Mouse ScrollWheel");
            scrollAmount = Mathf.Clamp(scrollAmount, -6f, 6f);
            HandleZoom();
            HandleCameraMovement();
            HandleCamera();
        }
        else scrollAmount = 0f;
    }

    void HandleCameraMovement(){
        /*if(Input.GetKey(KeyCode.A)) {   
            transform.RotateAround(planet.transform.position, Vector3.up, 90 * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.D)) {   
            transform.RotateAround(planet.transform.position, Vector3.down, 90 * Time.deltaTime);
        }*/
        if(Input.GetKey(KeyCode.W)) {
            transform.RotateAround(planet.transform.position, transform.right, 90 * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.S)) {   
            transform.RotateAround(planet.transform.position, -transform.right, 90 * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.Q)) {   
            transform.Rotate(0, 0, 90 * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.E)) {   
            transform.Rotate(0, 0, -90 * Time.deltaTime);
        }    
    }

    void HandleZoom(){
        distanceBetween = planet.transform.position - transform.position;
        if(scrollAmount > 0f && distanceBetween.magnitude >= 55f)
        {
            Vector3 _cam = Vector3.MoveTowards(transform.position, planet.transform.position, 10f);
            transform.position = Vector3.Lerp(transform.position, _cam, scrollAmount * Time.deltaTime);
            if(Input.mouseScrollDelta.y < 0) scrollAmount = 0f;
        }     
        if(scrollAmount < 0f && distanceBetween.magnitude <= 140f)
        {
            Vector3 _cam = Vector3.MoveTowards(transform.position, planet.transform.position, -10f);
            transform.position = Vector3.Lerp(transform.position, _cam, -1*(scrollAmount * Time.deltaTime));
            if(Input.mouseScrollDelta.y > 0) scrollAmount = 0f;
        }
        else if(distanceBetween.magnitude > 140f || distanceBetween.magnitude <55f)
        {
            scrollAmount = 0f;
        }
    }

    void HandleCamera(){
        /*if(Input.GetKeyDown(KeyCode.M))
        {
            map.enabled = true; planetBlueCam.enabled = false; planetRedCam.enabled = false;
        }*/
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
           planetBlueCam.enabled = true; planetRedCam.enabled = false; map.enabled = false;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            planetRedCam.enabled = true; planetBlueCam.enabled = false; map.enabled = false;
        }
    }
}
