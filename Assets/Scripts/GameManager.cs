using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int width, height, depth;
    public int level;
    public int bombs;
    public bool gameOver = false;
    public bool win = false;
    public int difficult = 0;
    public bool playSound = true;
    public bool cues = true;

    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("GM").AddComponent<GameManager>();
            }
            return instance;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Awake()
    {
        if(instance != null)
        {
            DestroyImmediate(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

}
