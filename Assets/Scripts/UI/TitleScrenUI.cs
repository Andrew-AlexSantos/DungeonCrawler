using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class TitleScrenUI : MonoBehaviour {

        public Animator thisAnimator;
        public float fadeOutDuration;

        private bool isFadingOut;

        public void FadeOut() {

            // Debounce
            if (isFadingOut) return;
            isFadingOut = true;

            // begin animator
            thisAnimator.enabled = true;

            // Schedule switch
            StartCoroutine(TransitionToNextScene());
        }

        private IEnumerator TransitionToNextScene() {
            yield return new WaitForSeconds(fadeOutDuration);

            SceneManager.LoadScene("SampleScene");
        }
    }
}