using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class colisionCam : MonoBehaviour
{
    [SerializeField] private float distanciaMin = 1;
    [SerializeField] private float distanciaMax = 5;
    [SerializeField] private float suavidad = 10;
    [SerializeField] private float distancia;

    Vector3 direccion;

    private void Start()
    {
        direccion = transform.localPosition.normalized;
        distancia = transform.localPosition.magnitude;
    }

    private void Update()
    {
        Vector3 posCam = transform.parent.TransformPoint(direccion * distanciaMax);

        RaycastHit hit;
        if(Physics.Linecast(transform.parent.position, posCam, out hit)){
            distancia = Mathf.Clamp(hit.distance * 0.85f, distanciaMin, distanciaMax);
        }
        else{
            distancia = distanciaMax;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, direccion * distancia, suavidad * Time.deltaTime);
    }
}
