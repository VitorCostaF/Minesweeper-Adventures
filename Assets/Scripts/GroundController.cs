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
                    groundObjectsClass[x, y, z] = new GroundPartClass();
                    groundObjectsClass[x, y, z].GroundPartGameObject = instGroundPart ; 
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
                groundPartClass.GroundPartGameObject.GetComponent<Renderer>().material.color = Color.red;
                groundPartClass.Mined = true;
                bombsPlaced++;
            }
        }
    }

    private List<int[]> neighborhood(int x, int y, int z)
    {
        List<int[]> neighbors = new List<int[]>();

        for(int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                for (int k = -1; k < 2; k++)
                {
                    if( (0 <= x + i && x + i < width ) &&
                        (0 <= y + j && y + j < height) &&
                        (0 <= z + k && z + k < depth))
                    {
                        neighbors.Add(new int[] { x + i, y + j, z + k });
                    }
                }
            }
        }
        return neighbors;
    }

}


//class GroundPartClass
//{
//    private bool mined = false;
//    private GameObject groundPartGameObject;

//    public GameObject GroundPartGameObject
//    {
//        get { return groundPartGameObject; }
//        set { groundPartGameObject = value; }
//    }

//    public bool Mined
//    {
//        get { return mined; }
//        set { mined = value; }
//    }
//}