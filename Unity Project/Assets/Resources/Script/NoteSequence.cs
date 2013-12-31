using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NoteSequence : MonoBehaviour
{
	[SerializeField] private NoteType[]		mSequenceList;						// The Actual List
	[SerializeField] private List<Texture> 	mGUIList = new List<Texture>();		// Textures
	[SerializeField] private Vector2		mStartPosition;						// Starting Position

	private int 		mCurrentIndex;
	#region Singleton
	private static NoteSequence mInstance;
	public static NoteSequence Instance
	{
		get
		{
			if(mInstance == null)
				mInstance = GameObject.Find("NotesManager").GetComponent<NoteSequence>() as NoteSequence;	
			return mInstance;
		}
	}
	#endregion

	#region Unity Function
	private void Awake()
	{
		if(mInstance == null)			mInstance = this;
		else if(mInstance != this)		Destroy(this);
	}
	private void Start()
	{
		mSequenceList = new NoteType[GameManager.Instance.GetCurrentDifficulty().mNumberOfSequenceSets];
		if(mSequenceList.Length == 0)				throw new System.NullReferenceException("PrefabList is Empty");
		ResetList();
		switch(mSequenceList[0])
		{
		case NoteType.typeA:	EffectManager.Instance.ChangeAuroraWaveColor(Color.cyan);	break;
		case NoteType.typeB:	EffectManager.Instance.ChangeAuroraWaveColor(Color.yellow);	break;
		case NoteType.typeC:	EffectManager.Instance.ChangeAuroraWaveColor(Color.green);	break;
		case NoteType.typeD:	EffectManager.Instance.ChangeAuroraWaveColor(Color.red);	break;
		}

	}
	private void OnGUI()
	{
		Texture image = null;
		for(int i=mCurrentIndex; i<mSequenceList.Length; i++)
		{

			switch(mSequenceList[i])
			{
			case NoteType.typeA: image = mGUIList[0];	break;
			case NoteType.typeB: image = mGUIList[1];	break;
			case NoteType.typeC: image = mGUIList[2];	break;
			case NoteType.typeD: image = mGUIList[3];	break;
			}
			GUI.DrawTexture( new Rect(Screen.width - image.width - 10	, mStartPosition.y + (i*50) + 10,
			                          image.width						, (mStartPosition.y+image.height))
			                ,	
			                image );
		}
	}
	#endregion

	#region Class Function
	public NoteType GetCurrentNote()	{	return mSequenceList[mCurrentIndex];	}
	public void NextNote()
	{
		if(++mCurrentIndex == mSequenceList.Length)		ResetSequence();
		else 											SoundManager.Instance.Play("Right");

	}
	public void ResetSequence()
	{
		SoundManager.Instance.Play("Clear");
		ResetList();
	}
	private void ResetList()
	{
		mCurrentIndex = 0;
		for(int i=0;i<mSequenceList.Length;i++)	mSequenceList[i] = Utility.GetRandomEnum<NoteType>();
	}
	#endregion
}

