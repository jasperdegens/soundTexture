using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace jasper.AdditiveSynth
{

    public class SynthMixer : MonoBehaviour, ISoundTexture
    {

        public List<ISoundTexture> synths = new List<ISoundTexture>();
        public AdditiveSynth mainSynth;
        public RenderTexture mixedTexture;

        public RenderTexture SoundTexture
        {
            get
            {
                return mainSynth.SoundTexture;
            }
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }

}