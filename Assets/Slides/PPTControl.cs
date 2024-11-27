using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PPTControl : MonoBehaviour
{ 

    public GameObject[] background;
    public int pageNumber;

    // Start is called before the first frame update
    void Start()
    {
        pageNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (pageNumber >= background.Length - 1)
            pageNumber = background.Length - 1;

        if (pageNumber < 0)
            pageNumber = 0;


        if (pageNumber == 0)
        {
            background[0].gameObject.SetActive(true);
        }

    }

    public void Next()
    {
        pageNumber += 1;

        for (int i = 0; i < background.Length; i++)
        {
            background[i].gameObject.SetActive(false);
            background[pageNumber].gameObject.SetActive(true);
        }
        //Debug.Log(pageNumber);
    }

    public void Previous()
    {
        pageNumber -= 1;

        for (int i = 0; i < background.Length; i++)
        {
            background[i].gameObject.SetActive(false);
            background[pageNumber].gameObject.SetActive(true);
        }
        //Debug.Log(pageNumber);
    }


}