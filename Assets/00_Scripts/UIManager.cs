using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Stack<UIKey> uiStack;
    private Dictionary<UIKey, BaseUI> uiDictionary;

    private void Awake()
    {
        uiStack = new Stack<UIKey>();
        uiDictionary = new Dictionary<UIKey, BaseUI>();
    }

    public void SetUI(UIKey uiKey, BaseUI ui)
    {
        uiDictionary.Add(uiKey, ui);
    }

    public void OpenUI(UIKey uiKey)
    {
        uiStack.Push(uiKey);
        uiDictionary[uiKey].SetUIActive(true);
    }

    
    // 현재 UI 비활성화
    public void CloseUI()
    {
        if (uiStack.Count == 0) return;
        
        uiDictionary[uiStack.Peek()].SetUIActive(false);
        uiStack.Pop();
    }
    
    // 현재 UI 비활성화 후 다음 UI 활성화
    public void ChangeUI(UIKey uiKey)
    {
        CloseUI();
        OpenUI(uiKey);
    }
}