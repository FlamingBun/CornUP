using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private CharacterManager characterManager;
    private UIManager uiManager;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
                instance.AddComponent<UIManager>();
                instance.AddComponent<CharacterManager>();
                if (instance is GameManager gameManager)
                {
                    gameManager.Initialize();
                }
            }
            return instance;
        }
    }

    public CharacterManager CharacterManager { get { return characterManager; } }
    public UIManager UIManager { get { return uiManager; } }
    public GameObject player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        uiManager = GetComponent<UIManager>();
        characterManager = GetComponent<CharacterManager>();
        characterManager.Player = Instantiate(player).GetComponent<Player>();
    }

    private void Start()
    {
        GameStart();
    }

    private void GameStart()
    {
        UIManager.OpenUI(UIKey.ConditionUI);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
