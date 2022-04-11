using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SiegeUp.ModdingPlugin.SiegeUpModBase.ObjectRecord))]
public class SiegeUpObjectDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, property.isExpanded);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		try
		{
			var prefabRefProp = property.FindPropertyRelative("Prefab");
			var prefabRef = prefabRefProp.objectReferenceValue as GameObject;
			label.text = prefabRef.gameObject.name;
		}
		catch (System.NullReferenceException) { }
		catch (System.InvalidCastException) { }

		EditorGUI.PropertyField(position, property, label, property.isExpanded);
	}
}
