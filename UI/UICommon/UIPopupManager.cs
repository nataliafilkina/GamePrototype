using GameSystems;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupManager : MonoBehaviour
{
    public event Action OnOpenedModalWindow;
    public event Action OnCloseAllModalWindow;

    [SerializeField]
    private GameObject _inventory;

    [SerializeField]
    private GameObject _pauseMenu;

    [SerializeField]
    private GameObject _upgrateTree;

    private GameObject _headActiveChain = null;

    private Stack<GameObject> _chainOpenWindow = new();

    private void Start()
    {
        _inventory.SetActive(false);
        _pauseMenu.SetActive(false);
        _upgrateTree.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
            OnPressButton(_inventory);

        if(Input.GetKeyUp(KeyCode.L)) 
            OnPressButton(_upgrateTree);


        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (_chainOpenWindow.Count > 0)
                CloseTopWindow();
            else
                OpenWindow(_pauseMenu);
        }
    }

    private void OnPressButton(GameObject window)
    {
        if (_headActiveChain != _pauseMenu)
        {
            var isOpening = _headActiveChain != window;

            if (_headActiveChain != null)
                CloseAllWindow();

            if (isOpening)
                OpenWindow(window);
        }
    }

    private void CloseAllWindow()
    {
        if (_chainOpenWindow.Count > 0)
        {
            _chainOpenWindow.Peek().SetActive(false);
            _chainOpenWindow.Clear();
        }

        _headActiveChain = null;
        PauseControl.SetPauseGame(false);

        OnCloseAllModalWindow?.Invoke();
    }

    public void OpenWindow(GameObject window)
    {
        if (_chainOpenWindow.Count == 0)
        {
            PauseControl.SetPauseGame(true);
            _headActiveChain = window;

            OnOpenedModalWindow?.Invoke();
        }
        else
            _chainOpenWindow.Peek().SetActive(false);

        window.SetActive(true);
        _chainOpenWindow.Push(window);
    }

    public void CloseTopWindow()
    {
        var countOpenedWindow = _chainOpenWindow.Count;

        if (countOpenedWindow > 1)
        {
            _chainOpenWindow.Pop().SetActive(false);
            _chainOpenWindow.Peek().SetActive(true);
        }
        else
            if (_chainOpenWindow.Count > 0)
            CloseAllWindow();
    }
}
