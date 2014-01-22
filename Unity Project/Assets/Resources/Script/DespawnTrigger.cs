using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class DespawnTrigger : MonoBehaviour 
{
	private void Awake()	{		gameObject.collider.isTrigger = true;	}

	private void OnTriggerEnter(Collider _c)
	{
		if(_c.gameObject.GetComponent<PlanetAI>())							// if we hit a planet despawn it
			_c.gameObject.GetComponent<PlanetAI>().IsEnabled = false;
		if(_c.gameObject.GetComponent<NoteAI>())							// if we hit a note despawn it
			_c.gameObject.GetComponent<NoteAI>().IsEnabled = false;
	}
}
