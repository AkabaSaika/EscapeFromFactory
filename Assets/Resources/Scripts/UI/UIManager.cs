using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{
    public Image hpBar;
    public Image hpBarBackGround;

    [SerializeField]
    private Button newGame;
    [SerializeField]
    private Button exitGame;


    /// <summary>
    /// ポーズパネルのボタン
    /// </summary>
    public GameObject pausePanel;
    public Button resume;//復帰
    public Button restart;//はじめから
    public Button option;//オプション
    public Button exit;//終了


    [SerializeField]
    private GameObject optionPanel;
    [SerializeField]
    private TMPro.TMP_Dropdown resolution;
    [SerializeField]
    private Button back;

    [SerializeField]
    private GameObject previousPanel;
    [SerializeField]
    private GameObject currentPanel;

    private GameObject player;
    private PlayerController pc;

    private float hpBarReduceSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("StartMenu"))
        {

            newGame.onClick.AddListener(NewGame);
            
            option.onClick.AddListener(Option);
            ButtonEffect(option, "Audio/UI_Buttons_Pack2/Button_15_Pack2");
            exitGame.onClick.AddListener(Exit);
            ButtonEffect(exitGame, "Audio/UI_Buttons_Pack2/Button_15_Pack2");
            resolution.onValueChanged.AddListener(ResolutionChange.SetResolution);
            back.onClick.AddListener(delegate { Back(optionPanel); });
            ButtonEffect(back, "Audio/UI_Buttons_Pack2/Button_15_Pack2");
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            pc = player.GetComponent<PlayerController>();
            hpBar.fillMethod = Image.FillMethod.Horizontal;
            resume.onClick.AddListener(Resume);
            restart.onClick.AddListener(Restart);
            option.onClick.AddListener(Option);
            exit.onClick.AddListener(Exit);

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("StartMenu"))
        {
            
        }
        else
        {
            hpBar.fillAmount = pc.CurrentHp / 100;
        }
        
    }

    private void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        Camera.main.GetComponent<CameraController>().enabled = true;
    }
    private void Restart()
    {
        Global.Instance.LoadNextScene(SceneManager.GetActiveScene().name);
    }
    private void Option()
    {
        optionPanel.SetActive(true);
    }
    private void Exit()
    {
        Application.Quit();
    }
    private void NewGame()
    {
        Global.Instance.LoadNextScene("SampleScene");
    }
    private void Back(GameObject panel)
    {
        panel.SetActive(false);
    }
    private void ButtonEffect(Button button,string path)
    {
        button.onClick.AddListener(delegate { AudioManager.EffectPlay(path, false); });
        
    }
}
