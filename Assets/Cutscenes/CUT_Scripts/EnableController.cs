using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableController : MonoBehaviour
{
    public GameObject Obj;

    public void enable(bool active)
    {
        Obj.SetActive(active);
    }

    public void move(GameObject refObj)
    {
        Vector3 pos = refObj.transform.position;
        Quaternion rot = refObj.transform.rotation;
        //Obj.transform.SetPositionAndRotation(pos, rot);
        Obj.transform.position = pos;
        Obj.transform.rotation = rot;
    }
}
