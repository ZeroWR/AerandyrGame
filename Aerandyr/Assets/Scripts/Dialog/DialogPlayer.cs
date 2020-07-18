using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPlayer : MonoBehaviour
{
    public bool IsPlaying { get { return PlayerState == DialogPlayerState.IsPlaying; } }
	public bool IsFinishedPlaying { get { return PlayerState == DialogPlayerState.FinishedPlaying; } }
    public Dialog CurrentDialog { get; protected set; }
    private int CurrentSection { get; set; }
    private int CurrentPlaceInSection { get; set; }
    private string CurrentSectionText { get; set; }
    private float fadeInTime = 0.025F;

    public enum DialogPlayerState
    {
        NotPlaying,
        ShouldBePlaying,
        IsPlaying,
        ShouldFinishPlaying,
        FinishedPlaying
    };

    public DialogPlayerState PlayerState { get; private set; }

    public DialogPlayer()
    {
        CurrentSection = 0;
        CurrentPlaceInSection = 0;
        PlayerState = DialogPlayerState.NotPlaying;
    }

    // Update is called once per frame
    public void ProcessInput()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") || Input.GetButtonDown("Use"))
        {
            //If we're currently making our way through a section, skip to the end of it.
            if (!IsCurrentSectionDone)
            {
                FinishCurrentSection();
            }
			//If we're completely done with the dialog, signal the end of it.
			//else if (dialogueComplete && !printingText)
			//{
			//	//Leave this up to whatever is controlling the player.
			//}
			//If we're not done with the dialog, but we aren't currently printing anything, start printing the next section.
			else if (IsCurrentSectionDone && !IsFinishedPlaying)
            {
				StartNextSection();
				InvokeRepeating("PlayCurrentSection", fadeInTime, fadeInTime);
			}
        }
    }

    public bool LoadDialog(Dialog dialog)
    {
        if (IsPlaying)
            return false;

        Reset();
        CurrentDialog = dialog;
        return true;
    }

    public bool PlayDialog()
    {
        if (IsPlaying || CurrentDialog == null)
            return false;

        Reset();
        PlayerState = DialogPlayerState.ShouldBePlaying;
		OnStartedPlaying();
		InvokeRepeating("PlayCurrentSection", fadeInTime, fadeInTime);
		return true;
    }

    public void StopPlaying()
    {
		Reset();
    }

	private void Reset()
	{
		PlayerState = DialogPlayerState.NotPlaying;
		CurrentSection = 0;
		CurrentPlaceInSection = 0;
		CurrentSectionText = string.Empty;
	}

	void PlayCurrentSection()
	{
		if(PlayerState == DialogPlayerState.ShouldFinishPlaying)
		{
			CancelInvoke("PlayCurrentSection");
			return;
		}
		PlayerState = DialogPlayerState.IsPlaying;
		DialogSection ourCurrentSection = CurrentDialog.Sections[CurrentSection];
		if(CurrentPlaceInSection == 0 && ourCurrentSection.SectionText.Length > 0)
		{
			OnSectionStarted(ourCurrentSection);
		}
		if(CurrentPlaceInSection >= ourCurrentSection.SectionText.Length)
		{
			CancelInvoke("PlayCurrentSection");
			CurrentSectionText = ourCurrentSection.SectionText;
			OnTextUpdated(CurrentSectionText);
			OnSectionFinished(ourCurrentSection);

			if (DonePlayingEntireDialog)
			{
				PlayerState = DialogPlayerState.FinishedPlaying;
				CurrentDialog.HasPlayed = true;
				OnFinishedPlaying();
			}
		}
		else
		{
			CurrentSectionText += ourCurrentSection.SectionText[CurrentPlaceInSection];
			OnTextUpdated(CurrentSectionText);
			CurrentPlaceInSection++;
		}
		
	}

	private void FinishCurrentSection()
    {
        if (CurrentDialog == null || CurrentDialog.Sections.Count == 0 || CurrentSection > CurrentDialog.Sections.Count)
            return;

        CurrentSectionText = CurrentDialog.Sections[CurrentSection].SectionText;
        CurrentPlaceInSection = CurrentSectionText.Length;
    }

    private void StartNextSection()
    {
        if (CurrentDialog == null || CurrentDialog.Sections.Count == 0 || CurrentSection > CurrentDialog.Sections.Count)
            return;
        CurrentSectionText = string.Empty;
        CurrentPlaceInSection = 0;
        ++CurrentSection;
    }

    private bool IsCurrentSectionDone
    {
        get
        {
            return
                CurrentDialog != null &&
                CurrentDialog.Sections.Count > 0 &&
                CurrentSection < CurrentDialog.Sections.Count &&
                CurrentPlaceInSection >= (CurrentDialog.Sections[CurrentSection].SectionText.Length - 1);
        }
    }

    private bool DonePlayingEntireDialog
    {
        get
        {
			return CurrentDialog != null && CurrentSection >= (CurrentDialog.Sections.Count - 1) && IsCurrentSectionDone;
        }
    }

    #region Events
    public delegate void SectionStartedEventHandler(object sender, DialogSection section);
    public delegate void SectionFinishedEventHandler(object sender, DialogSection section);
    public delegate void TextUpdatedEventHandler(object sender, string currentText);
    public delegate void StartedPlayingEventHandler(object sender);
    public delegate void FinishedPlayingEventHandler(object sender);

    public SectionStartedEventHandler SectionStarted;
    public SectionFinishedEventHandler SectionFinished;
    public TextUpdatedEventHandler TextUpdated;
    public StartedPlayingEventHandler StartedPlaying;
    public FinishedPlayingEventHandler FinishedPlaying;

    protected void OnSectionStarted(DialogSection section)
    {
        if (SectionStarted != null)
            SectionStarted(this, section);
    }

    protected void OnSectionFinished(DialogSection section)
    {
        if (SectionFinished != null)
            SectionFinished(this, section);
    }

    protected void OnTextUpdated(string currentText)
    {
        if (TextUpdated != null)
            TextUpdated(this, currentText);
    }

    protected void OnStartedPlaying()
    {
        if (StartedPlaying != null)
            StartedPlaying(this);
    }

    protected void OnFinishedPlaying()
    {
        if (FinishedPlaying != null)
            FinishedPlaying(this);
    }

    #endregion
}
