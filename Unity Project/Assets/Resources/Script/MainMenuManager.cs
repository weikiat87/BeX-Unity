using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
	// A Special Menu for myself to be lazy
	[System.Serializable]
	protected class Menu
	{
		public MainMenuManager.mMenuType mMenuType;
		public Vector3 mEulerAnglesRotation;
	}
	#region Variables
	public enum mMenuType { Main, Option, Credit };

	private Menu mCurrentMenu	= new Menu();
	private Menu mToMenu 		= new Menu();

	[SerializeField] private Menu[]	mMenuList;
	[SerializeField] private float 	mRotationSpeed;
	#endregion

	#region Singleton
	private static MainMenuManager mInstance;
	public static MainMenuManager Instance
	{
		get
		{
			if(mInstance == null) GameObject.Find("MainMenu").GetComponent<MainMenuManager>();
			return mInstance;
		}
	}
	#endregion

	#region Unity Function
	private void Awake()
	{
		if(mInstance == null)	mInstance = this;
		else if(mInstance != this)
		{
			if(mInstance.gameObject != this.gameObject) Destroy(this.gameObject);
			else 										Destroy(this);
		}
	}
	private void Update()
	{
		if(mCurrentMenu.mMenuType != mToMenu.mMenuType)	
		Transit();
	}
	#endregion

	#region Class Function
	public void ChangeMenu(mMenuType _type)
	{
		foreach(Menu menu in mMenuList)
		{
			if(menu.mMenuType == _type) 
			{
				mToMenu = menu;
				ButtonManager.Instance.EnableButton = false;
				return;
			}
		}
	}

	private void Transit()
	{
		Camera.main.transform.rotation = Quaternion.Lerp(	Camera.main.transform.rotation,
															Quaternion.FromToRotation(Camera.main.transform.forward,mToMenu.mEulerAnglesRotation),
															Time.deltaTime * mRotationSpeed);
		if(Quaternion.Angle(Camera.main.transform.rotation, Quaternion.FromToRotation(Camera.main.transform.forward,mToMenu.mEulerAnglesRotation)) == 0)
		{
			// Active Buttons
			ButtonManager.Instance.EnableButton = true;
			mCurrentMenu = mToMenu;
		}
	}
	#endregion
}

