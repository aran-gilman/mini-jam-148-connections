using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WinAndLose : MonoBehaviour
{
    [SerializeField] GameObject _battlefield;
    [SerializeField] int NumberOfPortals;
    [SerializeField] GameObject _winScreen;
    [SerializeField] GameObject _loseScreen;
    [SerializeField] UnityEngine.InputSystem.PlayerInput _playerInput;
    [SerializeField] SceneField nextLevel;
    bool _win;

    public void Lose()
    {
        _battlefield.SetActive(false);
        _loseScreen.SetActive(true);
        _playerInput.SwitchCurrentActionMap("End Screen");
    }

    public void Win()
    {
        _battlefield.SetActive(false);
        _winScreen.SetActive(true);
        _playerInput.SwitchCurrentActionMap("End Screen");
        _win = true;
    }

    public void PortalDestroyed()
    {
        NumberOfPortals--;
        if(NumberOfPortals <= 0)
        {
            Win();
        }
    }

    public void EndScene()
    {
        if(_win)
        {
            SceneManager.LoadScene(nextLevel.SceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
