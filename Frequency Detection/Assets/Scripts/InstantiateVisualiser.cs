using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateVisualiser : MonoBehaviour
{
    public GameObject instantiatePrefab;
    GameObject[] eqObjects = new GameObject[512];
    public float maxScale;
    public float spacing;
    public float startPosition;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            GameObject tempObject = Instantiate(instantiatePrefab,this.transform,false);
            tempObject.transform.position = this.transform.position;
            tempObject.name = $"EQ {i}";
            //tempObject.transform.position = new Vector3(startPosition + (spacing * i), 0, 0);
            eqObjects[i] = tempObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //for (int i = 0; i < 512; i++)
        //{
        //    if (eqObjects != null)
        //    {
        //        var temp = eqObjects[i].GetComponent<RectTransform>();
        //        temp.sizeDelta = new Vector2(.08f, (AudioAnalyser.samples[i] * maxScale + 2));
        //    }
        //}
    }
}
