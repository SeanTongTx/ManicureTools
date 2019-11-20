
namespace SeanLib.TortoiseSVN.Editor
{
#if EDITORPLUS
    using EditorPlus;
    using ServiceTools.Reflect;
#endif
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;
#if EDITORPLUS
    [CustomSeanLibEditor("ManicureTools/TortoiseSVN", order = 100)]
    public class TortoiseSVNMenu : SeanLibEditor
#else
    public class TortoiseSVNMenu : EditorWindow
#endif
    {

        [MenuItem("Assets/TortoiseSVN/Setting", false, 119)]
        //[MenuItem("Tools/SVNSetting")]
        private static void Setting()
        {
#if EDITORPLUS
            var tortoiseSvnMenu = EditorWindow.GetWindow<SeanLibManager>();
            var item = tortoiseSvnMenu.SeachIndex("ManicureTools/TortoiseSVN");
            if (item != null)
            {
                tortoiseSvnMenu.SelectIndex(item.id);
            }
#else
            var tortoiseSvnMenu = EditorWindow.GetWindow<TortoiseSVNMenu>();
#endif
        }

        [MenuItem("Assets/TortoiseSVN/Commit", false, 111)]
        private static void SVNCommit()
        {
            foreach (string path in GetSelectionPath())
            {
                TortoiseSVNCommit(path);
            }
        }

        [MenuItem("Assets/TortoiseSVN/Update", false, 110)]
        private static void SVNUpdate()
        {
            TortoiseSVNUpdate(GetSelectionPath());
        }
        [MenuItem("Assets/TortoiseSVN/UpdateAll", false, 112)]
        private static void UpdateAll()
        {
            List<string> paths = SubSVNSetting.Paths;
            TortoiseSVNUpdate(paths.ToArray());
        }
        [MenuItem("Assets/TortoiseSVN/CommitAll", false, 113)]
        private static void CommitAll()
        {
            List<string> paths = SubSVNSetting.Paths;
            foreach (string path in paths)
            {
                TortoiseSVNCommit(path);
            }
        }


        public static void TortoiseSVNUpdate(params string[] path)
        {
            string paths = "";
            foreach (var item in path)
            {
                paths += item + "*";
            }
            RunCmd(TortoiseSVNSetting.TortoiseProcPath, string.Format("/command:update  /path:\"{0}\"", paths));
            AssetDatabase.Refresh();
        }

        public static void TortoiseSVNCommit(string path)
        {
            RunCmd(TortoiseSVNSetting.TortoiseProcPath, string.Format("/command:commit  /path:\"{0}\"", path));
            AssetDatabase.Refresh();
        }



        public static string[] GetSelectionPath()
        {
            string[] path = new string[Selection.objects.Length];
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                var o = Selection.objects[i];
                var assetPath = AssetDatabase.GetAssetPath(o);
                path[i] = assetPath;
            }
            return path;
        }

        /// <summary>
        /// 运行cmd命令
        /// 会显示命令窗口
        /// </summary>
        /// <param name="cmdExe">指定应用程序的完整路径</param>
        /// <param name="cmdStr">执行命令行参数</param>
        static bool RunCmd(string cmdExe, string cmdStr)
        {
            bool result = false;
            try
            {
                using (Process myPro = new Process())
                {
                    //指定启动进程是调用的应用程序和命令行参数
                    ProcessStartInfo psi = new ProcessStartInfo(cmdExe, cmdStr);
                    myPro.StartInfo = psi;
                    myPro.Start();
                    myPro.WaitForExit();
                    result = true;
                }
            }
            catch
            {

            }
            return result;
        }

        private static List<ITortoiseSVNSettingEditor> settingEditors = new List<ITortoiseSVNSettingEditor>() { new TortoiseSVNSetting(), new SubSVNSetting() };

        private static Vector2 scrollView;
#if EDITORPLUS
        public override void OnGUI()
#else
        public void OnGUI()
#endif
        {
            scrollView = GUILayout.BeginScrollView(scrollView);
            for (int i = 0; i < settingEditors.Count; i++)
            {
                var tortoiseSvnSettingEditor = settingEditors[i];
                tortoiseSvnSettingEditor.ShowTortoiseSVNSetting();
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            }
            GUILayout.EndScrollView();
        }
    }
}
