using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPartController : MonoBehaviour
{

    public bool mined;

    public int posX, posY, posZ;

    public List<GroundController> observers;

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
        foreach (GroundController observer in observers)
        {
            observer.notifyClick(this.gameObject);
        }
    }

    public void registerObservers(GroundController observer)
    {
        observers.Add(observer);
    }

}
