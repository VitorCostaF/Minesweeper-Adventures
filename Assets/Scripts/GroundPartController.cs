using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GroundPartController : MonoBehaviour
{

    public bool mined;
    public bool visited;
    public bool marked;
    public bool opened;

    public int posX, posY, posZ;

    public GameObject explosionPrefab, openingPrefab;

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
            NotifyObservers(EventsEnum.MouseLeftClick);

        }
        else if (Input.GetMouseButtonDown(1))
        {
            NotifyObservers(EventsEnum.MouseRightClick);
        }
    }

    private void NotifyObservers(EventsEnum gameEvent)
    {
        foreach (GroundController observer in observers)
        {
            observer.NotifyClick(gameObject, gameEvent);
        }
    }

    public void RegisterObservers(GroundController observer)
    {
        observers.Add(observer);
    }
    
    public void ExplodeField()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
    }

    public void OpenSafeField()
    {
        GameObject explosion = Instantiate(openingPrefab);
        explosion.transform.position = transform.position;
    }

    public void ShowTextBombs(int bombs)
    {
        if (mined || marked)
        {
            return;
        }

        opened = true;

        GameObject texts = transform.Find("Texts").gameObject;
        texts.SetActive(true);
        for (int i = 0; i < texts.transform.childCount; i++)
        {
            GameObject text = texts.transform.GetChild(i).gameObject;
            TextMeshPro textMesh = text.GetComponent<TextMeshPro>();
            textMesh.text = bombs.ToString();
            if (bombs == 9 || bombs == 6)
                textMesh.fontStyle = FontStyles.Underline;
        }

    }

}
