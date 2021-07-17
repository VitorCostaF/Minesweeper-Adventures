using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPartController : MonoBehaviour
{

    public List<GroundPartClass> observers;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        foreach(GroundPartClass observer in observers)
        {
            observer.notify();
        }
        Destroy(this.gameObject);
    }

    public void registerObservers(GroundPartClass observer)
    {
        observers.Add(observer);
    }

}
