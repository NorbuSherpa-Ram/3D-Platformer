using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject gameWinUI;

    [SerializeField] private List<GameObject> uiPanels;



    private void Start()
    {
        GameWinTrigger.OnGameWin += GameWinTrigger_OnGameWin;
    }

    private void OnDestroy()
    {
        GameWinTrigger.OnGameWin -= GameWinTrigger_OnGameWin;
    }

    private void GameWinTrigger_OnGameWin(object sender, EventArgs e)
    {
        SwitchToGameWinPanel();
    }




    private void SwitchToGameWinPanel()
    {
        ChangePanel(gameWinUI);
    }


    public void ChangePanel(GameObject _panel)
    {
        foreach (GameObject panel in uiPanels)
        {
            panel.SetActive(false);
        }

        if (_panel != null)
        {
            _panel.SetActive(true);
        }
    }


}
