using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshDirector : MonoBehaviour
{
    protected NavMeshSurface navMeshSurface;
    // Start is called before the first frame update
    void Awake() {
        navMeshSurface = this.GetComponent<NavMeshSurface>();

        navMeshSurface.BuildNavMesh();
    }

}
