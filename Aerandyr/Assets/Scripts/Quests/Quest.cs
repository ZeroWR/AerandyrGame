using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class Quest 
{
	public string Name;
	public string Description;
	public List<QuestSection> Sections;
	public Quest()
	{
		Sections = new List<QuestSection>();
	}
	[IgnoreDataMember]
	public bool IsDone { get { return Sections.All(x => x.IsDone); } }
	[IgnoreDataMember]
	public QuestSection CurrentSection { get; set; }
}
