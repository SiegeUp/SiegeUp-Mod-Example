using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SiegeUp.ModdingPlugin
{
	[CustomEditor(typeof(ModCommandsExecutor))]
	public class TestingToolGUI : Editor
	{
		private ModCommandsExecutor _targetObject;
		private string _command = "";

		private void OnEnable() => _targetObject = (ModCommandsExecutor)target;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			_command = GUILayout.TextField(_command);
			if (GUILayout.Button("Execute"))
			{
				_targetObject.Execute(_command.Split().ToList());
			}
		}
	}
}
