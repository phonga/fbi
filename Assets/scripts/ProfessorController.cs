using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProfessorController : MonoBehaviour {

	public enum DIRECTION {
		SOUTH = 0,
		EAST = 1,
		WEST = 2,
		NORTH = 3
	};

	public DIRECTION direction;

	private Animator animator = null;
	private Vector3 waypoint = Vector3.zero;
	private List<Vector3> waypoints = new List<Vector3>();
	private Vector3 lastpoint = Vector3.zero;
	private Vector3 lastMoveDirection = Vector3.zero;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		this.setDirection (this.direction);
	}
	
	// Update is called once per frame
	void Update () {
		var vertical = Input.GetAxis("Vertical");
		var horizontal = Input.GetAxis("Horizontal");

		Vector3 currentPosition = transform.position;
		Vector3 moveDirection = currentPosition;

		
		if (vertical > 0) {
			this.setDirection(DIRECTION.NORTH);
		} else if (vertical < 0) {
			this.setDirection(DIRECTION.SOUTH);
		} else if (horizontal > 0) {
			this.setDirection(DIRECTION.EAST);
		} else if (horizontal < 0) {
			this.setDirection(DIRECTION.WEST);
		}

		GameObject FBI = GameObject.FindGameObjectWithTag ("Enemy");

		Camera mainCamera = Camera.main;
		Vector3 cameraPosition = mainCamera.transform.position;
		float xDist = mainCamera.aspect * mainCamera.orthographicSize; 
		float xMax = cameraPosition.x + xDist;
		float xMin = cameraPosition.x - xDist;
		float yMax = mainCamera.orthographicSize;

		if (currentPosition.y < -yMax || currentPosition.y > yMax) {
			waypoint = Vector3.zero;
		}
		
		if (currentPosition.x < xMin || currentPosition.x > xMax) {
			waypoint = Vector3.zero;
		}
		
		
		if (waypoint == Vector3.zero) {

			float distance = 0;

			Vector3 newWayPoint = new Vector3(currentPosition.x, currentPosition.y - 5);
			if (Vector3.Distance(FBI.transform.position, newWayPoint) > distance) {
				waypoint = newWayPoint;
				distance = Vector3.Distance(FBI.transform.position, newWayPoint);
				this.setDirection(DIRECTION.SOUTH);
			}

			newWayPoint = new Vector3(currentPosition.x, currentPosition.y + 5);
			if (Vector3.Distance(FBI.transform.position, newWayPoint) > distance) {
				waypoint = newWayPoint;
				distance = Vector3.Distance(FBI.transform.position, newWayPoint);
				this.setDirection(DIRECTION.NORTH);
			}

			newWayPoint = new Vector3(currentPosition.x - 5, currentPosition.y);
			if (Vector3.Distance(FBI.transform.position, newWayPoint) > distance) {
				waypoint = newWayPoint;
				distance = Vector3.Distance(FBI.transform.position, newWayPoint);
				this.setDirection(DIRECTION.WEST);
			}

			newWayPoint = new Vector3(currentPosition.x + 5, currentPosition.y);
			if (Vector3.Distance(FBI.transform.position, newWayPoint) > distance) {
				waypoint = newWayPoint;
				distance = Vector3.Distance(FBI.transform.position, newWayPoint);
				this.setDirection(DIRECTION.EAST);
			}

			newWayPoint = new Vector3(currentPosition.x + 5, currentPosition.y + 5);
			if (Vector3.Distance(FBI.transform.position, newWayPoint) > distance) {
				waypoint = newWayPoint;
				distance = Vector3.Distance(FBI.transform.position, newWayPoint);
				this.setDirection(DIRECTION.EAST);
			}

			newWayPoint = new Vector3(currentPosition.x - 5, currentPosition.y - 5);
			if (Vector3.Distance(FBI.transform.position, newWayPoint) > distance) {
				waypoint = newWayPoint;
				distance = Vector3.Distance(FBI.transform.position, newWayPoint);
				this.setDirection(DIRECTION.WEST);
			}

			newWayPoint = new Vector3(currentPosition.x + 5, currentPosition.y - 5);
			if (Vector3.Distance(FBI.transform.position, newWayPoint) > distance) {
				waypoint = newWayPoint;
				distance = Vector3.Distance(FBI.transform.position, newWayPoint);
				this.setDirection(DIRECTION.EAST);
			}
			
			newWayPoint = new Vector3(currentPosition.x - 5, currentPosition.y + 5);
			if (Vector3.Distance(FBI.transform.position, newWayPoint) > distance) {
				waypoint = newWayPoint;
				distance = Vector3.Distance(FBI.transform.position, newWayPoint);
				this.setDirection(DIRECTION.WEST);
			}


			if (waypoint.y < -yMax || waypoint.y > yMax) {
				waypoint.y = -waypoint.y;
			}

			if (currentPosition.x < xMin || currentPosition.x > xMax) {
				waypoint.x = -waypoint.x;
			}
		}

		moveDirection = waypoint - currentPosition;

		moveDirection.z = 0; 
		moveDirection.Normalize ();
		
		Vector3 target = moveDirection * 3 + currentPosition;
//		transform.position = Vector3.Lerp( currentPosition, target, Time.deltaTime );

		/**
		 * Waypoints on drag
		 **/

		if (this.lastpoint == Vector3.zero) {
			this.lastpoint = currentPosition;
		}

		if (this.waypoints.Count > 1) {
			for(int i = 1; i < this.waypoints.Count; ++i) {
				Debug.DrawLine(this.waypoints[i-1], this.waypoints[i], Color.green, 500f, false);
			}
		}

		if (this.waypoints.Count > 0 && Vector3.SqrMagnitude (currentPosition - this.lastpoint) < 0.01) {
			this.lastpoint = this.waypoints [0];
			this.waypoints.RemoveAt (0);
		}


		moveDirection = this.lastpoint - currentPosition;
		moveDirection.z = 0; 
		moveDirection.Normalize ();

		this.lastMoveDirection = moveDirection;
		
		target = moveDirection * 3 + currentPosition;
		transform.position = Vector3.Lerp (currentPosition, target, Time.deltaTime);
	}

	void OnMouseDown() {
		Debug.Log ("Clearing waypoints");
		this.waypoints.Clear ();
	}

	void OnMouseDrag() {
		Vector3 point = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		point.z = 0;
		this.waypoints.Add (point);

//		Debug.Log (point.x + "," + point.y + ":" + this.waypoints.Count);
	}

	private void setDirection(DIRECTION direction) {
		this.direction = direction;
		this.animator.SetInteger ("direction", (int)this.direction);
	}
}
