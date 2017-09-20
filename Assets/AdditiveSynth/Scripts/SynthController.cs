using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jasper.AdditiveSynth
{
    [RequireComponent(typeof(AudioSource))]
    public class SynthController : MonoBehaviour
    {

        public AdditiveSynth[] synths;

        private void OnAudioFilterRead(float[] data, int channels)
        {
            int dataLen = data.Length / channels;

            int n = 0;
            while(n < dataLen)
            {

                float synthData = 0;

                for (int i = 0; i < synths.Length; i++)
                {
                  //  synthData += synths[i].GetData();
                }

                int j = 0;
                while (j < channels)
                {
                    data[n * channels + j] += synthData;
                    j++;
                }

                n++;
            }

        }


    }

}