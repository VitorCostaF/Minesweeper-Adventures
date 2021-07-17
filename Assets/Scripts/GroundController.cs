using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GroundController : MonoBehaviour
{
    public int width, height, depth;
    public int maxBombs;
    public GameObject groundPart;
    public float offsetX, offsetY, offsetZ;
    public GameObject player;
    private GameObject[,,] groundObjects;

    void Start()
    {
        maxBombs = 100;
        groundObjects = new GameObject[width, height, depth];
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
                    GroundPartController groundPartController = instGroundPart.GetComponent<GroundPartController>();
                    groundPartController.registerObservers(this);
                    groundPartController.posX = x;
                    groundPartController.posY = y;
                    groundPartController.posZ = z;
                    groundObjects[x, y, z] = instGroundPart;

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

            GameObject groundPart = groundObjects[x, y, z];
            GroundPartController groundPartContr = groundPart.GetComponent<GroundPartController>();

            if (!groundPartContr.mined)
            {
                groundPartContr.mined = true;
                groundPart.GetComponent<Renderer>().material.color = Color.red;
                bombsPlaced++;
            }
        }
    }

    public void notifyClick(GameObject groundPart)
    {
        int bombs;
        GroundPartController gpController = groundPart.GetComponent<GroundPartController>();
        
        bombs = bombsInNeighborhood(gpController.posX, gpController.posY, gpController.posZ);
        showTextBombs(groundPart, bombs);
        //Destroy(groundPart);
    }

    private void showTextBombs(GameObject groundPart, int bombs)
    {
        GameObject texts = groundPart.transform.Find("Texts").gameObject;
        texts.SetActive(true);
        for(int i = 0; i < texts.transform.childCount; i++)
        {
            GameObject text =  texts.transform.GetChild(i).gameObject;
            TextMeshPro textMesh = text.GetComponent<TextMeshPro>();
            textMesh.text = bombs.ToString();
        }

    }

    public int bombsInNeighborhood(int x, int y, int z)
    {
        int bombs = 0;
        List<int[]> neighbors = neighborhood(x, y, z);

        int i = 0;
        foreach (int[] neighbor in neighbors)
        {
            i++;
            GameObject groundPart = groundObjects[neighbor[0], neighbor[1], neighbor[2]];

            if(groundPart != null)
            {
                if (groundPart.GetComponent<GroundPartController>().mined)
                {
                    bombs++;
                }
            }
        }

        return bombs;
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
                        if(!( i == 0 && j == 0 && k == 0))
                        {
                            neighbors.Add(new int[] { x + i, y + j, z + k });
                        }
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