using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerificaEntorno : MonoBehaviour
{
    public Vector3 posicionRayo = new Vector3(0, 0.8f, 0);
    public float rayoObstaculo = 0.8f;
    public float largoRayo = 2f;
    public float alturaRayo = 6f;
    public LayerMask capaObstaculo;

    [Header ("Escalar")]
    [SerializeField] float rayoEscala = 1.6f;
    [SerializeField] LayerMask capaEscala;
    [SerializeField] int numRayos = 12;

    public infoObstaculo verifObstaculo()
    {
        var datosToque = new infoObstaculo();
        var origenRayo = transform.position + posicionRayo;

        // Raycast hacia el frente relativo al jugador para detectar el obstáculo
        datosToque.toca = Physics.Raycast(origenRayo, transform.forward, out datosToque.infoToque, rayoObstaculo, capaObstaculo);
        Debug.DrawRay(origenRayo, transform.forward * rayoObstaculo, (datosToque.toca) ? Color.green : Color.red);

        if (datosToque.toca)
        {
            // Raycast hacia abajo para detectar la altura del obstáculo
            var origenAltura = datosToque.infoToque.point + Vector3.up * alturaRayo;
            datosToque.alturaToca = Physics.Raycast(origenAltura, Vector3.down, out datosToque.infoAltura, alturaRayo, capaObstaculo);
            Debug.DrawRay(origenAltura, Vector3.down * alturaRayo, (datosToque.alturaToca) ? Color.blue : Color.red);
            
            // Raycast hacia la dirección opuesta del jugador para medir el "largo" del obstáculo
            Vector3 direccionOpuesta = -transform.forward;
            var origenLargo = datosToque.infoToque.point + (transform.forward * largoRayo);  // Ajuste aquí usando transform.forward
            datosToque.largoToca = Physics.Raycast(origenLargo, direccionOpuesta, out datosToque.infoLargo, largoRayo, capaObstaculo);
            Debug.DrawRay(origenLargo, direccionOpuesta * largoRayo, (datosToque.largoToca) ? Color.blue : Color.red);
        }

        return datosToque;
    }

    public bool verifExcalar(Vector3 dirEscala, out RaycastHit infoEscala)
    {
        infoEscala = new RaycastHit();

        if(dirEscala == Vector3.zero)
            return false;
        
        var origenEscala = transform.position + Vector3.up * 1.5f;
        var posicionEscala = new Vector3(0,0.19f,0);

        for(int i = 0; i < numRayos; i++)
        {
            Debug.DrawRay(origenEscala + posicionEscala * i, dirEscala, Color.blue);
            if(Physics.Raycast(origenEscala + posicionEscala * i, dirEscala, out RaycastHit toqueEscala, rayoEscala, capaEscala))
            {
                infoEscala = toqueEscala;
                return true;
            }
        }

        return false;
    }
}



public struct infoObstaculo
{
    public bool toca;
    public bool alturaToca;
    public bool largoToca;
    public RaycastHit infoToque;
    public RaycastHit infoAltura;
    public RaycastHit infoLargo;
}
