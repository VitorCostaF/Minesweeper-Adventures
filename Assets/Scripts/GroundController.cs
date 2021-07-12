using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public int height, width, depth;
    public GameObject groundPart;
    public float offsetX, offsetY, offsetZ;

    void Start()
    {
        generateStandardGround(height, width, depth);
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
                }
            }
        }
    }
}
