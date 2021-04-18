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
}
