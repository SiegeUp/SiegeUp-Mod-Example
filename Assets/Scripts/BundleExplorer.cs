using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BundleExplorer : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> _spawnedObjects = new List<GameObject>();
	[SerializeField]
	private SiegeUp.ModdingPlugin.SiegeUpModBase _loadedMod;
	private ModsLoader _modsLoader = new ModsLoader();

    public void LoadBundle(string path)
	{
		if (_loadedMod != null)
			UnloadBundle();
		_loadedMod = _modsLoader.TryLoadBundle(path);
	}

	public void SpawnObjects()
	{
		int x = 0;
		var objects = _loadedMod.GetAllAssets();
		foreach(var prefab in objects)
		{
			var go = Instantiate(prefab, new Vector3(x, 0, 0), Quaternion.identity, transform);
			_spawnedObjects.Add(go);
			x += 2;
		}
	}

	private void UnloadBundle()
	{
		foreach (var go in _spawnedObjects)
			DestroyImmediate(go.gameObject);
		_spawnedObjects.Clear();
		AssetBundle.UnloadAllAssetBundles(true);
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
		}

		if (GUILayout.Button("Spawn objects"))
		{
			_targetObject.SpawnObjects();
		}
	}
}
#endif