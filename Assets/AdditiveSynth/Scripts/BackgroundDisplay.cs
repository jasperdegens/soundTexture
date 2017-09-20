using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jasper.AdditiveSynth
{

    public class BackgroundDisplay : MonoBehaviour
    {


        public SynthMixer texToDisplay;
        public Material backgroundMat;

        void Start()
        {
            ResizeToScreen();
        }

        void ResizeToScreen()
        {
            transform.position = new Vector3(0, 0, 50);

            transform.localScale = new Vector3(1, 1, 1);


            float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            transform.localScale = new Vector3(worldScreenWidth, worldScreenHeight, 1);

        }

        // Update is called once per frame
        void Update()
        {
            backgroundMat.mainTexture = texToDisplay.SoundTexture;
        }
    }


}