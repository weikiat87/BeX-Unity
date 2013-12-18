using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetManager : MonoBehaviour
{
	#region Variables
	[SerializeField] private GameObject mPlanetPrefab;		// Planets Prefabs
	[SerializeField] private int 		mSize;				// Size of our Manager

	[SerializeField] private List<PlanetAI> mList = new List<PlanetAI>();	// Creating Our List
	#endregion

	#region Singleton
	private static PlanetManager mInstance;
	public static PlanetManager Instance
	{
		get
		{
			if(mInstance == null)
			{	mInstance = Resources.Load("PlanetManager") as PlanetManager;	}
			return mInstance;
		}
	}
	#endregion

	#region Unity
	private void Awake()
	{
		// Checking if there are any duplicates
		if (mInstance == null)		
		{	
			mInstance = this;
		}
		else if(mInstance != this)		
		{
			if(mInstance.gameObject != this.gameObject)	Destroy(gameObject);
			else 										Destroy(this);
		}
		// Adding Planets into List
		while(mList.Count < mSize)
		{
			GameObject temp = Instantiate(mPlanetPrefab) as GameObject;
			temp.gameObject.SetActive(false);
			mList.Add(temp.GetComponent<PlanetAI>());

		}
	}

	private void Start()
	{

	}

	private void Update()
	{

	}
	#endregion

	#region Delegates

	#endregion
}