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
	[SerializeField] private int mSpeed;			// Speed;
	[SerializeField] private Vector2 mLowerLimit;	// Minimum Range;
	[SerializeField] private Vector2 mUpperLimit;	// Maximum Range;
	#endregion

	#region Unity Functions
	private void Start()
	{
		PlayerController.Instance.UpdateMovementHook += UpdateMovement;	// Attach our function to the hook
	}
	#endregion

	#region Class Function
	private void UpdateMovement()
	{
		if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			// Left Right
			Vector3 xAxisMovement = new Vector3(Input.acceleration.x * mSpeed * Time.deltaTime,0,0);
			if( (xAxisMovement.x + gameObject.transform.position.x) > mLowerLimit.x &&
			   (xAxisMovement.x + gameObject.transform.position.x) < mUpperLimit.x )
			{
				PlayerController.Instance.GetModelTransform().Rotate(new Vector3(0,0,mSpeed/2*Time.deltaTime*-Input.acceleration.x), Space.Self);
				gameObject.transform.Translate(xAxisMovement,Space.Self);
			}
			Vector3 yAxisMovement = Vector3.zero;
			if(Input.acceleration.y < -0.5f && gameObject.transform.position.y > mLowerLimit.y)	
				yAxisMovement = new Vector3(0,(Input.acceleration.y + 0.5f) * mSpeed * Time.deltaTime,0);
			else if(Input.acceleration.y < 0.0f && Input.acceleration.y > -0.5f && gameObject.transform.position.y < mUpperLimit.y)
				yAxisMovement = new Vector3(0,-Input.acceleration.y * mSpeed * Time.deltaTime,0);
			else if(gameObject.transform.position.y < mUpperLimit.y && Input.acceleration.y > 0.0f)
				yAxisMovement = new Vector3(0, 0.5f * mSpeed * Time.deltaTime,0);
			gameObject.transform.Translate(yAxisMovement,Space.World);

		}
		else if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
		{
			Vector3 xAxisMovement = Vector3.zero;
			Vector3 yAxisMovement = Vector3.zero;

			// Left Right
			if(Input.GetKey(KeyCode.LeftArrow))			xAxisMovement = new Vector3( -mSpeed * Time.deltaTime,0,0);
			else if(Input.GetKey(KeyCode.RightArrow))	xAxisMovement = new Vector3(  mSpeed * Time.deltaTime,0,0);

			// Up Down
			if(Input.GetKey(KeyCode.UpArrow))			yAxisMovement = new Vector3(  0, mSpeed * Time.deltaTime,0);	
			else if(Input.GetKey(KeyCode.DownArrow))	yAxisMovement = new Vector3(  0,-mSpeed * Time.deltaTime,0);	

			// Check X Axis
			if(xAxisMovement != Vector3.zero  &&	(xAxisMovement.x + gameObject.transform.position.x) > mLowerLimit.x &&
			   										(xAxisMovement.x + gameObject.transform.position.x) < mUpperLimit.x)
			{
				PlayerController.Instance.GetModelTransform().Rotate(new Vector3(0,0,-xAxisMovement.x),Space.Self);
				gameObject.transform.Translate(xAxisMovement,Space.Self);
			}
			// Check Y Axis
			if(yAxisMovement != Vector3.zero  &&	(yAxisMovement.y + gameObject.transform.position.y) > mLowerLimit.y &&
			   										(yAxisMovement.y + gameObject.transform.position.y) < mUpperLimit.y )
				gameObject.transform.Translate(yAxisMovement,Space.World);

		}
	}
	#endregion
}
