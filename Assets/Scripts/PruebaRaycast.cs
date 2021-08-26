using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PruebaRaycast : MonoBehaviour
{
    private Vector2 touchPosition = default;
    public Material color;
    List<InfoBehaviour> infos = new List<InfoBehaviour>();
    public bool abrioInfo = false;
    // Start is called before the first frame update
    void Start()
    {
        infos = FindObjectsOfType<InfoBehaviour>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            //print("entro al if");
            Touch touch = Input.GetTouch(0);

            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if (Physics.Raycast(ray, out hitObject))
                {
                    if(hitObject.collider != null)
                    {
                        //ChangeSelectedObject(hitObject);
                        AccionObjetos(hitObject);
                    }
                    
                }
            }
        }
    }

    void ChangeSelectedObject(RaycastHit hit)
    {
        GameObject go = hit.collider.gameObject;
        //---------------Cambia el tag------------------------
        if (go.CompareTag("car"))
        {
            color.color = Color.black;
        }
    }

    public void AccionObjetos(RaycastHit hit)
    {
        infos = FindObjectsOfType<InfoBehaviour>().ToList();
        GameObject go = hit.collider.gameObject;
        //---------------Cambia el tag------------------------
        if (go.CompareTag("car") && abrioInfo == false)
        {
            abrioInfo = true;
            OpenInfo(go.GetComponent<InfoBehaviour>());
        }
        else
        {
            abrioInfo = false;
            CloseAll();
        }
    }

    //---------------------Funciones extra dependiendo el prefab-----------------
    void OpenInfo(InfoBehaviour desiredInfo)
    {
        foreach (InfoBehaviour info in infos)
        {
            if (info == desiredInfo)
            {
                print("Entro a OpenInfo DetecCube");
                info.OpenInfo();

            }
            else
            {
                info.CloseInfo();

            }
        }
    }

    void CloseAll()
    {
        foreach (InfoBehaviour info in infos)
        {
            info.CloseInfo();
        }
    }
}
