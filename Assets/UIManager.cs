using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{
    public Image hpBar;
    public Image hpBarBackGround;

    /// <summary>
    /// ポーズパネルのボタン
    /// </summary>
    public GameObject pausePanel;
    public Button resume;//復帰
    public Button restart;//はじめから
    public Button option;//オプション
    public Button exit;//終了



    private GameObject player;
    private PlayerController pc;

    private float hpBarReduceSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        hpBar.fillMethod = Image.FillMethod.Horizontal;
        resume.onClick.AddListener(Resume);
        restart.onClick.AddListener(Restart);
        option.onClick.AddListener(Option);
        exit.onClick.AddListener(Exit);
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = pc.CurrentHp / 100;
    }

    private void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        Camera.main.GetComponent<CameraController>().enabled = true;
    }
    private void Restart()
    {

        //Time.timeScale = 1;
        //Camera.main.GetComponent<CameraController>().enabled = true;
        Global.Instance.loadName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadingScene");
    }
    private void Option()
    {

    }
    private void Exit()
    {
        SceneManager.LoadScene(0);
    }

}
