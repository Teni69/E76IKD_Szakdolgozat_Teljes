using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class selectManager : MonoBehaviour
{
    //
    Texture2D _borderTexture;
    Texture2D borderTexture
    {
        get
        {
            if (_borderTexture == null)
            {
                _borderTexture = new Texture2D(1, 1);
                _borderTexture.SetPixel(0, 0, Color.white);
                _borderTexture.Apply();
            }

            return _borderTexture;
        }
    }

    private Camera camera1;
    private Camera camera2;
    private Camera cameraSelected;

    bool selectionStarted = false;
    Vector3 mousePosition1;

    public static List<SC_Selectable> selectables = new List<SC_Selectable>();

    public static List<GameObject> units = new List<GameObject>();

    public string selectableTag = "Selectable";
    public Material selectedMaterial;
    public Material defaultMaterial;
    private Transform _selection;
    public static bool isSelected;

    void Start()
    {
        camera1 = GameObject.Find("PlanetCameraBlue").GetComponent<Camera>();
        camera2 = GameObject.Find("PlanetCameraRed").GetComponent<Camera>();
        cameraSelected = Camera.main;
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

        if(_selection != null && isSelected == false)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }
 
        Ray ray = cameraSelected.ViewportPointToRay(cameraSelected.ScreenToViewportPoint(Input.mousePosition));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if(Input.GetMouseButtonDown(0) && selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if(selectionRenderer != null && isSelected == false)
                {
                    selectionRenderer.material = selectedMaterial;
                }
                if(Input.GetMouseButton(0) && selection.CompareTag(selectableTag))
                {
                    selectionRenderer.material = selectedMaterial;
                    isSelected = true;
                    _selection = selection;
                    if(!units.Contains(_selection.gameObject))
                    {
                        units.Add(_selection.gameObject);
                    }
                }
            }
            else if(Input.GetMouseButton(0) && !selection.CompareTag(selectableTag) || Input.GetKey(KeyCode.Escape))
            {
                units.Clear();
                isSelected = false;
                MakeDefaultMaterial();
            }
        }

        // Begin selection
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            selectionStarted = true;
            mousePosition1 = Input.mousePosition;
        }
        // End selection
        if (Input.GetMouseButtonUp(0))
        {
            selectionStarted = false;
        }

        if (selectionStarted)
        {
            // Detect which Objects are inside selection rectangle
            MakeDefaultMaterial();
            units.Clear();
            isSelected = false;
            for (int i = 0; i < selectables.Count; i++)
            {
                Bounds viewportBounds = GetViewportBounds(cameraSelected, mousePosition1, Input.mousePosition);
                if (viewportBounds.Contains(cameraSelected.WorldToViewportPoint(selectables[i].transform.position)))
                {
                    var selection = selectables[i].transform;
                    if(selection.CompareTag(selectableTag))
                    {
                        var selectionRenderer = selection.GetComponent<Renderer>();
                        if(selectionRenderer != null && isSelected == false)
                        {
                            selectionRenderer.material = selectedMaterial;
                        }
                        selectionRenderer.material = selectedMaterial;
                        isSelected = true;
                        _selection = selection;
                        if(!units.Contains(_selection.gameObject))
                        {
                            units.Add(_selection.gameObject);
                        }
                    }
                }
            }
        }
    }

    void MakeDefaultMaterial(){
         for (int i = 0; i < selectables.Count; i++)
            {
                var selectionRenderer = selectables[i].GetComponent<Renderer>();
                selectionRenderer.material = defaultMaterial;
            }
    }

    //
    void OnGUI()
    {
        if (selectionStarted)
        {
            Rect rect = GetScreenRect(mousePosition1, Input.mousePosition);
            DrawScreenRectBorder(rect, 2, Color.green);
        }
    }

    void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        DrawBorderRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        DrawBorderRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        DrawBorderRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        DrawBorderRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    void DrawBorderRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, borderTexture);
        GUI.color = Color.white;
    }

    Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        Vector3 v1 = camera.ScreenToViewportPoint(screenPosition1);
        Vector3 v2 = camera.ScreenToViewportPoint(screenPosition2);
        Vector3 min = Vector3.Min(v1, v2);
        Vector3 max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }
    //
}
