using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Global : MonoSingleton<Global>
{
    public string loadName;
    public void LoadNextScene(string name)
    {
        loadName = name;
        SceneManager.LoadScene("LoadingScene");
        
    }
    private void Awake()
    {
        Screen.SetResolution(1699, 900, false);
    }
}
