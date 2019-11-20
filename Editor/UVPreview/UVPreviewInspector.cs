using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameObject))]
public class UVPreviewInspector : InternalInspector<GameObject>
{
    public override string InternalClassFullName
    {
        get
        {
            return "UnityEditor.GameObjectInspector";
        }
    }

    /// <summary>
    /// UV颜色
    /// </summary>
    private Color m_UvColor = Color.green;

    private string[] m_PreviewModeDesc = new string[] { "模型", "UV1", "UV2", "UV3", "UV4" };
    private int m_PreviewMode;
    [SerializeField] private UVPreview m_UVPreview;
    private List<Texture2D> m_Textures;
    private GUIContent m_TexContent;
    private int m_RendersCount = 0;

    private new void OnEnable()
    {
        base.OnEnable();
        if(Application.isPlaying)
        {
            return;
        }
        try
        {
            m_UVPreview = new UVPreview();
            if (target)
            {
                m_UVPreview.Add((GameObject)target, true);
            }
            m_Textures = CollectTextures((GameObject)target);
            m_TexContent = new GUIContent("贴图");

            Renderer[] renderers = ((GameObject)target).GetComponentsInChildren<Renderer>();
            m_RendersCount = renderers.Length;
        }
        catch(Exception e)
        {

        }
    }

    private new  void OnDisable()
    {
        base.OnDisable();
        if (m_UVPreview != null)
            m_UVPreview.Release();
        m_UVPreview = null;
    }
    public override bool HasPreviewGUI()
    {
        if(base.HasPreviewGUI())
        {
            return true;
        }
        return m_RendersCount > 0;
    }

    public override void DrawPreview(Rect previewArea)
    {
        GUI.Box(new Rect(previewArea.x, previewArea.y, previewArea.width, 17), string.Empty, GUI.skin.FindStyle("toolbar"));

        m_PreviewMode = GUI.Toolbar(new Rect(previewArea.x + 5, previewArea.y, 50 * 4, 17), m_PreviewMode,
            m_PreviewModeDesc, GUI.skin.FindStyle("toolbarbutton"));

        if (m_PreviewMode != 0)
        {
            m_UvColor = EditorGUI.ColorField(
                new Rect(previewArea.x + previewArea.width - 50, previewArea.y + 2, 40, 13),
                m_UvColor);
            if (GUI.Button(new Rect(previewArea.x + previewArea.width - 120, previewArea.y, 70, 17), m_TexContent,
                GUI.skin.FindStyle("ToolbarDropDown")))
            {
                DropDownTextures(new Rect(previewArea.x + previewArea.width - 120, previewArea.y, 70, 17));
            }
            if (GUI.Button(new Rect(previewArea.x + previewArea.width - 190, previewArea.y, 70, 17), "光照贴图",
               GUI.skin.FindStyle("ToolbarDropDown")))
            {
                DropDownLightMaps(new Rect(previewArea.x + previewArea.width - 120, previewArea.y, 70, 17));
            }
        }

        Rect previewRect = new Rect(previewArea.x, previewArea.y + 17, previewArea.width, previewArea.height - 17);
        if (m_PreviewMode == 0)
            this.m_ObjectInspector.DrawPreview(previewRect);
        else
        {
            m_UVPreview.DrawPreview(previewRect, m_UvColor, (UVPreview.UVIndex)(m_PreviewMode - 1), false);
        }
    }

    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
        return this.m_ObjectInspector.RenderStaticPreview(assetPath, subAssets, width, height);
    }
    //从Renderer绑定的材质中读取所有的贴图
    private List<Texture2D> CollectTextures(GameObject target)
    {
        List<Texture2D> result = new List<Texture2D>();
        MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] skinnedMeshRenderers = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        List<Material> mats = new List<Material>();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            if (meshRenderers[i].sharedMaterial)
            {
                if (!mats.Contains(meshRenderers[i].sharedMaterial))
                    mats.Add(meshRenderers[i].sharedMaterial);
            }
        }
        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            if (skinnedMeshRenderers[i].sharedMaterial)
            {
                if (!mats.Contains(skinnedMeshRenderers[i].sharedMaterial))
                    mats.Add(skinnedMeshRenderers[i].sharedMaterial);
            }
        }
        if (mats.Count > 0)
        {
            foreach (var mat in mats)
            {
                var matProperties = MaterialEditor.GetMaterialProperties(new Material[]{ mat });
                for (int i = 0; i < matProperties.Length; i++)
                {
                    if (matProperties[i].type == MaterialProperty.PropType.Texture &&
                        matProperties[i].textureDimension == UnityEngine.Rendering.TextureDimension.Tex2D && matProperties[i].textureValue != null)
                    {
                        result.Add((Texture2D)matProperties[i].textureValue);
                    }
                }
            }
            //MaterialProperty[] matProperties = MaterialEditor.GetMaterialProperties(mats.ToArray());
           
        }
        return result;
    }

    private void DropDownTextures(Rect rect)
    {
        if (m_Textures == null || m_Textures.Count == 0)
            return;
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Empty"), m_TexContent.image == null, ClearTexture);
        menu.AddSeparator("");
        for (int i = 0; i < m_Textures.Count; i++)
        {
            menu.AddItem(new GUIContent(m_Textures[i].name, m_Textures[i]), m_TexContent.image == m_Textures[i],
                SelectTexture, m_Textures[i]);
        }
        menu.DropDown(rect);
    }
    private void DropDownLightMaps(Rect rect)
    {
        if (LightmapSettings.lightmaps.Length > 0)
        {
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < LightmapSettings.lightmaps.Length; i++)
            {
                menu.AddItem(new GUIContent("Index:" + i), false, SetLightMap, new LightMapData(i, false));
                menu.AddItem(new GUIContent("Index:" + i + ",Directional"), false, SetLightMap,
                    new LightMapData(i, true));
            }
            menu.DropDown(rect);
        }
    }
    private void ClearTexture()
    {
        m_UVPreview.ClearTexture();
        m_TexContent = new GUIContent("贴图");
    }
    private void SelectTexture(System.Object texture)
    {
        if (texture == null)
            return;
        Texture2D tex = (Texture2D)texture;
        m_TexContent = new GUIContent(tex.name, tex);
        m_UVPreview.SetTexture(tex);
    }

    private void SetLightMap(object index)
    {
        LightMapData id = (LightMapData)index;
        m_UVPreview.SetLightMap(id.index, id.isDirectional);
    }

    private struct LightMapData
    {
        public int index;
        public bool isDirectional;

        public LightMapData(int index, bool isDirectional)
        {
            this.index = index;
            this.isDirectional = isDirectional;
        }
    }
}
