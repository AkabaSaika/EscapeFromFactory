using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    int count = 0;

    public UnityAction gameClearHandler;//ゲームクリアのイベントを受け取る

    public bool GameClearState { get => gameClearState;}

    /// <summary>
    /// シーンの読み込みが終わったら初期化を行う。
    /// Loading.csで呼び出される。
    /// </summary>
    public void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().buildIndex>1)
        {
            //敵を初期化する
            StageManager.Instance.InitStage();
            gameClearState = false;

            //UIの初期化
            canvas = GameObject.Find("Canvas");
            pausePanel = canvas.transform.Find("PausePanel").gameObject;
            gameOverPanel = canvas.transform.Find("GameOverPanel").gameObject;
            gameClearPanel = canvas.transform.Find("GameClearPanel").gameObject;
            if (pausePanel)
            {
                pausePanel.SetActive(false);
            }
            if (gameOverPanel)
            {
                gameOverPanel.SetActive(false);
            }
            if (gameClearPanel)
            {
                gameClearPanel.SetActive(false);
            }

            gameClearHandler += delegate { GameClear(); };
            player = GameObject.FindGameObjectWithTag("Player");

            //オーディオの初期化

            AudioManager.Instance.MusicClear();
            AudioManager.Instance.EffectClear();
            AudioManager.Instance.Init();
            AudioManager.Instance.MusicPlay("Musics/FutureWorld_Dark_Loop_02", true);
            AudioManager.Instance.MusicVolume("Musics/FutureWorld_Dark_Loop_02", 0.4f);

            Debug.Log(count);
            count++;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        else
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        

    }

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex>1)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();

            }
            if (player.GetComponent<PlayerController>().CurrentHp <= 0)
            {
                PlayerDeath();
            }
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
            //クリアパネルを表示する
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
