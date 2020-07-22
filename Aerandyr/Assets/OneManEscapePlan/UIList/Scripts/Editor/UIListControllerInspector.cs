/// © 2019 Kevin Foley
/// For distribution only on the Unity Asset Store
/// Terms/EULA: https://unity3d.com/legal/as_terms

using OneManEscapePlan.UIList.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class UIListControllerInspector<ItemModel, ItemView> : Editor where ItemView : UIListItemViewBase<ItemModel> {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		UIListController<ItemModel, ItemView> list = (UIListController<ItemModel, ItemView>)target;

		EditorGUILayout.LabelField("Current List Size: " + list.Count);
		EditorGUILayout.LabelField("Current Pool Size: " + list.CurrentPoolSize);
	}
}
