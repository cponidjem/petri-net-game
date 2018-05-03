﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : GameElement {
	PlaceElement[] places;
	TransitionElement[] transitions;

	void Start(){
		places = new PlaceElement[this.GetComponentsInChildren<PlaceElement>().Length];
		transitions = new TransitionElement[this.GetComponentsInChildren<TransitionElement>().Length];

		foreach (PlaceElement p in this.GetComponentsInChildren<PlaceElement>())
		{
			places[p.id] = p;
		}

		foreach (TransitionElement t in this.GetComponentsInChildren<TransitionElement>())
		{
			transitions[t.id] = t;
		}
	}

    // Effectuate the transition.
	public void updateGraphics(List<Place> newPlaces)
    {
		foreach (Place newPlace in newPlaces) {
			places [newPlace.id].changeMarking (newPlace.marking);
		}
    }

	public void transitionAnimation(int transitionId){
		transitions [transitionId].FireTransition ();
	}

    // Return all places of the scene.
    public List<Place> getPlaces()
    {
        List<Place> places = new List<Place>();
        foreach (PlaceElement p in this.GetComponentsInChildren<PlaceElement>())
        {
			places.Add(new Place(p.id, p.initialMarking));
        }
        return places;
    }

    // Return all transitions in the scene.
    public List<Transition> getTransitions()
    {
        List<Transition> transitions = new List<Transition>();
        foreach (TransitionElement t in this.GetComponentsInChildren<TransitionElement>())
        {
            List<Arc> preconditions = new List<Arc>();
            List<Arc> postconditions = new List<Arc>();
            foreach (ArcElement a in this.GetComponentsInChildren<ArcElement>())
            {
                if (a.type == ArcElement.ConditionType.POSTCONDITION)
                {
                    postconditions.Add(new global::Arc(a.place.id, a.coeff));
                }
                else
                {
                    preconditions.Add(new global::Arc(a.place.id, a.coeff));
                }
            }
            transitions.Add(new Transition(t.id, preconditions, postconditions));
        }
        return transitions;
    }


}