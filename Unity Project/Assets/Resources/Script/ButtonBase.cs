using UnityEngine;
using System.Collections;

public abstract class ButtonBase : MonoBehaviour
{
	[SerializeField] protected Color mHoverColor;
	[SerializeField] protected Color mNormalColor;
	[SerializeField] protected Color mClickColor;
	protected TextMesh 		mTextMesh;
	protected RaycastHit	mHit;
	protected bool			mClicked;
	protected bool 			mHover = false;
	#region Unity Function
	private void Awake()	{		mTextMesh = gameObject.GetComponent<TextMesh>();	}
	protected virtual void Start()
	{
		ButtonManager.Instance.OnClickHook		+= OnClick;
		ButtonManager.Instance.OnHoverHook		+= OnHover;
		ButtonManager.Instance.OnExitHook 		+= OnExit;
		ButtonManager.Instance.OnReleaseHook	+= OnRelease;
		mClicked = false;
	}
	#endregion

	#region Class Function
	protected virtual void OnHover(Ray _ray)
	{
		if(Physics.Raycast(_ray,out mHit))
		{
			if(mHit.collider.gameObject == this.gameObject)
			{
				if(!mHover) SoundManager.Instance.Play("Hover");
				mHover = true;
				mTextMesh.color = mHoverColor;
			}
		}
	}
	protected virtual void OnExit(Ray _ray)
	{
		if(!Physics.Raycast(_ray,out mHit))
		{
			if(mClicked)
			{
				mClicked = false;
				ButtonManager.Instance.OnHoverHook += OnHover;
			}
			mHover   = false;
			mTextMesh.color = mNormalColor;
		}
	}
	protected void OnClick(Ray _ray)
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(Physics.Raycast(_ray,out mHit))
			{
				if(this.gameObject == mHit.collider.gameObject)
				{
					mClicked = true;
					ButtonManager.Instance.OnHoverHook -= OnHover;
					mTextMesh.color = mClickColor;
				}
			}
		}
	}
	protected abstract void OnRelease(Ray _ray);
	#endregion
}