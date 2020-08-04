using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TransientDialog : Dialog
{
	public TransientDialog(string dialogText, string speakerName = "", string speakerNameColor = "")
	{
		var section = new DialogSection();
		section.SectionText = dialogText;
		section.SpeakerName = speakerName;
		section.SpeakerNameColor = speakerNameColor;
		this.Sections.Add(section);
		this.ShouldOnlyPlayOnce = true;
	}
}
