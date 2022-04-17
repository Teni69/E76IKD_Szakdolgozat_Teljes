using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour
{
    public string gameName;
    public void loadGame()
    {
        SceneManager.LoadScene(gameName);
    }
}
