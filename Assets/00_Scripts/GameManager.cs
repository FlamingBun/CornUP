using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private CharacterManager characterManager;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("GameManager").AddComponent<GameManager>();
                
                if (instance is GameManager gameManager)
                {
                    gameManager.Initialize();
                }
            }
            return instance;
        }
    }

    public CharacterManager CharacterManager { get { return characterManager; } }


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
        
        characterManager = GetComponent<CharacterManager>();
    }
}
