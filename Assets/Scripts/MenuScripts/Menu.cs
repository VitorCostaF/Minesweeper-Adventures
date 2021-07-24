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
        SceneManager.LoadScene("Game");
    }


}
