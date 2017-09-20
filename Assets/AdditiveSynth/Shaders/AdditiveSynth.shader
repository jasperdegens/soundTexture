
Shader "jasper/AdditiveSynth"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"
#define PI 3.1415926535


			float2 _Resolution;
			StructuredBuffer<float> _FrequencyBuffer;
			int _SampleRate;
			int _NumFreqs;


			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			
			float CalcFreq(float freq, float time) {
				// freq is cycles per second

				float length = 1 / freq;
				float currPos = time / length;
								
				return sin(2 * PI * currPos);
			}


			float AdditiveSynthesis(float time) 
			{
				float audioOut;
				// additive synthesis -- easy as pi
				for (int i = 0; i < _NumFreqs; i++)
				{
					// find current time step based on sample rate and resultion
					audioOut += CalcFreq(_FrequencyBuffer[i], time);
				}

				audioOut /= (float)_NumFreqs;
				return audioOut;
			}


			// We pack 4 different timestamps into r, g, b channels.
			// Alpha can be used for mix strength or amplitude???
			// each color channel represents an indipendent time
			fixed4 frag (v2f_img i) : SV_Target
			{
				
				
			// time is determined by using xcoord first and then y -> time per row = 
			float pixel = ((i.uv.y * _Resolution.y) * _Resolution.x + i.uv.x * _Resolution.x) * 3; // x3 because each pixel = 3 timestamps
			
			//float baseTime = pixel / (float)_SampleRate;
			

			float3 timeSteps = 0;
			for (int i = 0; i < 3; i++) {
				float t = (pixel + i) / (float)_SampleRate;
				timeSteps[i] = AdditiveSynthesis(t);
			}

			timeSteps = (timeSteps + 1) / 2;

				// quantize between 0 and 1
				return float4(timeSteps, 1);
			}
			ENDCG
		}
	}
}
