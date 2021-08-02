using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    
    public GameObject levelButtons;

    void Start()
    {
        addLevelButtonListeners();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void addLevelButtonListeners()
    {
        int children = levelButtons.transform.childCount;
        for(int i = 0; i < children; i++)
        {
            Button button = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>() ;
            button.onClick.AddListener(() => levelButtonClicked(button.name));
        }
    }

    private void levelButtonClicked(string levelStr)
    {
        int level = Int32.Parse(levelStr[levelStr.Length - 1].ToString());

        GameManager.Instance.level = level;

        switch (level)
        {
            case 1:
                setFieldDimension(5 ,1 ,5);
                GameManager.Instance.bombs = 5;
                break;
            case 2:
                setFieldDimension(10, 1, 10);
                GameManager.Instance.bombs = 20;
                break;
            case 3:
                setFieldDimension(20, 1, 20);
                GameManager.Instance.bombs = 100;
                break;
            case 4:
                setFieldDimension(4, 2, 4);
                GameManager.Instance.bombs = 3;
                break;
            case 5:
                setFieldDimension(7, 2, 7);
                GameManager.Instance.bombs = 10;
                break;
            case 6:
                setFieldDimension(10, 2, 10);
                GameManager.Instance.bombs = 40;
                break;
            case 7:
                setFieldDimension(3, 3, 3);
                GameManager.Instance.bombs = 4;
                break;
            case 8:
                setFieldDimension(5, 3, 5);
                GameManager.Instance.bombs = 10;
                break;
            default:
                setFieldDimension(10, 3, 10);
                GameManager.Instance.bombs = 30;
                break;
        }
            
        SceneManager.LoadScene("Game");
    }

    private void setFieldDimension(int width, int height, int depth )
    {
        GameManager.Instance.width = width;
        GameManager.Instance.height = height;
        GameManager.Instance.depth = depth;
    }

}
