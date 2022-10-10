using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlataform : MonoBehaviour
{
    private float contador = 0;

    void Update()
    {
        contador += Time.deltaTime;
        if (contador >= 0 && contador < 5) transform.Rotate(new Vector3(0, 0, 90) * Time.deltaTime);
        else if (contador >= 5) transform.Rotate(new Vector3(0, 0, -90) * Time.deltaTime);
        if (contador >= 10) contador = 0;
    } 
}
