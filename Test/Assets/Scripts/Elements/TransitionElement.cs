﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionElement : GameElement {

	public int id;
	public PlaceElement[] preconditions;
	public int[] preconditionCoefficients;
	public PlaceElement[] postconditions;
	public int[] postconditionCoefficients;
	public GameObject arcPrefab;
	public GameObject transitionExplosion;

	// Create arcs corresponding to given pre/postconditions
	void Start()
	{
		ArcElement arc = arcPrefab.GetComponent<ArcElement>();
		arc.transition = this;
		int i = 0;
		foreach (PlaceElement p in preconditions)
		{
			arc.place = p;
			arc.type = ArcElement.ConditionType.PRECONDITION;
			arc.coeff = preconditionCoefficients[i];
			++i;
			Instantiate(arc, this.transform);
		}
		i = 0;
		foreach (PlaceElement p in postconditions)
		{
			arc.place = p;
			arc.type = ArcElement.ConditionType.POSTCONDITION;
			arc.coeff = postconditionCoefficients[i];
			++i;
			Instantiate(arc, this.transform);
		}
	}

	void OnMouseUp(){
		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
		if (hit.collider!=null) {
			if (hit.collider.gameObject.Equals (this.gameObject)) {
				Debug.Log ("Transition clicked.");
				game.controller.OnTransitionClicked (id);
			} else {
				if (hit.collider.gameObject.CompareTag("Place")) {
					Debug.Log ("call addPostcondition with idTransition: "+this.id+" and idPlace: "+(hit.collider.gameObject.GetComponent<PlaceElement>()).id);
				}
			}

		}
	}

	// Display animation
	public void FireTransition()
	{
		Instantiate(transitionExplosion, transform);
	}

}