using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class GroundController : MonoBehaviour
{
    public int width, height, depth;
    public int maxBombs, markedBombs, markedFields;
    public float offsetX, offsetY, offsetZ;
    public float explodeBombFactorTime;
    public float playerMinDistance;

    public int wheatPerFiled = 100;
    public int fieldsNumber = 10;

    public Text gameOverText;
    public Text bombsText, marksText, difficultText, levelText;
    public AudioClip winClip, loseClip;

    public Material groundPartMt, warningMt;

    public GameObject wheatPrefab;

    public GameObject player;
    public GameObject groundPart;
    private GameObject[,,] groundObjects;

    private bool leftControl = false;

    private AudioSource audioGameOver;

    void Start()
    {
        audioGameOver = gameObject.AddComponent<AudioSource>();
        GenerateWheatFields();
        GetFieldDimensions();
        groundObjects = new GameObject[width, height, depth];
        ResetGame();    
    }

    // Update is called once per frame
    void Update()
    {
        if(markedBombs == maxBombs && markedBombs == markedFields)
        {
            GameManager.Instance.win = true;
            GameManager.Instance.gameOver = true;
        }

        if(GameManager.Instance.gameOver && GameManager.Instance.win)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "You Win!!";
            if (GameManager.Instance.playSound)
            {
                audioGameOver.PlayOneShot(winClip);
                audioGameOver.volume = 0.1f;
                GameManager.Instance.playSound = false;
            }

        }

        if (GameManager.Instance.gameOver && !GameManager.Instance.win)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "Game Over";
            if(GameManager.Instance.playSound)
            {
                audioGameOver.PlayOneShot(loseClip);
                GameManager.Instance.playSound = false;
            }
        }
    }

    private void GetFieldDimensions()
    {
        width = GameManager.Instance.width;
        height = GameManager.Instance.height;
        depth = GameManager.Instance.depth;
    }

    private void GenerateStandardGround(int height, int width, int depth)
    {
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if(groundObjects[x, y, z] != null)
                    {
                        Destroy(groundObjects[x, y, z]);
                    }

                    Vector3 pos = new Vector3(x + offsetX - width/2, y + offsetY, z + offsetZ - depth/2);
                    GameObject instGroundPart = Instantiate<GameObject>(groundPart, pos, Quaternion.identity, transform);
                    instGroundPart.name = "GrondPart" + x + y + z;
                    GroundPartController groundPartController = instGroundPart.GetComponent<GroundPartController>();
                    groundPartController.RegisterObservers(this);
                    groundPartController.posX = x;
                    groundPartController.posY = y;
                    groundPartController.posZ = z;
                    groundObjects[x, y, z] = instGroundPart;

                }
            }
        }
    }

    public void ResetGame()
    {
        resetTexts();
        audioGameOver.Stop();
        GameManager.Instance.gameOver = false;
        GameManager.Instance.win = false;
        GameManager.Instance.playSound = true;
        maxBombs = GameManager.Instance.bombs;
        markedBombs = 0;
        markedFields = 0;
        GenerateStandardGround(height, width, depth);
        PlaceBombs();
        player.GetComponent<CharacterControls>().resetPlayerPosition();
    }

    private void resetTexts()
    {
        string difficult; 
        gameOverText.gameObject.SetActive(false);
        bombsText.text = "Bombas: " + maxBombs;
        marksText.text = "Marca��es: " + 0;
        levelText.text = "N�vel: " + GameManager.Instance.level;

        switch(GameManager.Instance.difficult)
        {
            case 0:
                difficult = "F�cil";
                break;
            case 1:
                difficult = "Normal";
                break;
            default:
                difficult = "Dif�cil";
                break;
        }

        difficultText.text = "Dificuldade: " + difficult;

    }

    private void PlaceBombs()
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
                bombsPlaced++;
            }
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void InclinateShovel(GameObject gameObject, EventsEnum gameEvent)
    {
        Vector3 playerPos = player.transform.rotation.eulerAngles;
        player.transform.DORotate(new Vector3(-50, playerPos.y, playerPos.z), 0.5f).SetLoops(2, LoopType.Yoyo).OnComplete(() => HandleClick(gameObject, gameEvent));
    }

    private void StraightShovel(GameObject gameObject, EventsEnum gameEvent)
    {
        Vector3 playerPos = player.transform.rotation.eulerAngles;
        player.transform.DORotate(new Vector3(0, playerPos.y, playerPos.z), 0.05f).OnComplete( () => InclinateShovel(gameObject, gameEvent));
    }

    public void NotifyClick(GameObject gameObject, EventsEnum gameEvent)
    {
        leftControl = false;
        if (GameManager.Instance.gameOver)
            return;

        if(Vector3.Distance(player.transform.position, gameObject.transform.position) > playerMinDistance)
            return;


        if (Input.GetKey(KeyCode.LeftControl))
            leftControl = true;

        player.transform.DOLookAt(gameObject.transform.position, 0.05f).OnComplete(() => StraightShovel(gameObject, gameEvent));

    }

    private void HandleClick(GameObject gameObject, EventsEnum gameEvent)
    {
        GroundPartController groundPartContr = gameObject.GetComponent<GroundPartController>();
        if (gameEvent == EventsEnum.MouseLeftClick)
        {
            if (groundPartContr.marked)
                return;

            if (groundPartContr.mined)
            {
                GameManager.Instance.gameOver = true;
                ExplodeBomb(gameObject);
                ExplodeAllBombs();
            }
            else
            {
                OpenGroundSafe(gameObject);
                ClearVisitedMarks();
            }
        }
        else if (gameEvent == EventsEnum.MouseRightClick)
        {

            if (groundPartContr.opened)
            {
                if (leftControl)
                    Destroy(gameObject);
                else
                    return;
            }

            if (groundPartContr.marked)
            {
                groundPartContr.marked = false;
                gameObject.GetComponent<Renderer>().sharedMaterial = groundPartMt;
                markedFields--;
                if (groundPartContr.mined)
                {
                    markedBombs--;
                }
            }
            else
            {
                groundPartContr.marked = true;
                gameObject.GetComponent<Renderer>().sharedMaterial = warningMt;
                markedFields++;
                if (groundPartContr.mined)
                {
                    markedBombs++;
                }
            }
        }
        marksText.text = "Marca��es: " + markedFields;
    }

    private void ExplodeAllBombs()
    {
        float seconds = explodeBombFactorTime;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    GameObject groundPart = groundObjects[x, y, z];
                    if (groundPart != null)
                    {
                        GroundPartController gpController = groundPart.GetComponent<GroundPartController>();
                        if(gpController.mined)
                        {
                            StartCoroutine(WaitBeforeExplodeNext(groundPart, seconds));
                            seconds += explodeBombFactorTime;
                        }
                    }
                }
            }
        }
    }

    private IEnumerator<WaitForSeconds> WaitBeforeExplodeNext(GameObject groundPart, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (groundPart != null)
            ExplodeBomb(groundPart);
    }

    private void ExplodeBomb(GameObject groundPart)
    {
        GroundPartController gpController = groundPart.GetComponent<GroundPartController>();
        gpController.ExplodeField();
        GameManager.Instance.gameOver = true;
        Destroy(groundPart);
    }

    public void ClearVisitedMarks()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    GameObject groundPart = groundObjects[x, y, z];
                    if(groundPart != null)
                    {
                        GroundPartController gpController = groundPart.GetComponent<GroundPartController>();
                        gpController.visited = false;
                    }
                }
            }
        }
    }

    public void OpenGroundSafe(GameObject groundPart)
    {
        int bombs;
        GroundPartController gpController = groundPart.GetComponent<GroundPartController>();
        List<int[]> neighborsPos = Neighborhood(gpController.posX, gpController.posY, gpController.posZ);
        int[] pos = new int[] { gpController.posX, gpController.posY, gpController.posZ };

        bombs = BombsInNeighborhood(neighborsPos);

        gpController.ShowTextBombs(bombs, TextColor(neighborsPos, pos, gpController.opened, gpController.marked));
        gpController.visited = true;


        if (bombs == 0 && !gpController.mined && !gpController.marked)
        {
            gpController.OpenSafeField();
            Destroy(groundPart);
            foreach (int[] neighborPos in neighborsPos)
            {
                GameObject neighbor = groundObjects[neighborPos[0], neighborPos[1], neighborPos[2]];

                if (neighbor != null && !neighbor.GetComponent<GroundPartController>().visited)
                {
                    OpenGroundSafe(neighbor);
                }
            }
        }
    }

    private Color TextColor(List<int[]> neighbors, int[] position, bool opened, bool marked)
    {
        Color color;
        float proximity = 10;

        if (!GameManager.Instance.cues)
        {
            return Color.white;
        }

        Vector3 pos = new Vector3(position[0], position[1], position[2]);
        List<GameObject> neighborsObj = NeighborhoodWithBomb(neighbors);
        foreach (GameObject neighbor in neighborsObj)
        {
            GroundPartController gpController = neighbor.GetComponent<GroundPartController>();
            Vector3 neighborPos = new Vector3(gpController.posX, gpController.posY, gpController.posZ);
            proximity = Mathf.Min(proximity, Vector3.Distance(neighborPos, pos));

        }

        if(proximity <= 1)
            color = Color.red;
        else if (proximity <= 1.42)
            color = new Color(1, 0.5f, 0);
        else if (proximity <= 1.74)
            color = Color.yellow;
        else
            color = Color.white;

        return color;
    }

    public int BombsInNeighborhood(List<int[]> neighbors)
    {
        return NeighborhoodWithBomb(neighbors).Count;
    }

    private List<GameObject> NeighborhoodWithBomb(List<int[]> neighbors)
    {
        List<GameObject> neighborhood = new List<GameObject>();
        foreach (int[] neighbor in neighbors)
        {
            GameObject groundPart = groundObjects[neighbor[0], neighbor[1], neighbor[2]];

            if (groundPart != null)
            {
                if (groundPart.GetComponent<GroundPartController>().mined)
                {
                    neighborhood.Add(groundPart);
                }
            }
        }
        return neighborhood;
    }

    private List<int[]> Neighborhood(int x, int y, int z)
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

    private void GenerateWheatFields()
    {
        for (int i = 0; i < fieldsNumber; i++)
        {
            float xRandom = Random.Range(-30, 30);
            float zRandom = Random.Range(-30, 30);

            if(-20 <= xRandom && xRandom <= 20 && -20 <= zRandom && zRandom <= 20)
            {
                int xz = Random.Range(0, 2);
                if(xz == 0)
                {
                    while (-20 <= xRandom  && xRandom <= 20)
                    {
                        xRandom = Random.Range(-30, 30);
                    }
                } 
                else
                {
                    while (-20 <= zRandom && zRandom <= 20)
                    {
                        zRandom = Random.Range(-30, 30);
                    }
                }
   
            }

            float widhtRandom = Random.Range(3, 5);
            float depthRandom = Random.Range(3, 5);

            GenerateWheatField(xRandom, 0, zRandom, i, widhtRandom, depthRandom);
        }
        
    }


    private void GenerateWheatField(float x, float y, float z, int number, float width, float depth)
    {
        GameObject field = new GameObject("WheatField" + number);
        field.transform.position = new Vector3(x,y,z);
        InstantiateWheats(field, width, depth);
    }

    private void InstantiateWheats(GameObject field, float width, float depth)
    {
        int wheatPerFiledRandom = Random.Range(wheatPerFiled/2, wheatPerFiled);
        Vector3 parentPosition = field.transform.position;
        for (int i = 0; i < wheatPerFiledRandom; i++)
        {
            float xWheatRandom = Random.Range(0, width);
            float zWheatRandom = Random.Range(0, depth);

            GameObject wheat = Instantiate(wheatPrefab);
            wheat.transform.parent = field.transform;
            wheat.transform.position = new Vector3(parentPosition.x + xWheatRandom, 1.8f, parentPosition.z + zWheatRandom);
        }
    }
}