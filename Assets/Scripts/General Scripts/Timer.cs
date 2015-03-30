using UnityEngine;

public struct Timer{

	//The time to wait for
	private float _goalTime;

	//returns true if enough time passed
	public bool isDone {
		get{ return Time.time >= _goalTime; }
	}

	//gets current difference in time between now and goal
	//- is before goal; + is after goal
	public float getTime {
		get{ return Time.time - _goalTime; }
	}

	//Sets how long the timer should time for
	public void SetTimer(float length){
		_goalTime = length + Time.time;
	}

}

