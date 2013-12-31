using UnityEngine;
using System.Collections;

public class ScrollingScript : MonoBehaviour
{
	[SerializeField] private Vector3 mStartPos;
	[SerializeField] private Vector3 mWaitPos;
	[SerializeField] private Vector3 mEndPos;
	private enum mState { start,waiting,wait,end }
	[SerializeField] private mState mCurrentState = mState.start;
	private const int SCROLLINGSPEED = 1;

	public IEnumerator Scroll()
	{
		switch(mCurrentState)
		{
		case mState.start: 
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,mWaitPos,Time.deltaTime * SCROLLINGSPEED); 
			if( Vector3.Distance(gameObject.transform.position,mWaitPos) < 0.2f)
			{mCurrentState = mState.waiting;}
			break;
		case mState.waiting:
			yield return new WaitForSeconds(2.0f);
			mCurrentState = mState.wait;
			break;
		case mState.wait:
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,mEndPos,Time.deltaTime * SCROLLINGSPEED);
			if( Vector3.Distance(gameObject.transform.position,mEndPos) < 0.5f)
			mCurrentState = mState.end;
			break;
		case mState.end:
			gameObject.transform.position = mStartPos;
			mCurrentState 				  = mState.start;
			gameObject.transform.parent.GetComponent<ScrollingManager>().IncreaseIndex();
			yield break;
		}
		yield return Scroll();
	}
}

