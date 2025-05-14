using System.Collections.Generic;
using UnityEngine;

public class DialogVariables : MonoBehaviour
{
    private Dictionary<string, string> variables = new Dictionary<string, string>();
    private static DialogVariables instance;

    [System.Obsolete]
    public static DialogVariables Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DialogVariables>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("DialogVariables");
                    instance = obj.AddComponent<DialogVariables>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetVariable(string key, string value)
    {
        if (variables.ContainsKey(key))
        {
            variables[key] = value;
        }
        else
        {
            variables.Add(key, value);
        }
    }

    public string GetVariable(string key)
    {
        if (variables.ContainsKey(key))
        {
            return variables[key];
        }
        return $"{{{key}}}"; // Return the original format if variable not found
    }

    public string ReplaceVariablesInText(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        foreach (var variable in variables)
        {
            text = text.Replace($"{{{variable.Key}}}", variable.Value);
        }
        return text;
    }

    public void ClearVariables()
    {
        variables.Clear();
    }
}