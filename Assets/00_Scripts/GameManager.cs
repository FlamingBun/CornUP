using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    public GameObject player; // TODO: resource로 옮기기
    
    private CharacterManager characterManager;
    private UIManager uiManager;
    private Inventory inventory;
    
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
                instance.AddComponent<UIManager>();
                instance.AddComponent<CharacterManager>();
                instance.AddComponent<Inventory>();
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
    public Inventory Inventory { get { return inventory; } }
    
    

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
        inventory = GetComponent<Inventory>();
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
