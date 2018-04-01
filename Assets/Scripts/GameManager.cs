using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject endUI;
    public Text endMsg;
    public static GameManager instance;
    private EnemySpawner enemySpawner;

    void Awake()
    {
        instance = this;
        enemySpawner = GetComponent<EnemySpawner>();
    }

    public void Win()
    {
        endUI.SetActive(true);
        endMsg.text = "WIN !!";
    }

    public void Lose()
    {
        enemySpawner.Stop();
        endUI.SetActive(true);
        endMsg.text = "GAME   OVER";
    }

    public void OnButtonRetry()
    {
        //SceneManager.GetActiveScene().buildIndex
        SceneManager.LoadScene(1);
    }

    public void OnButtonMenu()
    {
        SceneManager.LoadScene(0);
    }
}
