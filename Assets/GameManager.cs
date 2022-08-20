using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    private GameObject pausePanel;
    private GameObject player;

    // Start is called before the first frame update
    private void Awake()
    {
        AudioManager.MusicClear();
        AudioManager.EffectClear();
    }
    void Start()
    {
        pausePanel = GameObject.Find("PausePanel");
        pausePanel.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");

        AudioManager.MusicPlay("Musics/FutureWorld_Dark_Loop_02", true);
        AudioManager.MusicVolume("Musics/FutureWorld_Dark_Loop_02", 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            
        }
    }

    private void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        //player.GetComponent<PlayerController>().enabled = false;
        Camera.main.GetComponent<CameraController>().enabled = false;
    }
}
