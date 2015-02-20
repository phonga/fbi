using UnityEngine;
using System.Collections;

public class FBIController : MonoBehaviour {

	public enum DIRECTION {
		SOUTH = 0,
		EAST = 1,
		WEST = 2,
		NORTH = 3
	};

	public DIRECTION direction = 0;

	private Animator animator = null;
	private Vector3 waypoint = Vector3.zero;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		this.setDirection (this.direction);
	}
	
	// Update is called once per frame
	void Update () {
		var vertical = Input.GetAxis("Vertical");
		var horizontal = Input.GetAxis("Horizontal");

		GameObject Player = GameObject.FindGameObjectWithTag ("Player");

		if (vertical > 0) {
			this.setDirection(DIRECTION.NORTH);
		} else if (vertical < 0) {
			this.setDirection(DIRECTION.SOUTH);
		} else if (horizontal > 0) {
			this.setDirection(DIRECTION.WEST);
		} else if (horizontal < 0) {
			this.setDirection(DIRECTION.EAST);
		}

		Vector3 currentPosition = transform.position;
		Vector3 moveDirection = currentPosition;

		waypoint = Player.transform.position;

		if (waypoint != Vector3.zero) {
			Vector3 traverse = waypoint - currentPosition;
			if (traverse.magnitude < 0.5) {
				waypoint = Vector3.zero;
			}
		}

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

			DIRECTION direction = (DIRECTION)Random.Range(0, 4);

			while (direction == this.direction) {
				direction = (DIRECTION)Random.Range(0, 4);
			}

			this.setDirection(direction);

			switch(this.direction) {
			case DIRECTION.SOUTH:
				waypoint = new Vector3(currentPosition.x, currentPosition.y - 5);
				break;
			case DIRECTION.EAST:
				waypoint = new Vector3(currentPosition.x + 5, currentPosition.y);
				break;
			case DIRECTION.NORTH:
				waypoint = new Vector3(currentPosition.x, currentPosition.y + 5);
				break;
			case DIRECTION.WEST:
				waypoint = new Vector3(currentPosition.x - 5, currentPosition.y);
				break;
			}
		}


		moveDirection = waypoint - currentPosition;
		moveDirection.z = 0; 
		moveDirection.Normalize ();

		Vector3 target = moveDirection * 2 + currentPosition;
//		transform.position = Vector3.Lerp( currentPosition, target, Time.deltaTime );
	}

	void OnMouseDrag() {
		Debug.Log("MouseDrag");
	}

	void OnTriggerEnter2D( Collider2D other )
	{
		if (this.animator != null) {
			DIRECTION direction = (DIRECTION)Random.Range (0, 4);
			
			while (direction == this.direction) {
				direction = (DIRECTION)Random.Range (0, 4);
			}

			this.setDirection (direction);
			waypoint = Vector3.zero;
		}
	}

	private void setDirection(DIRECTION direction) {
		this.direction = direction;
		this.animator.SetInteger ("direction", (int)this.direction);
	}
}
