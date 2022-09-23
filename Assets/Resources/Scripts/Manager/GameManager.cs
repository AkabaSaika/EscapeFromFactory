using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject gameClearPanel;
    [SerializeField]
    private GameObject canvas;
    private GameObject player;
    [SerializeField]
    private bool gameClearState;
    


    public UnityAction gameClearHandler;//ゲームクリアのイベントを受け取る

    public bool GameClearState { get => gameClearState;}

    // Start is called before the first frame update
    private void Awake()
    {
        AudioManager.MusicClear();
        AudioManager.EffectClear();
        StageManager.Instance.InitStage();

        gameClearState = false;
    }
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        pausePanel = canvas.transform.Find("PausePanel").gameObject;
        gameOverPanel = canvas.transform.Find("GameOverPanel").gameObject;
        gameClearPanel = canvas.transform.Find("GameClearPanel").gameObject;
        if (pausePanel)
        {
            pausePanel.SetActive(false);
        }
        if(gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }
        if (gameClearPanel)
        {
            gameClearPanel.SetActive(false);
        }

        gameClearHandler += delegate { GameClear(); };
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
        if(player.GetComponent<PlayerController>().CurrentHp<=0)
        {
            PlayerDeath();
        }
        
    }

    private void Pause()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        //player.GetComponent<PlayerController>().enabled = false;
        Camera.main.GetComponent<CameraController>().enabled = false;
    }

    private void PlayerDeath()
    {
        Cursor.visible = true;
        gameOverPanel.SetActive(true);
        Camera.main.GetComponent<CameraController>().enabled = false;
    }

    private void GameClear()
    {
        Cursor.visible = true;
        if (!gameClearState)
        {
            gameClearState = true;
            gameClearPanel.SetActive(true);
        }
        else
        {
            return;
        }
        
    }
    public bool GetPauseState()
    {
        return pausePanel.activeSelf;
    }

}
