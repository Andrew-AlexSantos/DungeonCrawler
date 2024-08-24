using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace DungeonCrawler.Assets.Scripts
{
    public static class FadeAudioSource {
        public static IEnumerator StartFade(AudioSource audioSource, float targetVolume, float duration) {
            float currentTime = 0;
            float start = audioSource.volume;
            while (currentTime < duration) {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
        }
    }
}