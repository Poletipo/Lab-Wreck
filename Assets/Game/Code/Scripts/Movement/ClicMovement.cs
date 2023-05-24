using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClicMovement : MonoBehaviour {

    [SerializeField] NavMeshAgent _agent;
    Plane _plane = new Plane(Vector3.up, 0);
    Vector3 _worldMousePosition;

    NavMeshPath _navMeshPath;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectMovePosition(Vector2 mousePosition)
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (_plane.Raycast(ray, out distance)) {
            _worldMousePosition = ray.GetPoint(distance);

            if (CalculateNewPath(_worldMousePosition)) {
                _agent.SetDestination(_worldMousePosition);
            }

        }
    }

    bool CalculateNewPath(Vector3 targetPosition)
    {
        _agent.CalculatePath(targetPosition, _navMeshPath);
        if (_navMeshPath.status != NavMeshPathStatus.PathComplete) {
            return false;
        }
        else {
            return true;
        }
    }




}
