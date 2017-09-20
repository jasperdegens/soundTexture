using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jasper.AdditiveSynth
{
    [RequireComponent(typeof(AudioSource))]
    public class TextureInstrument : MonoBehaviour
    {

        public RenderTexture note;
        bool playNote = false;
        Color[] synthData;

        // Use this for initialization
        void Start()
        {

        }


        Color[] TextureToArr(RenderTexture rendTex)
        {
            Texture2D tex = new Texture2D((int)rendTex.width, (int)rendTex.height, TextureFormat.RGBAHalf, false);
            tex.filterMode = FilterMode.Point;
            RenderTexture.active = rendTex;
            tex.ReadPixels(new Rect(0, 0, rendTex.width, rendTex.height), 0, 0);

            // faster ways to do this but I want it done!
            Color[] vals = new Color[rendTex.width * rendTex.height];
            vals = tex.GetPixels();
            return vals;
        }

        void PlayTexture()
        {
            arrOffset = 0;
            synthData = TextureToArr(note);
            playNote = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayTexture();
            }
        }


        public AdditiveSynth[] synths;
        int arrOffset = 0;
        private void OnAudioFilterRead(float[] data, int channels)
        {
            if (playNote)
            {

                int dataLen = data.Length / channels;

                int n = 0;
                while (n < dataLen && arrOffset < synthData.Length)
                {

                    int j = 0;

                    int colorIndex = arrOffset / 3;
                    int channelOffset = arrOffset % 3;

                    while (j < channels)
                    {
                        data[n * channels + j] += synthData[colorIndex][channelOffset];
                        j++;
                    }

                    arrOffset++;
                    n++;
                }
            }

        }
    }

}