using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PromptText : MonoBehaviour
{
        private TextMeshProUGUI promptText;

        private void Awake()
        {
                promptText = GetComponent<TextMeshProUGUI>();
                GameManager.Instance.UIManager.PromptText = this;
                this.gameObject.SetActive(false);
        }
        
        public void SetPromptText(string _text)
        {
                this.gameObject.SetActive(true);
                promptText.text = _text;
        }
}
