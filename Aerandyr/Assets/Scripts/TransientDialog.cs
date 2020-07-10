using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TransientDialog : Dialog
{
	public TransientDialog(string dialogText)
	{
		var section = new DialogSection();
		section.SectionText = dialogText;
		this.Sections.Add(section);
		this.ShouldOnlyPlayOnce = true;
	}
}
