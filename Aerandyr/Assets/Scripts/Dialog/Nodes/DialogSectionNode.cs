using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Aerandyr.Dialog.Nodes
{
	public class DialogSectionNode : BaseDialogNode
	{
		[Input, HideInInspector] public EmptyValueType input;
		[Output, HideInInspector] public EmptyValueType output;
		[HideInInspector] public SpeakerInfo Speaker;
		[TextArea] public string text;
		// Use this for initialization
		protected override void Init()
		{
			base.Init();
		}

		// Return the correct value of an output port when requested
		public override object GetValue(NodePort port)
		{
			return null; // Replace this
		}
	}
}