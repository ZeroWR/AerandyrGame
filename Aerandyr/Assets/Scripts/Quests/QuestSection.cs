using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class QuestSection
{
	public string Name;
	public List<QuestObjective> Objectives;
	[IgnoreDataMember]
	public bool IsDone { get { return this.Objectives.All(x => x.IsDone); } }
	public QuestSection()
	{
		Objectives = new List<QuestObjective>();
	}
}
