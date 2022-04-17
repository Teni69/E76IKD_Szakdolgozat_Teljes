using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class keyPressed : MonoBehaviour
{
    public string gameName;
    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene(gameName);
        }
    }
}
