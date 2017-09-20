using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace jasper.AdditiveSynth
{

    public class AdditiveSynth : MonoBehaviour, ISoundTexture
    {

        public float[] Frequencies = new float[1];
        
        // texture resolution will determine length of the sample
        // at 44.1kHz, a 256x256 texture is approximately 1 second
        public Vector2 TextureResolution = new Vector2(256, 256);
        public static readonly string AdditiveSynthShader = "jasper/AdditiveSynth";

        public RenderTexture soundBuffer;

        private Material mat;
        private ComputeBuffer freqBuffer;

        public RenderTexture SoundTexture
        {
            get
            {
                return soundBuffer;
            }
        }   

        // Use this for initialization
        void Start()
        {
            CreateRenderTexture(ref soundBuffer, (int)TextureResolution.x, (int)TextureResolution.y);
            SetupFrequencies();

            mat = new Material(Shader.Find(AdditiveSynthShader));


            UpdateSynth();

        }

        void UpdateSynth()
        {
            SetupFrequencies();
            mat.SetVector("_Resolution", TextureResolution);
            mat.SetBuffer("_FrequencyBuffer", freqBuffer);
            mat.SetInt("_NumFreqs", Frequencies.Length);
            mat.SetInt("_SampleRate", AudioSettings.outputSampleRate);

            Graphics.Blit(null, soundBuffer, mat);

        }

        void RetrieveSynthData()
        {
            Texture2D tex = new Texture2D((int)TextureResolution.x, (int)TextureResolution.y, TextureFormat.RGBAHalf, false);
            tex.filterMode = FilterMode.Point;
            RenderTexture.active = soundBuffer;
            tex.ReadPixels(new Rect(0, 0, soundBuffer.width, soundBuffer.height), 0, 0);

            // faster ways to do this but I want it done!
            Color[] vals = new Color[soundBuffer.width * soundBuffer.height];
            vals = tex.GetPixels();

        }


        void Update()
        {
            UpdateSynth();
        }

        void SetupFrequencies()
        {
            if(freqBuffer != null)
            {
                freqBuffer.Release();
            }
            freqBuffer = new ComputeBuffer(Frequencies.Length, sizeof(float));
            freqBuffer.SetData(Frequencies);
        }

        void CreateRenderTexture(ref RenderTexture rt, int width, int height)
        {
            rt = new RenderTexture(width, height, 0);
            rt.filterMode = FilterMode.Point;
            rt.format = RenderTextureFormat.ARGBHalf; // 16 bits should be enough for sound
            rt.depth = 0;
        }

    }

}
