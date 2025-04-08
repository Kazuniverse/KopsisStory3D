using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem.Controls;

public class Search : MonoBehaviour
{
    public InputField inputField;
    [SerializeField] private List<GameObject> buttonList = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputField.onValueChanged.AddListener(delegate { SearchEngine(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SearchEngine()
    {
        if (inputField.text.Length == 0)
        {
            foreach (GameObject button in buttonList)
            {
                button.SetActive(true);
            }
        }

        if (inputField.text.Length > 0)
        {
            string searchText = inputField.text.ToLower();

            foreach (GameObject button in buttonList)
            {
                button.SetActive(false);

                Text buttonText = button.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    string text = buttonText.text.ToLower();
                    bool textMatch = text.Contains(searchText);

                    button.SetActive(textMatch);
                }
            }
        }

    }
}
