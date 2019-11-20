using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if EDITORPLUS
using SeanLib.Core;
#endif
[ExecuteInEditMode]
public class ReplaceLightmap : MonoBehaviour
{
    [Serializable]
    public struct LightmapInfo
    {
        public Texture2D lightmapColor;
        public Texture2D lightmapDir;
        public Texture2D shadowMask;
    }
    [Serializable]
    public struct RendererInfo
    {
        public Renderer renderer;
        public int lightmapIndex;
        public Vector4 lightmapScaleOffset;
    }
    [Serializable]
    public struct ReflectProbeInfo
    {
        public ReflectionProbe Probe;
        public Texture customBakedTexture;
        public Texture bakedTexture;
    }
    public List<RendererInfo> m_RendererInfoList = new List<RendererInfo>();
    public List<LightmapInfo> lightmapDatas=new List<LightmapInfo>();
    /*public List<ReflectProbeInfo> m_ReflectInfoList = new List<ReflectProbeInfo>();*/
    private void Start()
    {
        if(!Application.isPlaying)
        ApplyLightMap();
    }
#if EDITORPLUS
    [InspectorPlus.Button(Editor = true)]
#endif
    public void RecordLightMap()
    {
        lightmapDatas.Clear();
        for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
        {
            var data = LightmapSettings.lightmaps[i];
            lightmapDatas.Add(new LightmapInfo() { lightmapColor = data.lightmapColor, lightmapDir = data.lightmapDir, shadowMask = data.shadowMask });
        }
        m_RendererInfoList.Clear();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            if (r.lightmapIndex != -1)
            {
                RendererInfo info = new RendererInfo();
                info.renderer = r;
                info.lightmapIndex = r.lightmapIndex;
                info.lightmapScaleOffset = r.lightmapScaleOffset;

                m_RendererInfoList.Add(info);
            }
        }
       /* m_ReflectInfoList.Clear();
        ReflectionProbe[] reflects = GetComponentsInChildren<ReflectionProbe>();
        foreach (ReflectionProbe rl in reflects)
        {
            if (rl.bakedTexture||rl.customBakedTexture||rl.texture)
            {
                ReflectProbeInfo info = new ReflectProbeInfo();
                info.bakedTexture = rl.bakedTexture;
                info.customBakedTexture = rl.customBakedTexture;
                m_ReflectInfoList.Add(info);
            }
        }*/
#if UNITY_EDITOR
        UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
#endif
    }
#if EDITORPLUS
    [InspectorPlus.Button(Editor =true)]
#endif
    public void ApplyLightMap()
    {
        LightmapData[] datas = new LightmapData[lightmapDatas.Count];
        for (int i = 0; i < lightmapDatas.Count; i++)
        {
            var data = lightmapDatas[i];
            datas[i] = new LightmapData() { lightmapColor = data.lightmapColor, lightmapDir = data.lightmapDir, shadowMask = data.shadowMask };
        }
        LightmapSettings.lightmaps = datas;

        foreach (RendererInfo info in m_RendererInfoList)
        {
            if (info.renderer != null)
            {
                info.renderer.lightmapIndex = info.lightmapIndex;
                info.renderer.lightmapScaleOffset = info.lightmapScaleOffset;
            }
        }
        /*foreach (var info in m_ReflectInfoList)
        {
            if (info.Probe != null)
            {
                info.Probe.customBakedTexture = info.customBakedTexture;
                info.Probe.bakedTexture = info.bakedTexture;
            }
        }*/
    }
}
