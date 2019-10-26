Shader "Hidden/PostShader"
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
		GLSLPROGRAM

			#ifdef VERTEX

				varying vec2 UV;

				void main() {
					gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
					UV = (gl_TextureMatrix[0] * gl_MultiTexCoord0).xy;
				}

			#endif
			#ifdef FRAGMENT

				uniform sampler2D _MainTex;
				uniform vec4 Resolution;
				varying vec2 UV;

				vec3 brightnessContrast(vec3 value, float brightness, float contrast) {
					return (value - 0.5) * contrast + 0.5 + brightness;
				}

				vec3 tonemapFunc(const vec3 x) {
					const float A = 0.15;
					const float B = 0.50;
					const float C = 0.10;
					const float D = 0.20;
					const float E = 0.02;
					const float F = 0.30;
					return ((x * (A * x + C * B) + D * E) / (x * (A * x + B) + D * F)) - E / F;
				}

				vec3 tonemap(const vec3 color) {
					const float W = 3.2;
					const float exposureBias = 2.0;

					vec3 curr = tonemapFunc(exposureBias * color);
					vec3 whiteScale = 1.0 / tonemapFunc(vec3(W));

					return brightnessContrast(curr * whiteScale, 0.05, 1.5);
				}

				vec2 pixelate(vec2 uv, float amount) {
					if (amount <= 0)
						return uv;

					float d = 1.0 / amount;
					float ar = Resolution.x / Resolution.y;

					float u = floor(uv.x / d) * d + (ar / Resolution.x);
					d = ar / amount;

					float v = floor(uv.y / d) * d + (ar / Resolution.y);

					return vec2(u, v);
				}

				void main() {
					const float pix_amt = 0; // default 300

					vec4 InColor = texture2D(_MainTex, pixelate(UV, pix_amt));
					vec3 ColorRGB = InColor.xyz;
					float ColorAlpha = InColor.w;

					ColorRGB = tonemap(ColorRGB);
					
					gl_FragColor = vec4(ColorRGB, ColorAlpha);
				}

			#endif
           
			ENDGLSL
        }
    }
}
