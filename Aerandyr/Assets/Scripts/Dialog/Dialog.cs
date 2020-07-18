using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Dialog 
{
    public Dialog()
    {
        Sections = new List<DialogSection>();
        ShouldOnlyPlayOnce = true;
        HasPlayed = false;
    }
    public string Name;
    public List<DialogSection> Sections;
    public bool ShouldOnlyPlayOnce;
    public bool HasPlayed;
}
