using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using Aerandyr.Dialog;

[CustomNodeGraphEditor(typeof(DialogGraph))]
public class DialogGraphEditor : NodeGraphEditor
{

	/// <summary> 
	/// Overriding GetNodeMenuName lets you control if and how nodes are categorized.
	/// In this example we are sorting out all node types that are not in the XNode.Examples namespace.
	/// </summary>
	public override string GetNodeMenuName(System.Type type)
	{
		if (type == typeof(Aerandyr.Dialog.Nodes.BaseDialogNode))
			return null;
		if (type.Namespace == "Aerandyr.Dialog.Nodes")
		{
			return base.GetNodeMenuName(type).Replace("Aerandyr/Dialog/Nodes/", "");
		}
		else return null;
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
