using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnStart : MonoBehaviour
{
    [SerializeField] protected List<GameObject> objectList;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject obj in objectList) {
            obj.SetActive(true);
        }
    }
}
