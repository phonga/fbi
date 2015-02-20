using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public GameObject fbiAgent;
	public GameObject professor;

	// Use this for initialization
	void Start () {
		int direction = Random.Range (0, 3);
		GameObject agent = (GameObject)Instantiate (fbiAgent, new Vector3 (0, 0, 0), Quaternion.identity);
		
		FBIController ctrl = (FBIController)agent.GetComponent ("FBIController");
		ctrl.direction = (FBIController.DIRECTION)direction;

		GameObject prof = (GameObject)Instantiate (professor, new Vector3 (4, 4, 0), Quaternion.identity);
		ProfessorController profCtrl = (ProfessorController)prof.GetComponent ("ProfessorController");
		profCtrl.direction = (ProfessorController.DIRECTION)direction;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
