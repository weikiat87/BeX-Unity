using UnityEngine;
using System.Collections;

/* <summary>
 * Basic Movement Controls for the player
 * to move the ship.
 * </summary>
 */

public class PlayerMovement : MonoBehaviour
{
	#region Variables
	[SerializeField] private int mSpeed;	// speed;
	#endregion

	#region Unity Functions
	private void Start()
	{
		PlayerController.Instance.UpdateMovementHook += UpdateMovement;
	}
	#endregion

	#region Delegate
	private void UpdateMovement()
	{
		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			//TO DO:
			//Add Axis Movement
			Vector3 temp = new Vector3(Input.acceleration.x *mSpeed * Time.deltaTime,0,0);
			gameObject.transform.Translate(temp);
		}
		else if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
		{
			Vector3 temp = Vector3.zero;
			if(Input.GetKey(KeyCode.LeftArrow))		
			{
				Debug.Log("left");
				temp = new Vector3( -mSpeed * Time.deltaTime,0,0);
			}
			if(Input.GetKey(KeyCode.RightArrow))	
			{
				Debug.Log("Right");
				temp = new Vector3(  mSpeed * Time.deltaTime,0,0);
			}
			if(temp != Vector3.zero) 					gameObject.transform.Translate(temp);
		}
	}
	#endregion
}
