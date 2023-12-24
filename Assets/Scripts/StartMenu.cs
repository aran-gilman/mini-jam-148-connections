using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] SceneField levelOne;

    public void StartGame()
    {
        SceneManager.LoadScene(levelOne.SceneName);
    }
}
