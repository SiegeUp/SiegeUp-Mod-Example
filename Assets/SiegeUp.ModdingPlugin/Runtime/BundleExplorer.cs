using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SiegeUp.ModdingPlugin
{
	[ExecuteInEditMode]
	public class BundleExplorer : MonoBehaviour
	{
		[SerializeField]
		private List<GameObject> _spawnedObjects = new List<GameObject>();
		[SerializeField]
		private List<SiegeUpModBase> _loadedMods = new List<SiegeUpModBase>();
		private ModsLoader _modsLoader;
		private const int ObjectsInterval = 2;

		private void OnEnable()
		{
			_modsLoader = new ModsLoader(SiegeUpModdingPluginConfig.Instance.PluginVersion, "1.1.102r19");
		}

		public void LoadBundle(string path)
		{
			_loadedMods.Add(_modsLoader.LoadBundle(path));
		}

		public void SpawnObjects()
		{
			int x = _spawnedObjects.Count * ObjectsInterval;
			var objects = _loadedMods.Last().GetAllObjects();
			foreach (var prefab in objects)
			{
				var go = Instantiate(prefab, new Vector3(x, 0, 0), Quaternion.identity, transform);
				_spawnedObjects.Add(go);
				x += ObjectsInterval;
			}
		}

		public void UnloadAllBundles()
		{
			foreach (var go in _spawnedObjects)
				DestroyImmediate(go.gameObject);
			_spawnedObjects.Clear();
			_modsLoader.UnloadMods();
			_loadedMods.Clear();
		}

		private void OnDestroy()
		{
			UnloadAllBundles();
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(BundleExplorer))]
	public class BundleExplorerGUI : Editor
	{
		private BundleExplorer _targetObject;

		private void OnEnable() => _targetObject = (BundleExplorer)target;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (GUILayout.Button("Load bundle"))
			{
				var selectedPath = EditorUtility.OpenFilePanel("Select mod file", "", "");
				_targetObject.LoadBundle(selectedPath);
				EditorUtility.SetDirty(_targetObject);
			}

			if (GUILayout.Button("Spawn objects from last loaded bundle"))
			{
				_targetObject.SpawnObjects();
				EditorUtility.SetDirty(_targetObject);
			}

			if (GUILayout.Button("Unload all bundles"))
			{
				_targetObject.UnloadAllBundles();
				EditorUtility.SetDirty(_targetObject);
			}
		}
	}
#endif
}