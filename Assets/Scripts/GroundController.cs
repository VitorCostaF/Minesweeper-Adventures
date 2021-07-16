using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public int width, height, depth;
    public int maxBombs;
    public GameObject groundPart;
    public float offsetX, offsetY, offsetZ;
    public GameObject player;
    private GameObject[,,] groundObjects;
    private GroundPartClass[,,] groundObjectsClass;

    void Start()
    {
        maxBombs = 250;
        groundObjects = new GameObject[width, height, depth];
        groundObjectsClass = new GroundPartClass[width, height, depth];
        generateStandardGround(height, width, depth);
        placeBombs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void generateStandardGround(int height, int width, int depth)
    {
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    Vector3 pos = new Vector3(x + offsetX, y + offsetY, z + offsetZ);
                    GameObject instGroundPart = Instantiate<GameObject>(groundPart, pos, Quaternion.identity, transform);
                    instGroundPart.name = "GrondPart" + x + y + z;
                    groundObjects[x, y, z] = instGroundPart;
                    groundObjectsClass[x, y, z] = new GroundPartClass();
                }
            }
        }

        Vector3 playerPos = new Vector3((width / 2) + offsetX, height + offsetY, (depth / 2) + offsetZ);

        player.transform.position = playerPos;
        player.SetActive(true);

    }

    private void placeBombs()
    {
        int bombsPlaced = 0;

        while(bombsPlaced < maxBombs)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            int z = Random.Range(0, depth);

            GroundPartClass groundPartClass = groundObjectsClass[x, y, z];

            if (!groundPartClass.Mined)
            {
                groundObjects[x, y, z].GetComponent<Renderer>().material.color = Color.red;
                groundPartClass.Mined = true;
                bombsPlaced++;
            }
        }
    }

}

class GroundPartClass
{
    private bool mined = false;

    public bool Mined
    {
        get { return mined; }
        set { mined = value; }
    }
}