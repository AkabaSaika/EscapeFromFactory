using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum GameState
{
    running,pause,over,clear
}
 
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
    public GameState gameState;

    public UnityAction gameClearHandler;//ゲームクリアのイベントを受け取る

    private string screenshotDirPath = "Screenshots/";
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
        gameState = GameState.running;

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
        if(Input.GetKeyDown(KeyCode.F12))
        {
            ScreenShot();
        }
        
    }

    private void Pause()
    {
        gameState = GameState.pause;
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        //player.GetComponent<PlayerController>().enabled = false;
        Camera.main.GetComponent<CameraController>().enabled = false;
    }

    private void PlayerDeath()
    {
        gameState = GameState.over;
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
            Time.timeScale = 0;
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

    private void ScreenShot()
    {
        DirectoryInfo screenshotDirInfo = new DirectoryInfo(screenshotDirPath);
        if(!screenshotDirInfo.Exists)
        {
            Directory.CreateDirectory(screenshotDirPath);
            ScreenCapture.CaptureScreenshot(screenshotDirPath+"/" + "screenshot" + System.DateTime.Now.ToString("yyyymmddhhmmss") + ".png");
        }
        else
        {
            ScreenCapture.CaptureScreenshot(screenshotDirPath+"/" + "screenshot" + System.DateTime.Now.ToString("yyyymmddhhmmss") + ".png");
        }
        
    }
}
