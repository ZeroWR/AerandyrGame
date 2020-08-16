using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Aerandyr.Dialog.Nodes
{
	[HideInInspector]
	public class BaseDialogNode : Node
	{
		protected DialogGraph GetDialogGraph() { return this.graph as DialogGraph; }
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