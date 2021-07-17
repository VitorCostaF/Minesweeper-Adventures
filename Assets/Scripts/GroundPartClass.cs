using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPartClass : MonoBehaviour
{
    private bool mined = false;
    private GameObject groundPartGameObject;

    public GameObject GroundPartGameObject
    {
        get { return groundPartGameObject; }
        set { groundPartGameObject = value; }
    }

    public bool Mined
    {
        get { return mined; }
        set { mined = value; }
    }

    public void notify()
    {
        Destroy(groundPartGameObject);
    }
}