using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HyperCasualPack
{
    [CustomEditor(typeof(MonitorCanvas))]
    public class MonitorCanvasEditor : Editor
    {
        private readonly List<GameObject> _activeResourceDatas = new();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MonitorCanvas monitorCanvas = (MonitorCanvas)target;

            if (GUILayout.Button("Create UI", GUILayout.MaxWidth(100f), GUILayout.MaxHeight(40f)))
            {
                string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(monitorCanvas.gameObject);
                if (string.IsNullOrEmpty(path))
                {
                    path = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage()?.assetPath;
                }

                var prefab = PrefabUtility.LoadPrefabContents(path);

                int childCount = prefab.transform.childCount;
                for (int i = childCount - 1; i >= 0; i--)
                {
                    Transform item = prefab.transform.GetChild(i);
                    if (item != prefab.transform)
                    {
                        DestroyImmediate(item.gameObject);
                    }
                }

                _activeResourceDatas.Clear();

                MonitorCanvas monitor = prefab.GetComponent<MonitorCanvas>();
                foreach (ScriptableObjects.ResourceData item in monitor.Datas)
                {
                    if (item != null)
                    {
                        GameObject instantiated = (GameObject)PrefabUtility.InstantiatePrefab(monitor.ResourceUIPrefab.gameObject, prefab.transform);
                        SerializedObject so = new SerializedObject(instantiated);
                        so.FindProperty("m_Name").stringValue = item.name + " Resource UI";
                        instantiated.GetComponent<ResourceUI>().Initialize(item.Sprite);
                        instantiated.GetComponent<SaveableIntVariableMonitor>().Initialize(item.CollectedAmount);
                        _activeResourceDatas.Add(instantiated.gameObject);
                        so.ApplyModifiedProperties();
                    }
                }

                PrefabUtility.SaveAsPrefabAsset(prefab, path);
                PrefabUtility.UnloadPrefabContents(prefab);
            }
        }
    }
}