using UnityEngine;
using System.Collections;

public class ScrollingManager : MonoBehaviour
{
	[SerializeField] private GameObject[] mList;
	private IEnumerator[] mRoutineList;
	private int mCurrentIndex;
	private void Start()
	{	
		mCurrentIndex = 0;
		mRoutineList = new IEnumerator[mList.Length];
		for(int i=0;i< mRoutineList.Length;i++)
		{
			mRoutineList[i] = mList[i].GetComponent<ScrollingScript>().Scroll();
		}
	}
	private void Update()
	{
		StartCoroutine(ScrollObjects());
	}
	private IEnumerator ScrollObjects()
	{	
		yield return StartCoroutine(mList[mCurrentIndex].GetComponent<ScrollingScript>().Scroll());
	}
	public void IncreaseIndex()
	{
		if(++mCurrentIndex == mList.Length) mCurrentIndex = 0;
	}
}

