using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
class SimplePerformanceTestingTool : MonoBehaviour
{
	public List<double> results = new List<double>() { 0, 0 };
	private Stopwatch sw = new Stopwatch();
	private ModsLoader modsModel = new ModsLoader();

	public void TestArray()
	{
		Action task = () => modsModel.TryLoadBundle(@"C:\Users\Пользователь\AppData\LocalLow\Zdorovtsov\SiegeUp!\mods\Simple decorations\windows");
		AssetBundle.UnloadAllAssetBundles(true);

		GC.Collect();
		GC.WaitForPendingFinalizers();

		sw.Restart();
		for (int i=0; i< 100; i++)
		{
			sw.Start();
			task.Invoke();
			sw.Stop();
			AssetBundle.UnloadAllAssetBundles(true);
		}
		sw.Stop();
		results[0] = sw.Elapsed.TotalMilliseconds;
	}
}

[CustomEditor(typeof(SimplePerformanceTestingTool))]
class SimplePerformanceTestingToolGUI : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Test array"))
		{
			(target as SimplePerformanceTestingTool).TestArray();
		}
	}
}
