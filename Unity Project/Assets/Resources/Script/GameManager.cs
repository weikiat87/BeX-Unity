using UnityEngine;
using System.Collections;

/* <summary>
 * The Game Manager will save and load
 * data as well as holding the difficulty settings
 * and transition from 1 level to another.
 * </summary>
 */
[RequireComponent (typeof(Transition))]
public class GameManager : MonoBehaviour 
{
	[SerializeField] private Difficulty[] mDifficultyList;	// List of Difficulties available
	[SerializeField] private ScreenType mType;				// Screen Type
	[SerializeField] private string mLevelToLoad;			// Level to load
	private Difficulty mCurrentDifficulty;					// Current Difficulty
	private Transition mTransition;							// Transition Controller

	// Singleton
	#region Singleton
	private static GameManager mInstance;
	public static GameManager Instance
	{
		get
		{
			if(mInstance == null)	GameObject.Find("GameManager").GetComponent<GameManager>();
			return mInstance;
		}
	
	}
	#endregion

	#region Unity Function
	private void Awake()
	{
		//  Checking for Singleton
		if(mInstance == null) mInstance = this;
		else if(mInstance.gameObject != this.gameObject)	Destroy(this.gameObject);
		else 												Destroy(this);
		DontDestroyOnLoad(this.gameObject);		// keeping the object persistent throught the entire game

		// Setting up the transition component
		mTransition = gameObject.GetComponent<Transition>();
		mTransition.Init();		// initializing Transition Component
		mTransition.FadeIn();	// start fading in

		Type = ScreenType.main;							// Setting Screen Type Default (Main as we will be in the Main Menu)
		Screen.sleepTimeout = SleepTimeout.NeverSleep;	// Prevent the game from sleeping in mobile
	}
	private void Start()	{	LoadData();	}			// Load Data at the start
	private void Update()
	{
		// Quiting the Game using hardware buttons/keyboard
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			// in game, load the main menu
			if(mType == ScreenType.game)
			{
				Debug.Log("Quiting Level");
				StartCoroutine( LoadLevel(mLevelToLoad,ScreenType.main) );
			}
			// Not in game, save and quit
			else
			{
				SaveData();
				Application.Quit();
			}
		}
	}
	#endregion

	#region Class Function
	// Loading a Scene using string
	public IEnumerator LoadLevel(string _levelName,ScreenType _type)
	{
		mTransition.FadeOut();						// Start Fade Out
		yield return new WaitForSeconds(2.0f);		// Wait for timer
		mTransition.FadeIn();						// Pre-Start Fade In
		mType = _type;								// Change Screen Typing (allow our buttons to change it to game)
		if(mType == ScreenType.game)
		{
			SoundManager.Instance.SetGUI(true);		// Enable our GUI Text
			SoundManager.Instance.PlayBGM("Free");	// Play Song
		}
		else						
		{
			SoundManager.Instance.SetGUI(false);		// Enable GUI Text
			SoundManager.Instance.PlayBGM("Pamgaea");	// Play Song
		}
		Application.LoadLevel(_levelName);			// Load Level
	}
	public Difficulty GetCurrentDifficulty()	{	return mCurrentDifficulty;	}
	public void SetDifficulty(DifficultyType _type)
	{
		foreach(Difficulty temp in mDifficultyList)
		{
			if(temp.mDifficulty == _type)	
			{
				mCurrentDifficulty = temp;
				Global.mCurrentDifficulty = mCurrentDifficulty.mDifficulty;
				if(ButtonManager.Instance != null) ButtonManager.Instance.UpdateDifficultyButtons();
				return;
			}
		}
		throw new System.ArgumentException("Invalid Typing");
	}
	public void ResetScores()
	{
		foreach(Difficulty d in mDifficultyList)	{	d.mHighScore = 0;	}
	}
	public int GetScore(DifficultyType _type)
	{
		foreach(Difficulty d in mDifficultyList)
		{		
			if(d.mDifficulty == _type) 
				return d.mHighScore;		
		}
		throw new System.ArgumentOutOfRangeException("Invalid Type");
	}

	public void SaveData()
	{
		// Updating Current Difficulty Highscore
		switch(mCurrentDifficulty.mDifficulty)
		{
		case DifficultyType.easy: 	mDifficultyList[0].mHighScore = mCurrentDifficulty.mHighScore;	break;
		case DifficultyType.normal:	mDifficultyList[1].mHighScore = mCurrentDifficulty.mHighScore;	break;
		case DifficultyType.hard:	mDifficultyList[2].mHighScore = mCurrentDifficulty.mHighScore;	break;
		}
		// Saving Difficulties into PlayerPrefs (Persistent Data)
		foreach(Difficulty d in mDifficultyList)
		{
			switch(d.mDifficulty)
			{
			case DifficultyType.easy: 	PlayerPrefs.SetInt("EasyHighscore",d.mHighScore);	break;
			case DifficultyType.normal:	PlayerPrefs.SetInt("NormalHighscore",d.mHighScore);	break;
			case DifficultyType.hard:	PlayerPrefs.SetInt("HardHighScore",d.mHighScore);	break;
			}
		}
		PlayerPrefs.SetInt("Music",Global.mMusicOn?1:0);							// Saving Music Setting
		PlayerPrefs.SetInt("SFX",Global.mSFXOn?1:0);								// Saving SFX Settings
		PlayerPrefs.SetString("Difficulty",Global.mCurrentDifficulty.ToString());	// Saving Difficulty Settings
		#if UNITY_ANDROID || UNITY_IPHONE
		PlayerPrefs.SetString("AndroidControl",Global.mControlType.ToString());		// Saving Control Settings
		#endif
		PlayerPrefs.Save();															// Writing PlayerPrefs into Memory
		if(ButtonManager.Instance != null)											
		{
			ButtonManager.Instance.UpdateDifficultyButtons();		// Update Buttons
			ButtonManager.Instance.UpdateAndroidControlButtons();	// Update Buttons
		}
	}
	public void LoadData()
	{
		//PlayerPrefs.DeleteAll();						// Use this if you want to delete the playersPrefs Keys for testing purpose

		// Loading Difficulty Scores into List
		for(int i=0; i<mDifficultyList.Length;i++)
		{
			switch(mDifficultyList[i].mDifficulty)
			{
			case DifficultyType.easy: 	mDifficultyList[i].mHighScore = 	PlayerPrefs.GetInt("EasyHighscore");	break;
			case DifficultyType.normal:	mDifficultyList[i].mHighScore = 	PlayerPrefs.GetInt("NormalHighscore");	break;
			case DifficultyType.hard:	mDifficultyList[i].mHighScore = 	PlayerPrefs.GetInt("HardHighScore");	break;
			}
		}

		// Added checks to fix start button error
		if(PlayerPrefs.HasKey("Music")) Global.mMusicOn	= PlayerPrefs.GetInt("Music")==1?true:false;	// Load Music Settings
		else 							Global.mMusicOn	= true;											// When there is no key, default setting is true
		if(PlayerPrefs.HasKey("SFX"))	Global.mSFXOn	= PlayerPrefs.GetInt("SFX")==1?true:false;		// Load SFX Settings
		else							Global.mSFXOn = true;											// When there is no key, default setting is true
		if(PlayerPrefs.HasKey("Difficulty"))	Global.mCurrentDifficulty = (DifficultyType) System.Enum.Parse( typeof( DifficultyType ), PlayerPrefs.GetString("Difficulty"));
		else 									Global.mCurrentDifficulty = DifficultyType.normal;	
		#if UNITY_ANDROID || UNITY_IPHONE
		if(PlayerPrefs.HasKey("AndroidControl"))	Global.mControlType = (AndroidControl) System.Enum.Parse( typeof( AndroidControl ), PlayerPrefs.GetString("AndroidControl"));
		else 										Global.mControlType = AndroidControl.drag;
		#endif
		SetDifficulty(Global.mCurrentDifficulty);
		if(ButtonManager.Instance != null)
		{
			ButtonManager.Instance.UpdateAndroidControlButtons();
		}
		SoundManager.Instance.SetVolume();			// Set the Volume of the Game
	}
	// Accessor for the game screen type
	public ScreenType Type
	{
		get	{ return mType;		}
		set	{ mType = value;	}
	}
	public string LevelToLoad { get { return mLevelToLoad; } }	// Gets the Level to Load
	#endregion
}