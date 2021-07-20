using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPartController : MonoBehaviour
{

    public bool mined;
    public bool visited;
    public bool marked;
    public bool opened;

    public int posX, posY, posZ;

    public List<GroundController> observers;

    // Start is called before the first frame update
    void Start()
    {
        visited = false;
        marked = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            notifyObservers(this.gameObject, EventsEnum.MouseLeftClick);

        }
        else if (Input.GetMouseButtonDown(1))
        {
            notifyObservers(this.gameObject, EventsEnum.MouseRightClick);
        }
    }

    //private void OnMouseDown()
    //{

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        notifyObservers(this.gameObject, EventsEnum.MouseLeftClick);

    //    }
    //    else if (Input.GetMouseButtonDown(1))
    //    {
    //        notifyObservers(this.gameObject, EventsEnum.MouseRightClick);
    //    }
    //}

    private void notifyObservers(GameObject obj, EventsEnum gameEvent)
    {
        foreach (GroundController observer in observers)
        {
            observer.notifyClick(this.gameObject, gameEvent);
        }
    }

    public void registerObservers(GroundController observer)
    {
        observers.Add(observer);
    }

}
