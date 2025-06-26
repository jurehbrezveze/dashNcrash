using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class InvertEffect : MonoBehaviour
{
    public Shader glitchShader;
    private Material glitchMat;
    private bool doGlitch = false;

    public float glitchIntensity = 1.0f;
    public float offsetAmount = 0.01f;
    public float timeSpeed = 10.0f;

    void Start()
    {
        if (glitchShader != null)
            glitchMat = new Material(glitchShader);
    }

    public void TriggerGlitch(float duration)
    {
        StartCoroutine(GlitchForSeconds(duration));
    }

    IEnumerator GlitchForSeconds(float seconds)
    {
        doGlitch = true;
        yield return new WaitForSeconds(seconds);
        doGlitch = false;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (doGlitch && glitchMat != null)
        {
            glitchMat.SetFloat("_Intensity", glitchIntensity);
            glitchMat.SetFloat("_Offset", offsetAmount);
            glitchMat.SetFloat("_TimeSpeed", timeSpeed);
            Graphics.Blit(src, dest, glitchMat);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
