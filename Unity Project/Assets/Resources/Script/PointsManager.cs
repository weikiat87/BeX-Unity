using UnityEngine;
using System.Collections;

public class PointsManager : MonoBehaviour 
{
	private int mPoints;
	private int mCurrentPoints;
	[SerializeField] private GUIText mText;
	#region Singleton
	private static PointsManager mInstance;
	public static PointsManager Instance
	{
		get
		{
			if(mInstance == null) mInstance = GameObject.Find("PointsManager").GetComponent<PointsManager>();
			return mInstance;
		}
	}
	#endregion

	#region Unity Function
	private void Awake()
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance.gameObject != this.gameObject)	Destroy (this.gameObject);
		else 												Destroy (this);
	}
	private void Start()
	{
		mPoints = GameManager.Instance.GetCurrentDifficulty().mNotesPoint;
		mCurrentPoints = 0;
		mText.text = mCurrentPoints.ToString();
	}
	#endregion

	#region Class Function
	public int Points
	{
		get {	return mPoints;		}
		set	{	mPoints = value;	}
	}
	public int CurrentPoints
	{
		get {	return mCurrentPoints;	}
		set	{	mCurrentPoints = value;	}
	}
	#endregion

	#region UI
	private void OnGUI()
	{
		mText.text = mCurrentPoints.ToString();
	}
	#endregion
}
