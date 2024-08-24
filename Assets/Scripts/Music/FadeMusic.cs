using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music {
public class FadeMusic : MonoBehaviour
{
        private bool isFadingOut;

        public void FadeOut()
        {

            // Debounce
            if (isFadingOut) return;
            isFadingOut = true;
        }
}
}