using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changecolor : MonoBehaviour
{
    public Material carroceria;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeBlack()
    {
        carroceria.color = Color.black;
    }
    public void changeWhite()
    {
        carroceria.color = Color.white;
    }
    public void changeBlue()
    {
        carroceria.color = Color.blue;
    }
}
