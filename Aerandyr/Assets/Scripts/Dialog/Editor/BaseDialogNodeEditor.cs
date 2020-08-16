using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;
using Aerandyr.Dialog;
using Aerandyr.Dialog.Nodes;

//[CustomNodeEditor(typeof(BaseDialogNode))]
//public class BaseDialogNodeEditor : NodeEditor
//{
//	public override void OnBodyGUI()
//	{
//		if (target == null)
//		{
//			Debug.LogWarning("Null target node for node editor!");
//			return;
//		}
//		NodePort input = target.GetPort("input");
//		NodePort output = target.GetPort("output");

//		GUILayout.BeginHorizontal();
//		if (input != null) NodeEditorGUILayout.PortField(GUIContent.none, input, GUILayout.MinWidth(0));
//		if (output != null) NodeEditorGUILayout.PortField(GUIContent.none, output, GUILayout.MinWidth(0));
//		GUILayout.EndHorizontal();
//		EditorGUIUtility.labelWidth = 60;
//		base.OnBodyGUI();
//	}
//}

[CustomNodeEditor(typeof(DialogStartNode))]
public class DialogStartNodeEditor : NodeEditor
{
	public override void OnBodyGUI()
	{
		serializedObject.Update();
		if (target == null)
		{
			Debug.LogWarning("Null target node for node editor!");
			return;
		}
		NodePort output = target.GetPort("output");

		GUILayout.BeginHorizontal();
		if (output != null) NodeEditorGUILayout.PortField(GUIContent.none, output, GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();
		EditorGUIUtility.labelWidth = 60;
		serializedObject.ApplyModifiedProperties();
	}
}

[CustomNodeEditor(typeof(DialogEndNode))]
public class DialogEndNodeEditor : NodeEditor
{
	public override void OnBodyGUI()
	{
		serializedObject.Update();
		if (target == null)
		{
			Debug.LogWarning("Null target node for node editor!");
			return;
		}
		NodePort input = target.GetPort("input");

		GUILayout.BeginHorizontal();
		if (input != null) NodeEditorGUILayout.PortField(GUIContent.none, input, GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();
		EditorGUIUtility.labelWidth = 60;
		serializedObject.ApplyModifiedProperties();
	}
}

[CustomNodeEditor(typeof(DialogSectionNode))]
public class DialogSectionNodeEditor : NodeEditor
{
	private DialogSectionNode node;
	private DialogGraphEditor graphEditor;
	private DialogGraph graph;
	private int selectedSpeakerIndex = -1;
	private void InitNode()
	{
		if (node != null)
			return;
		node = target as DialogSectionNode;
		graphEditor = NodeGraphEditor.GetEditor(target.graph, window) as DialogGraphEditor;
		graph = graphEditor.target as DialogGraph;
		InitSelectedSpeakerIndex();
	}
	private void InitSelectedSpeakerIndex()
	{
		selectedSpeakerIndex = GetSpeakerIndex();
	}
	private int GetSpeakerIndex()
	{
		if (!CanDrawSpeakerPopup || node.Speaker == null)
			return -1;

		var equivalent = graph.Speakers.FirstOrDefault(x => x.Name == node.Speaker.Name);
		if (equivalent == null)
			return -1;
		return Array.IndexOf(graph.Speakers, equivalent);
	}
	private SpeakerInfo GetSpeaker(int index)
	{
		if (!GraphHasSpeakers)
			return null;
		return (index < 0 || index >= graph.Speakers.Length) ? null : graph.Speakers[index];
	}
	private bool CanDrawSpeakerPopup
	{
		get { return node != null && GraphHasSpeakers; }
	}
	private bool GraphHasSpeakers { get { return !(graph.Speakers == null || graph.Speakers.Length <= 0); } }
	public override void OnCreate()
	{
		base.OnCreate();
		InitNode();
	}
	public override void OnHeaderGUI()
	{
		InitNode();
		base.OnHeaderGUI();
	}
	public override void OnBodyGUI()
	{
		serializedObject.Update();
		if (target == null)
		{
			Debug.LogWarning("Null target node for node editor!");
			return;
		}
		NodePort input = target.GetPort("input");
		NodePort output = target.GetPort("output");

		GUILayout.BeginHorizontal();
		if (input != null) NodeEditorGUILayout.PortField(GUIContent.none, input, GUILayout.MinWidth(0));
		if (output != null) NodeEditorGUILayout.PortField(GUIContent.none, output, GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();
		if(selectedSpeakerIndex != GetSpeakerIndex() && node != null)
		{
			node.Speaker = GetSpeaker(selectedSpeakerIndex);
		}
		if(CanDrawSpeakerPopup)
			selectedSpeakerIndex = EditorGUILayout.Popup("Speaker", selectedSpeakerIndex, graph.Speakers.Select(x => x.Name).ToArray());
		else
			EditorGUILayout.LabelField("Speaker", "None");
		EditorGUIUtility.labelWidth = 60;
		serializedObject.ApplyModifiedProperties();
		base.OnBodyGUI();
	}
	public override Color GetTint()
	{

		if (node == null || node.Speaker == null)
			return base.GetTint();
		else
		{
			var color = node.Speaker.Color;
			color.a = 1;
			return color;
		}
	}
}
