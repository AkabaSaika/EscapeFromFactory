using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : ButtonBase
{
    protected override void PushDownAction(GameObject currPanel, GameObject nextPanel)
    {
        currPanel.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1;
        Camera.main.GetComponent<CameraController>().enabled = true;
        GameManager.Instance.gameState = GameState.running;
    }
}
