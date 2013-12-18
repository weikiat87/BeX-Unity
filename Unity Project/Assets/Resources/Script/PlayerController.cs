using UnityEngine;
using System.Collections;

/* <summary>
 * The Player Controller will have all the required component
 * that a player should have. It is also a singleton class.
 * </summary>
 */

[RequireComponent (typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{

	#region Singleton
	private static PlayerController mInstance;		// the Player Controller Instance
	public static PlayerController Instance			// Getting the player Instance
	{
		get
		{
			if(mInstance == null)
			{
				mInstance =	Resources.Load("Player") as PlayerController;
			}
			return mInstance;
		}
	}
	#endregion

	#region Unity Function
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
	}
	
	private void Start () 
	{
		
	}
	
	// Update is called once per frame
	private void Update () 
	{
		UpdateMovementHook();	// Firing the Event
	}
	#endregion

	#region Delegates
	public delegate void UpdateMovementDelegate();			// The function that we will be firing
	public event UpdateMovementDelegate UpdateMovementHook;	// The hook that we will be using to at other script
	#endregion
}
