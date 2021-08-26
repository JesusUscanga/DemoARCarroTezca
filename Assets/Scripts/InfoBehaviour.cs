using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoBehaviour : MonoBehaviour
{
    const float SPEED = 6f;
    [SerializeField]
    Transform SectionInfo;

    Vector3 desiredScale = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SectionInfo.localScale = Vector3.Lerp(SectionInfo.localScale, desiredScale, Time.deltaTime * SPEED);
    }

    public void OpenInfo()
    {
        print("Abre info");
        desiredScale = Vector3.one;
    }

    public void CloseInfo()
    {
        print("Cierra info");
        desiredScale = Vector3.zero;
    }
}
