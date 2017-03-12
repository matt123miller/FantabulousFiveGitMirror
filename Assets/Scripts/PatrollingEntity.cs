using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class PatrollingEntity : MonoBehaviour {

    private NavMeshAgent _agent;
    private float _patrolDistance;

    public Transform[] waypoints;
    public Transform currentWaypoint;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
