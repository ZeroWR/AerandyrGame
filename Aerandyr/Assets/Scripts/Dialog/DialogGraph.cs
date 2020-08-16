using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;
using Aerandyr.Dialog;
using Aerandyr.Dialog.Nodes;

namespace Aerandyr.Dialog
{
	[CreateAssetMenu(fileName = "New Dialog Graph", menuName = "Aerandyr/Dialog Graph")]
	public class DialogGraph : NodeGraph
	{
		[HideInInspector]
		public DialogStartNode StartNode;
		[HideInInspector]
		public DialogEndNode EndNode;
		public SpeakerInfo[] Speakers;
		private void OnEnable()
		{
			if (this.nodes == null)
				return;
			this.StartNode = this.nodes.FirstOrDefault(x => x != null && x.GetType() == typeof(DialogStartNode)) as DialogStartNode;
			this.EndNode = this.nodes.FirstOrDefault(x => x != null && x.GetType() == typeof(DialogEndNode)) as DialogEndNode;
			if (this.StartNode == null && this.EndNode == null)
			{
				this.StartNode = this.AddNode<DialogStartNode>();
				this.StartNode.name = "Dialog Start";
				this.EndNode = this.AddNode<DialogEndNode>();
				this.EndNode.name = "Dialog End";
				this.StartNode.OnCreateConnection(this.StartNode.GetOutputPort("output"), this.EndNode.GetInputPort("input"));
			}
		}
	}
}
