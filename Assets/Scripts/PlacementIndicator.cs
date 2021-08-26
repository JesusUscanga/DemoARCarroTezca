using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlacementIndicator : MonoBehaviour
{

    //Variables para el Plano
    private ARRaycastManager rayManager;//Referencia al scrip de Raycast 
    private GameObject visual;//Referencia al objeto con la imagen indicador
    public Camera arCamera; //Camara para el raycast
    private ARPlaneManager arPlane; //Variable del trackeo del plano
    //Objetos
    public bool banderaObjetoActivo = false; //Bandera para saber si el objeto esta activo en el plano
    public bool indicadorExiste = false;//Bandera para saber si el indicador esta activo en la escena
    //Objeto donde se realizara el spawn
    public GameObject objSpawn; //Objeto de Spawn
    private GameObject controlObjSpawn;
    Vector3 v3 = new Vector3(100, 100, 100); //Vector para ponerlo fuera de la vista de la escena


    //---------------Variables Extras dependiendo el prefab--------------
    List<InfoBehaviour> infos = new List<InfoBehaviour>();
    public bool abrioInfo = false;

    public Material color; //Material que cambiara de color
    private Vector2 touchPosition = default;

    private bool statusPlane = false;//Variable para ver si esta activo o inactivo los planos
    public GameObject placeHolderIndicacion;


    void Start()
    {
        //Busca los componentes
        rayManager = FindObjectOfType<ARRaycastManager>();
        visual = transform.GetChild(0).gameObject;
        arPlane = FindObjectOfType<ARPlaneManager>();

        //Esconde el indicador
        visual.SetActive(false);
        
        //Busca el objeto con el script 
        infos = FindObjectsOfType<InfoBehaviour>().ToList();

        //Disable Planes
        statusPlane = false;
    }

    // Update is called once per frame
    void Update()
    {
        // shoot a raycast from the center of the screen
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        // if we hit AR Plane, Update the postion and rotation

        //-----------------Esto lo que hace es mover el indicador de lugar
        if (hits.Count > 0 && indicadorExiste == false)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;

            if (!visual.activeInHierarchy)
                visual.SetActive(true);
               

        }
        else if (indicadorExiste == true)
        {
            if (!visual.activeInHierarchy)
                visual.SetActive(false);
                
        }
        //---------------Si no existe algun objeto dentro del plano puede crearlo en la posicion del indicador
        if (banderaObjetoActivo == false)
        {
            placeHolderIndicacion.SetActive(true);
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                controlObjSpawn = Instantiate(objSpawn, this.transform.position, this.transform.rotation);
                //objSpawn.transform.position = transform.position;
                //objSpawn.transform.rotation = transform.rotation;
                banderaObjetoActivo = true;
                indicadorExiste = true;
            }
        }
        //--------------Si existe objeto dentro del plano verifica su raycast
        if (banderaObjetoActivo == true)
        {
            placeHolderIndicacion.SetActive(false);
            visual.SetActive(false);
            if ((Input.touchCount > 0) && (Input.touches[0].phase == TouchPhase.Began))
            {
                print("entro al if");
                Touch touch = Input.GetTouch(0);

                touchPosition = touch.position;

                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hitObject;
                    if (Physics.Raycast(ray, out hitObject))
                    {
                        if (hitObject.collider != null)
                        {
                            //ChangeSelectedObject(hitObject);
                            AccionObjetos(hitObject);
                        }
                    }
                }
            }
        }
        //--------------Enables and Disable Plane Detection
        if (statusPlane == false)
        {
            foreach (var plane in arPlane.trackables)
                plane.gameObject.SetActive(false);
        }
        if (statusPlane == true)
        {
            foreach (var plane in arPlane.trackables)
                plane.gameObject.SetActive(true);
        }
    }

    //Funcion para eliminar el objeto de la escena
    public void RemplazarObjeto()
    {
        if(controlObjSpawn != null)
        {
            Destroy(controlObjSpawn);
        }
        //objSpawn.transform.position = v3;
        banderaObjetoActivo = false;
        indicadorExiste = false;
    }

    //Funcion para las acciones de los objetos
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
    //---------------------------------------------------------------------
    void ChangeSelectedObject(RaycastHit hit)
    {
        GameObject go = hit.collider.gameObject;
        //---------------Cambia el tag------------------------
        if (go.CompareTag("car"))
        {
            color.color = Color.black;
        }
            
    }
}

