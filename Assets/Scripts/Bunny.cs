using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bunny : MonoBehaviour {
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float searchRange = 10.0f;
    private float timer = 0f;

    private void Update() {
        if (timer == 0f) {
            Vector3 destination;
            if (RandomPoint(transform.position, searchRange, out destination)) {
                navMeshAgent.SetDestination(destination);
            }
            timer += Time.deltaTime;
        } else {
            if (timer > 5f) {
                timer = 0f;
            } else {
                timer += Time.deltaTime;
            }
        }
    }

    // https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
    private bool RandomPoint(Vector3 center, float range, out Vector3 result) {
        for (int i = 0; i < 30; i++) {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
