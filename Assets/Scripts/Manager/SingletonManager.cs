using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonManager : MonoBehaviour
{
    /// <summary>
    /// プログラム開始の時点で各Managerを初期化する
    /// </summary>
    void Start()
    {
        AudioManager.Instance.Init();
        GameManager.Instance.Init();
        UIManager.Instance.Init();
    }
}
