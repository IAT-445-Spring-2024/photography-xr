using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript: MonoBehaviour {
    [SerializeField] private Material metalic;
    [SerializeField] private Material black;
    [SerializeField] private GameObject target;

    private void Update() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000)) {
            MeshRenderer renderer = target.GetComponent<MeshRenderer>();
            if (hit.collider && hit.collider.gameObject == target) {
                if (Vector3.Distance(hit.collider.transform.position, transform.position) < 1) {
                    renderer.material = black;
                } else {
                    renderer.material = metalic;
                }
            }
        }
    }
}
