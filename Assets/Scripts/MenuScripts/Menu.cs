using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    
    public GameObject levelButtons;
    public GameObject difficultButton;
    public GameObject minesEasy, minesNormal, minesHard;
    public Text title;

    void Start()
    {
        title.color = Color.white;
        addLevelButtonListeners();
        GameManager.Instance.difficult = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void addLevelButtonListeners()
    {
        int children = levelButtons.transform.childCount;
        minesEasy.SetActive(true);
        minesNormal.SetActive(false);
        minesHard.SetActive(false);
        for (int i = 0; i < children; i++)
        {
            Button button = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>() ;
            if(button.name.StartsWith("Difficult"))
            {
                button.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Dificuldade Fácil";
                button.onClick.AddListener(() => changeDificult());
            } 
            else
            {
                button.onClick.AddListener(() => levelButtonClicked(button.name));
            }
        }
    }

    private void levelButtonClicked(string levelStr)
    {
        int level = Int32.Parse(levelStr[levelStr.Length - 1].ToString());

        GameManager.Instance.level = level;
        
        int difPlus = GameManager.Instance.difficult + 1;
        switch (level)
        {
            case 1:
                setFieldDimension(5 * difPlus, 1 ,5 * difPlus);
                GameManager.Instance.bombs = 5 * (int)Mathf.Pow(difPlus,3);
                break;
            case 2:
                setFieldDimension(10 * difPlus, 1, 10 * difPlus);
                GameManager.Instance.bombs = 20 * (int)Mathf.Pow(difPlus, 3);
                break;
            case 3:
                setFieldDimension(20 * difPlus, 1, 20 * difPlus);
                GameManager.Instance.bombs = 100 * (int)Mathf.Pow(difPlus, 3);
                break;
            case 4:
                setFieldDimension(4 * difPlus, 2, 4 * difPlus);
                GameManager.Instance.bombs = 3 * (int)Mathf.Pow(difPlus, 3);
                break;
            case 5:
                setFieldDimension(7 * difPlus, 2, 7 * difPlus);
                GameManager.Instance.bombs = 10 * (int)Mathf.Pow(difPlus, 3);
                break;
            case 6:
                setFieldDimension(10 * difPlus, 2, 10 * difPlus);
                GameManager.Instance.bombs = 40 * (int)Mathf.Pow(difPlus, 3);
                break;
            case 7:
                setFieldDimension(3 * difPlus, 3, 3 * difPlus);
                GameManager.Instance.bombs = 4 * (int)Mathf.Pow(difPlus, 3);
                break;
            case 8:
                setFieldDimension(5 * difPlus, 3, 5 * difPlus);
                GameManager.Instance.bombs = 10 * (int)Mathf.Pow(difPlus, 3);
                break;
            default:
                setFieldDimension(10 * difPlus, 3, 10 * difPlus);
                GameManager.Instance.bombs = 30 * (int)Mathf.Pow(difPlus, 3);
                break;
        }
            
        SceneManager.LoadScene("Game");
    }

    private void changeDificult()
    {
        GameManager.Instance.difficult = (GameManager.Instance.difficult + 1)%3;
        Text buttonText = difficultButton.transform.GetChild(0).gameObject.GetComponent<Text>();
        if (GameManager.Instance.difficult == 0)
        {
            title.color = Color.white;
            buttonText.text = "Dificuldade Fácil";
            minesEasy.SetActive(true);
            minesNormal.SetActive(false);
            minesHard.SetActive(false);

        }
        else if (GameManager.Instance.difficult == 1)
        {
            title.color = Color.yellow;
            buttonText.text = "Dificuldade Normal";
            minesEasy.SetActive(false);
            minesNormal.SetActive(true);
            minesHard.SetActive(false);
        }
        else
        {
            title.color = Color.red;
            buttonText.text = "Dificuldade Difícil";
            minesEasy.SetActive(false);
            minesNormal.SetActive(false);
            minesHard.SetActive(true);
        }
    }

    private void setFieldDimension(int width, int height, int depth )
    {
        GameManager.Instance.width = width;
        GameManager.Instance.height = height;
        GameManager.Instance.depth = depth;
    }

}
