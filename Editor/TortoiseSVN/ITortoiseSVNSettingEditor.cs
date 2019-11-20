
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
#if EDITORPLUS
using EditorPlus;
using ServiceTools.Reflect;
#endif
namespace SeanLib.TortoiseSVN.Editor
{
    public interface ITortoiseSVNSettingEditor
    {
        void ShowTortoiseSVNSetting();

        int GetTortoiseSVNSettingOrder();
    }

    public class SubSVNSetting : ITortoiseSVNSettingEditor
    {
        private static List<string> _Paths;

        public static List<string> Paths
        {
            get
            {
                if (_Paths == null)
                {
                    _Paths = new List<string>();
                    string path = Application.dataPath;
                    AllDir(path);
#if EDITORPLUS
                    var PackStorageDir = EditorUserSettings.GetConfigValue(HomePage.packageKey);
                    var LocalLibDir = EditorUserSettings.GetConfigValue(HomePage.localKey);
                    AllDir(LocalLibDir);
                    AllDir(PackStorageDir);
#endif
                    _Paths.Add(Application.dataPath);
                }
                return _Paths;
            }
        }
        public void ShowTortoiseSVNSetting()
        {
            foreach (string path in Paths)
            {
                GUILayout.Label(path);
            }
            if (GUILayout.Button("扫描"))
            {
                _Paths = null;
            }
        }

        private static void AllDir(string dirRoot)
        {
            List<string> dirs = new List<string>(Directory.GetDirectories(dirRoot));

            string directoryName = "";
            foreach (var dir in dirs)
            {
                directoryName = Path.GetFileName(dir);
                if (directoryName == ".svn")
                {
                    _Paths.Add(dirRoot);
                }
                else
                {
                    AllDir(dir);
                }
            }
        }
        public int GetTortoiseSVNSettingOrder()
        {
            return 2;
        }
    }
    public class TortoiseSVNSetting : ITortoiseSVNSettingEditor
    {
        public static string TortoiseProcPath
        {
            get
            {
                if (!EditorPrefs.HasKey(TortoiseProcPathKey))
                {
                    EditorPrefs.SetString(TortoiseProcPathKey, "TortoiseProc.exe");
                }
                return EditorPrefs.GetString(TortoiseProcPathKey);
            }
            set
            {
                EditorPrefs.SetString(TortoiseProcPathKey, value);
            }
        }

        private static string TortoiseProcPathKey
        {
            get
            {
                var key = string.Format("{0}TortoiseProc.exe 目录", UnityEngine.Application.dataPath);
                return key;
            }
        }

        public void ShowTortoiseSVNSetting()
        {
            GUILayout.Label("TortoiseProc.exe 目录");

            TortoiseProcPath = EditorGUILayout.TextField(TortoiseProcPath);
        }

        public int GetTortoiseSVNSettingOrder()
        {
            return 1;
        }

    }
}