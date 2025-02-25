using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Music;
public class CreditsScreenUI : MonoBehaviour {

    public FadeMusic fadeMusic;

    public void FadeOutMusic() {
        fadeMusic.FadeOut();
    }

    public void SwitchToTitleScreen() {
        SceneManager.LoadScene("Scenes/TitleMenu");
    }
}
