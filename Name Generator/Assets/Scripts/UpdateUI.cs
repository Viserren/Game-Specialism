using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject textFieldPrefab;
    private List<GameObject> contentList = new List<GameObject>();
    //public void AddNameToUIList(string newName)
    //{

    //    GameObject tempName = new GameObject($"text {newName}");
    //    tempName.transform.parent = content.transform;
    //    TextMeshProUGUI text = tempName.AddComponent<TextMeshProUGUI>();
    //    text.text = newName;
    //    contentList.Add(tempName);
    //}

    public void AddNameListToUI(List<string> nameList)
    {
        if (nameList.Count > 0)
        {
            foreach (string name in nameList)
            {
                GameObject temp = Instantiate(textFieldPrefab, content.transform);
                temp.name = $"Text Field: {name}";
                if (temp.TryGetComponent<TMP_InputField>(out TMP_InputField textField))
                {
                    textField.readOnly = true;
                    textField.text = name;
                }
                contentList.Add(temp);
            }
        }
        else
        {
            foreach (GameObject name in contentList)
            {
                Destroy(name);
            }
        }

    }
}
