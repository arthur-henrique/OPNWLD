using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatafomrPlayer : MonoBehaviour
{
    [SerializeField]
    private WaypointPath _waypointPath;

    [SerializeField]
    private float _speed;

    private int _targetWaypointIndex;

    private Transform _previousWaypoint;
    private Transform _targetWaypoint;
    public bool playerColindindo;

    private float _timeToWaypoint;
    private float _elapsedTime;
    private Transform originalParent;
    void Start()
    {
        TargetNextWaypoint();
        playerColindindo = true;
        StartCoroutine(DesligarPlayerColidindo());
    }
    IEnumerator DesligarPlayerColidindo()
    {
        yield return new WaitForSeconds(1f);
        playerColindindo = false;
    }

    void FixedUpdate()
    {
      

            _elapsedTime += Time.deltaTime;

             float elapsedPercentage = _elapsedTime / _timeToWaypoint;
             elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
             transform.position = Vector3.Lerp(_previousWaypoint.position, _targetWaypoint.position, elapsedPercentage);
             transform.rotation = Quaternion.Lerp(_previousWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage);
        if (playerColindindo)
        {

             if (elapsedPercentage >= 1)
             {
                 TargetNextWaypoint();
                
             }
        }
        
    }

    private void TargetNextWaypoint()
    {
        _previousWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);
        _targetWaypointIndex = _waypointPath.GetNextWaypointIndex(_targetWaypointIndex);
        _targetWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);

        _elapsedTime = 0f;

        float distanceToWaypoint = Vector3.Distance(_previousWaypoint.position, _targetWaypoint.position);
        _timeToWaypoint = distanceToWaypoint / _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("PlayerEntru");
            //originalParent = other.transform.parent;
            //other.transform.SetParent(transform);
            playerColindindo = true;
            StartCoroutine(DesligarPlayerColidindo());
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //other.transform.SetParent(originalParent);
            print("HasExited");
           
        }
    }
}