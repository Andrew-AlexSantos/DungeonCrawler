using System.Collections;
using System.Collections.Generic;
using DungeonCrawler.Assets.Scripts;
using UnityEngine;
using EventsArgs;
using UnityEngine.SceneManagement;
using DungeonCrawler.Assets.Scripts.EventArgs;

public class GameOver : MonoBehaviour {

    public GameObject objectToEnable;
    public float gameOverDuration = 6f;
    public bool isTriggered;

    private void Start() {
        GlobalEvent.Instance.OnGameOver += OnGameOver;
    }
    
    private void OnGameOver(object sender, GameOverArgs args) {
        // Debounce
        if (isTriggered) return;
        isTriggered = true;

        // Active object
        objectToEnable.SetActive(true);

        // Schedule scene reload
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene() {
        yield return new WaitForSeconds(gameOverDuration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
