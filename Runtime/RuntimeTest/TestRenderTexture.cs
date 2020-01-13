using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRenderTexture : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ARGB1555   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB1555));
        Debug.Log("ARGB2101010   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB2101010));
        Debug.Log("ARGB32   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32));
        Debug.Log("ARGB4444   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB4444));
        Debug.Log("ARGBARGB6432   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB64));
        Debug.Log("ARGBFloat   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat));
        Debug.Log("ARGBHalf   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf));
        Debug.Log("ARGBInt   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBInt));
        Debug.Log("BGR101010_XR   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.BGR101010_XR));
        Debug.Log("BGRA10101010_XR   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.BGRA10101010_XR));
        Debug.Log("BGRA32   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.BGRA32));
        Debug.Log("Default   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Default));
        Debug.Log("DefaultHDR   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.DefaultHDR));
        Debug.Log("Depth   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth));
        Debug.Log("R16   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R16));
        Debug.Log("R8   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8));
        Debug.Log("RFloat   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RFloat));
        Debug.Log("RG16   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RG16));
        Debug.Log("RG32   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RG32));
        Debug.Log("RGB111110Float   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGB111110Float));
        Debug.Log("RGB565   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGB565));
        Debug.Log("RGBAUShort   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGBAUShort));
        Debug.Log("RGFloat   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGFloat));
        Debug.Log("RGHalf   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf));
        Debug.Log("RGInt   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGInt));
        Debug.Log("RHalf   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RHalf));
        Debug.Log("RInt   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RInt));
        Debug.Log("Shadowmap   " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Shadowmap));
    }

}
