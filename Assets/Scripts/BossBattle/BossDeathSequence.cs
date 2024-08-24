
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace DungeonCrawler.Assets.Scripts.BossBattle
{
    public class BossDeathSequence : MonoBehaviour
    {
        public void SwitchToCreditsScene() {
            SceneManager.LoadScene("Scenes/Credits");
        }
    }
}