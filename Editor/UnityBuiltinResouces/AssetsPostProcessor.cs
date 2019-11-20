using UnityEditor;
using UnityEngine;

public class AssetsPostProcessor : AssetPostprocessor
{
    private uint _version = 0;

    public override uint GetVersion() { return _version; }

    /// <summary>
    /// This is called after importing of any number of assets is complete (when the Assets progress bar has reached the end).
    /// This call can occur after a manual reimport, or any time you move an asset or folder of assets to a new location in the Project View.All string arrays are filepaths relative to the Project's root Assets folder. importedAssets contains paths of all assets used in the operation. Each consecutive index of movedAssets and movedFromAssetPaths will always refer to the same asset.
    /// If you perform a bulk operation on several individual assets instead of a folder containing those assets, this function will be called once per asset with each individual asset as the only item in the various arrays.
    /// Note that this function must be declared as static, that is it will not be called correctly if it is declared as an instance function.
    /// The order specified by GetPostprocessOrder does not affect this function.
    /// </summary>
    /// <param name="importedAssets"></param>
    /// <param name="deletedAssets"></param>
    /// <param name="movedAssets"></param>
    /// <param name="movedFromAssetPaths"></param>
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="material"></param>
    /// <param name="renderer"></param>
    Material OnAssignMaterialModel(Material material, Renderer renderer)
    {
        if (material.shader == Shader.Find("Standard") || material.shader == null)
        {
            var assetPaths = AssetDatabase.FindAssets("ClearBuiltinResouces_Default t:Material");
            var assetPath = AssetDatabase.GUIDToAssetPath(assetPaths[0]);
            Material defaultMaterial = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
            return defaultMaterial;
        }
        return material;
    }

    /// <summary>
    /// Handler called when asset is assigned to a different asset bundle.
    /// Add this function to a subclass to get a notification after the assetBundle name for the asset changes.
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="previousAssetBundleName"></param>
    /// <param name="newAssetBundleName"></param>
    void OnPostprocessAssetbundleNameChanged(string assetPath, string previousAssetBundleName, string newAssetBundleName)
    {

    }

    /// <summary>
    /// Add this function to a subclass to get a notification when an audio clip has completed importing.
    /// </summary>
    /// <param name="audioClip"></param>
    void OnPostprocessAudio(AudioClip audioClip)
    {

    }

    /// <summary>
    /// Add this function to a subclass to get a notification just before a cubemap texture has completed importing.
    /// If the cubemap texture is modified, it needs to be readable, as shown in the example below.Set the isReadable flag to True in the Importer settings of the Editor(Read/Write Enabled). If the cubemap texture does not have to be readable at runtime, use texture.Apply(true, true) to update the mipmaps and make the cubemap texture unreadable at runtime.
    /// </summary>
    /// <param name="texture"></param>
    void OnPostprocessCubemap(Cubemap texture)
    {

    }

    /// <summary>
    /// Gets called for each GameObject that had at least one user property attached to it in the imported file.
    /// The second argument string array(propNames) contains all the names of the properties found.The System.Object array (values) contains all the actual values. These can be of type string, Vector4, bool, Color, float, int.
    /// A typical use case for this feature is reading out "userdata" stored on objects in 3dmax/maya.Depending on what is written in the text userdata for an object, you could decide to postprocess your GameObject in different ways.
    /// For a detailed description of the stage when the function is invoked see AssetPostprocessor.
    /// Please note that the GameObjects and Meshes only exist during the import and will be destroyed immediately afterwards. This function is called before the final prefab is created and before it is written to disk, thus you have full control over the generated game objects and components. Any references to game objects or meshes will become invalid after the import has been completed. Thus it is not possible to create a new prefab in a different file from OnPostprocessGameObjectWithUserProperties that references meshes in the imported fbx file.The sample JS example below adds a simple BoxCollider component if the userdata string contains "addboxcollider". The c# version is similar.
    /// </summary>
    /// <param name="go"></param>
    /// <param name="propNames"></param>
    /// <param name="values"></param>
    void OnPostprocessGameObjectWithUserProperties(GameObject go, string[] propNames, System.Object[] values)
    {

    }

    /// <summary>
    /// Add this function to a subclass to get a notification when a Material asset has completed importing.
    /// Use this method to modify the properties of newly created Material assets during import.
    /// </summary>
    /// <param name="material"></param>
    void OnPostprocessMaterial(Material material)
    {

    }

    /// <summary>
    /// Add this function to a subclass to get a notification when a model has completed importing.
    /// This lets you modify the imported Game Object, Meshes, AnimationClips referenced by it.Please note that the GameObjects, AnimationClips and Meshes only exist during the import and will be destroyed immediately afterwards.
    /// This function is called before the final prefab is created and before it is written to disk, thus you have full control over the generated game objects and components.
    /// Any references to game objects or meshes will become invalid after the import has been completed. Thus it is not possible to create a new prefab in a different file from OnPostprocessModel that references meshes in the imported fbx file.
    /// root is the root game object of the imported model.
    /// </summary>
    /// <param name="gameObject"></param>
    void OnPostprocessModel(GameObject gameObject)
    {

    }

    /// <summary>
    /// Add this function to a subclass to get a notification when a SpeedTree asset has completed importing.
    /// This function behaves much like OnPostprocessModel where modifications are allowed on the final imported prefab before being saved on the disk.
    /// </summary>
    /// <param name=""></param>
    void OnPostprocessSpeedTree(GameObject gameObject)
    {

    }

    /// <summary>
    /// Add this function to a subclass to get a notification when an texture of sprite(s) has completed importing.
    /// For Multiple sprite-mode assets each sprite will be passed in the second argument as an array of sprites.
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="sprites"></param>
    void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
    {

    }

    /// <summary>
    /// Add this function to a subclass to get a notification when a texture has completed importing just before.
    /// The texture is optionally compressed and saved to disk.
    /// At this point it is too late to choose compression format, it is still possible to compress the texture using texture.Compress but this is not adviced and the compression format will not be displayed in the editor. Use OnPreprocessTexture if you wish to change compression format based on filename or other attributes of the texture.
    /// If the texture is modified as in the example below it is required to be readable. The flag isReadable must to set True in importer settings either from the editor(Read/Write Enabled) or in the OnPreprocessTexture function. If the texture does not have to be readable at runtime use texture.Apply(true, true) to update the mipmaps and make the texture unreadable at runtime.
    /// </summary>
    /// <param name="texture"></param>
    void OnPostprocessTexture(Texture2D texture)
    {

    }

    /// <summary>
    /// Add this function to a subclass to get a notification just before animation from a model (.fbx, .mb file etc.) is imported.
    /// This lets you control the import settings through code.
    /// </summary>
    void OnPreprocessAnimation()
    {

    }

    /// <summary>
    /// Add this function to a subclass to get a notification just before any Asset is imported.
    /// This lets you control the import settings through code.
    /// </summary>
    void OnPreprocessAsset()
    {

    }

    /// <summary>
    /// Add this function to a subclass to get a notification just before an audio clip is being imported.
    ///This lets you control the import settings trough code.
    /// </summary>
    void OnPreprocessAudio()
    {

    }


    /// <summary>
    /// Add this function to a subclass to get a notification just before a model (.fbx, .mb file etc.) is imported.
    /// This lets you control the import settings through code.
    /// </summary>
    void OnPreprocessModel()
    {
        ModelImporter importer = assetImporter as ModelImporter;

        DisableMaterialImport(importer);
    }

    /// <summary>
    /// Add this function to a subclass to get a notification just before a SpeedTree asset (.spm file) is imported.
    /// This function behaves much like OnPreprocessModel where you can control the SpeedTree asset import settings through code.
    /// </summary>
    void OnPreprocessSpeedTree()
    {

    }

    /// <summary>
    /// Add this function to a subclass to get a notification just before the texture importer is run.
    /// This lets you setup default values for the import settings.
    /// Use textureImporter.isReadable to make the texture readable in OnPostprocessTexture if you wish to change the texture data eg. to do premultiplication of alpha.
    /// Should you want to change compression format of the texture this is where it should be done.
    /// </summary>
    void OnPreprocessTexture()
    {
       /* //Actor
        if (assetPath.StartsWith("Assets/Products/Resources/Actors/"))
        {
            TextureImporter textureImporter = assetImporter as TextureImporter;
            if (textureImporter != null)
            {
                textureImporter.textureType = TextureImporterType.Default;
                textureImporter.maxTextureSize = 1024;
                textureImporter.mipmapEnabled = true;
                textureImporter.alphaIsTransparency = false;
                textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
                textureImporter.textureCompression = TextureImporterCompression.CompressedHQ;
                textureImporter.crunchedCompression = true;

                TextureImporterPlatformSettings tipsandroid = new TextureImporterPlatformSettings();
                tipsandroid.name = "Android";
                tipsandroid.maxTextureSize = 1024;
                tipsandroid.format = textureImporter.DoesSourceTextureHaveAlpha() ? TextureImporterFormat.ETC2_RGBA8 : TextureImporterFormat.ETC2_RGB4;
                tipsandroid.compressionQuality = 50;
                tipsandroid.allowsAlphaSplitting = true;
                tipsandroid.overridden = true;
                tipsandroid.crunchedCompression = true;
                tipsandroid.textureCompression = TextureImporterCompression.CompressedHQ;
                textureImporter.SetPlatformTextureSettings(tipsandroid);

                TextureImporterPlatformSettings tipsios = new TextureImporterPlatformSettings();
                tipsios.name = "iPhone";
                tipsios.maxTextureSize = 1024;
                tipsios.format = TextureImporterFormat.ASTC_4x4;//textureImporter.DoesSourceTextureHaveAlpha() ? TextureImporterFormat.ASTC_RGBA_4x4 : TextureImporterFormat.ASTC_RGB_4x4;
                tipsios.compressionQuality = 50;
                tipsios.allowsAlphaSplitting = true;
                tipsios.overridden = true;
                tipsios.crunchedCompression = true;
                tipsios.textureCompression = TextureImporterCompression.CompressedHQ;
                textureImporter.SetPlatformTextureSettings(tipsios);

            }
        }
        //UI
        if (assetPath.StartsWith("Assets/Products/Resources/UI/")
            || assetPath.StartsWith("Assets/Products/Resources/UIAtlas/")
            || assetPath.StartsWith("Assets/Products/Resources/UIDyAtlas/"))
        {
            TextureImporter textureImporter = assetImporter as TextureImporter;
            if (textureImporter != null)
            {
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.maxTextureSize = 2048;
                textureImporter.mipmapEnabled = false;
                textureImporter.alphaIsTransparency = textureImporter.DoesSourceTextureHaveAlpha();
                textureImporter.anisoLevel = 0;
                textureImporter.npotScale = TextureImporterNPOTScale.None;
                textureImporter.crunchedCompression = true;
                textureImporter.textureCompression = TextureImporterCompression.CompressedHQ;

                TextureImporterPlatformSettings tipsandroid = new TextureImporterPlatformSettings();
                tipsandroid.name = "Android";
                tipsandroid.maxTextureSize = 2048;
                tipsandroid.format = textureImporter.DoesSourceTextureHaveAlpha() ? TextureImporterFormat.ETC2_RGBA8 : TextureImporterFormat.ETC2_RGB4;
                tipsandroid.compressionQuality = 50;
                tipsandroid.allowsAlphaSplitting = true;
                tipsandroid.overridden = true;
                tipsandroid.crunchedCompression = true;
                tipsandroid.textureCompression = TextureImporterCompression.CompressedHQ;
                textureImporter.SetPlatformTextureSettings(tipsandroid);

                TextureImporterPlatformSettings tipsios = new TextureImporterPlatformSettings();
                tipsios.name = "iPhone";
                tipsios.maxTextureSize = 2048;
                tipsios.format = TextureImporterFormat.ASTC_4x4;//textureImporter.DoesSourceTextureHaveAlpha() ? TextureImporterFormat.ASTC_RGBA_4x4 : TextureImporterFormat.ASTC_RGB_4x4;
                tipsios.compressionQuality = 50;
                tipsios.allowsAlphaSplitting = true;
                tipsios.overridden = true;
                tipsios.crunchedCompression = true;
                tipsios.textureCompression = TextureImporterCompression.CompressedHQ;
                textureImporter.SetPlatformTextureSettings(tipsios);

            }
        }
        if (assetPath.StartsWith("Assets/Products/Resources/UISpine"))
        {
            TextureImporter textureImporter = assetImporter as TextureImporter;
            if (textureImporter != null)
            {
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.maxTextureSize = 2048;
                textureImporter.spriteImportMode = SpriteImportMode.Single;
                textureImporter.mipmapEnabled = false;
                textureImporter.alphaIsTransparency = false;
                textureImporter.anisoLevel = 0;
                textureImporter.npotScale = TextureImporterNPOTScale.None;
                textureImporter.textureCompression = TextureImporterCompression.CompressedHQ;
                textureImporter.crunchedCompression = true;

                TextureImporterPlatformSettings tipsandroid = new TextureImporterPlatformSettings();
                tipsandroid.name = "Android";
                tipsandroid.maxTextureSize = 2048;
                tipsandroid.format = textureImporter.DoesSourceTextureHaveAlpha() ? TextureImporterFormat.ETC2_RGBA8 : TextureImporterFormat.ETC2_RGB4;
                tipsandroid.compressionQuality = 50;
                tipsandroid.allowsAlphaSplitting = true;
                tipsandroid.overridden = true;
                tipsandroid.textureCompression = TextureImporterCompression.CompressedHQ;
                tipsandroid.crunchedCompression = true;
                textureImporter.SetPlatformTextureSettings(tipsandroid);

                TextureImporterPlatformSettings tipsios = new TextureImporterPlatformSettings();
                tipsios.name = "iPhone";
                tipsios.maxTextureSize = 2048;
                tipsios.format = TextureImporterFormat.ASTC_4x4;//textureImporter.DoesSourceTextureHaveAlpha() ? TextureImporterFormat.ASTC_RGBA_4x4 : TextureImporterFormat.ASTC_RGB_4x4;
                tipsios.compressionQuality = 50;
                tipsios.allowsAlphaSplitting = true;
                tipsios.overridden = true;
                tipsios.textureCompression = TextureImporterCompression.CompressedHQ;
                tipsios.crunchedCompression = true;
                textureImporter.SetPlatformTextureSettings(tipsios);
            }
        }*/
    }

    /// <summary>
    /// 模型禁用材质导入
    /// </summary>
    void DisableMaterialImport(ModelImporter importer)
    {
        if (importer != null)
        {
            importer.importMaterials = false;
        }
    }

}