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

    public Text gameOverText;
    public Text bombsText, marksText;

    public Material groundPartMt, warningMt;

    public GameObject player;
    public GameObject groundPart;
    private GameObject[,,] groundObjects;

    void Start()
    {
        GetFieldDimensions();
        groundObjects = new GameObject[width, height, depth];
        ResetGame();    
    }

    // Update is called once per frame
    void Update()
    {
        if(markedBombs == maxBombs)
        {
            GameManager.Instance.win = true;
            GameManager.Instance.gameOver = true;
        }

        if(GameManager.Instance.gameOver && GameManager.Instance.win)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "You Win!!";
        }

        if (GameManager.Instance.gameOver && !GameManager.Instance.win)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "Game Over";
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
        GameManager.Instance.gameOver = false;
        GameManager.Instance.win = false;
        gameOverText.gameObject.SetActive(false);
        maxBombs = GameManager.Instance.bombs;
        bombsText.text = "Bombas: " + maxBombs;
        marksText.text = "Marca��es: " + 0;
        markedBombs = 0;
        markedFields = 0;
        GenerateStandardGround(height, width, depth);
        PlaceBombs();
        player.GetComponent<CharacterControls>().resetPlayerPosition();
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

    private void PlaceBomb(int x, int y, int z)
    {

        GameObject groundPart = groundObjects[x, y, z];
        GroundPartController groundPartContr = groundPart.GetComponent<GroundPartController>();

        if (!groundPartContr.mined)
        {
            groundPartContr.mined = true;
            ChangeGroundColor(groundPart, new Color(4, 0, 0.5f, 1));
        }
    }

    public void NotifyClick(GameObject gameObject, EventsEnum gameEvent)
    {
        if (GameManager.Instance.gameOver)
            return;

        if(Vector3.Distance(player.transform.position, gameObject.transform.position) > playerMinDistance)
            return;

        //playerPivot.transform.DOLookAt(gameObject.transform.position, 1).SetLoops(2, LoopType.Yoyo);

        //player.transform.DOMove(gameObject.transform.position, 1).SetLoops(2, LoopType.Yoyo);

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
        else if(gameEvent == EventsEnum.MouseRightClick)
        {

            if (groundPartContr.opened)
            {
                if (Input.GetKey(KeyCode.LeftControl))
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

        bombs = BombsInNeighborhood(neighborsPos);
        ShowTextBombs(groundPart, bombs);
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

    private void ShowTextBombs(GameObject groundPart, int bombs)
    {
        GroundPartController controller = groundPart.GetComponent<GroundPartController>();
        if (controller.mined || controller.marked)
        {
            return;
        }

        controller.opened = true;

        GameObject texts = groundPart.transform.Find("Texts").gameObject;
        texts.SetActive(true);
        for(int i = 0; i < texts.transform.childCount; i++)
        {
            GameObject text =  texts.transform.GetChild(i).gameObject;
            TextMeshPro textMesh = text.GetComponent<TextMeshPro>();
            textMesh.text = bombs.ToString();
            if(bombs == 9 || bombs == 6)
                textMesh.fontStyle = FontStyles.Underline;
        }

    }

    public int BombsInNeighborhood(List<int[]> neighbors)
    {
        int bombs = 0;

        foreach (int[] neighbor in neighbors)
        {
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
    private void ChangeGroundColor(GameObject gameObject, Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;
    }

}