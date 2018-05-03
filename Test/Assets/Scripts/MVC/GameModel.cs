﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place
{
    public int id;
    public int marking;
    public Place(int id, int marking)
    {
        this.id = id;
        this.marking = marking;
    }
}

public class Arc
{
    public int idPlace;
    public int coeff;
    public Arc(int idPlace, int coeff)
    {
        this.idPlace = idPlace;
        this.coeff = coeff;
    }
}

public class Transition
{
    public int id;
    public List<Arc> preconditions;
    public List<Arc> postconditions;

    public Transition(int id, List<Arc> preconditions, List<Arc> postconditions)
    {
        this.id = id;
        this.preconditions = preconditions;
        this.postconditions = postconditions;
    }

    public bool isEnabled(List<Place> places)
    {
        foreach (Arc precondition in preconditions)
        {
            foreach (Place place in places)
            {
                if (precondition.idPlace == place.id)
                {
                    if (precondition.coeff > place.marking)
                    {
                        Debug.Log(precondition.coeff);
                        Debug.Log(place.marking);
                        return false;
                    }
                    break;
                }
            }
        }
        return true;
    }

    public List<Place> fire(List<Place> places)
    {
        foreach (Place place in places)
        {
            foreach (Arc precondition in preconditions)
            {
                if (precondition.idPlace == place.id)
                {
                    place.marking = place.marking - precondition.coeff;
                    break;
                }
            }
            foreach (Arc postcondition in postconditions)
            {
                if (postcondition.idPlace == place.id)
                {
                    place.marking = place.marking + postcondition.coeff;
                    break;
                }
            }
        }
        return places;
    }
}

public class GameModel : GameElement
{
    private List<Place> places;
    private List<Transition> transitions;

	private List<Place> targetPlaces;

	/*public void initialisation(List<Place> places, List<Transition> transitions, List<Place> targetPlaces)
    {
        this.places = places;
        this.transitions = transitions;
		this.targetPlaces = targetPlaces;
    }*/

	public void initialisation(List<Place> places, List<Transition> transitions)
	{
		this.places = places;
		this.transitions = transitions;
	}

    public List<Place> performFire(int id)
    {
        foreach (Transition transition in transitions)
        {
            if (transition.id == id)
            {
                places = transition.fire(places);
                break;
            }
        }
        return places;
    }

    public bool canPerformFire(int id)
    {
        foreach (Transition transition in transitions)
        {
            if (transition.id == id)
            {
                return transition.isEnabled(places);
            }
        }
        return false;
    }

	public bool canPerformAddArc(int idTransition, int idPlace){
		bool transitionExists = false;
		bool placeExists = false;
		foreach (Transition transition in transitions) {
			if (transition.id == idTransition) {
				transitionExists = true;
			}
		}
		foreach(Place place in places){
			if (place.id == idPlace) {
				placeExists = true;
			}
		}
		return transitionExists && placeExists;
	}

	public void performAddArc(int idTransition, int idPlace, bool precondition){
        int coeff = 1;
		foreach (Transition transition in transitions) {
			if (transition.id == idTransition) {
				if(precondition) {
                    foreach (Arc arc in transition.preconditions) // If arc exists already, remove
                    {
                        if (arc.idPlace == idPlace)
                        {
                            transition.preconditions.Remove(arc);
                        }
                    }
                    transition.preconditions.Add (new Arc (idPlace, coeff));
				} else {
                    foreach (Arc arc in transition.preconditions) // If arc exists already, remove
                    {
                        if (arc.idPlace == idPlace)
                        {
                            transition.preconditions.Remove(arc);
                        }
                    }
                    transition.postconditions.Add (new Arc (idPlace, coeff));
				}
			}
		}
	}

    public List<Place> getPlaces()
    {
        return this.places;
	}

	public bool targetPetriNetReached(){
		for (int i = 0; i < places.Count; i++) {
			if(!places[i].id.Equals(targetPlaces[i].id) || !places[i].marking.Equals(targetPlaces[i].marking)){
				return false;
			}
		}
		return true;
	}

	/*void Start(){
		places = new List<Place> ();
		transitions = new List<Transition> ();
		places.Add (new Place(1,1));
		places.Add (new Place(2,0));
		transitions.Add (new Transition (1, new List<Arc>(), new List<Arc>()));
		performAddArc (1, 1, 1, true);
		performAddArc (1, 2, 1, false);
		targetPlaces = new List<Place> ();
		targetPlaces.Add (new Place(1,0));
		targetPlaces.Add (new Place(2,1));
		Debug.Log (targetPetriNetReached());

		if (canPerformFire (1)) {
			performFire (1);
		}
		Debug.Log (targetPetriNetReached());
	}*/
}