using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPartController : MonoBehaviour
{

    public bool mined;

    public bool Mined
    {
        get { return mined; }
        set { mined = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        mined = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (mined)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

}
