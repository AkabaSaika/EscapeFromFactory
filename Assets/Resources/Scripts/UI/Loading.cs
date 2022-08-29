using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    private AsyncOperation async;

    [SerializeField]
    private GameObject loadPanel;
    [SerializeField]
    private Slider loadingProcess;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }
    void Start()
    {
        StartCoroutine("LoadData");
    }
    

    IEnumerator LoadData()
    {
        async = SceneManager.LoadSceneAsync(Global.Instance.loadName);
        while(!async.isDone)
        {
            var progress = Mathf.Clamp01(async.progress / 0.9f);
            loadingProcess.value = progress;
            yield return null;
        }
        yield return async;
    }
}
