using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlEscalado : MonoBehaviour
{
    [SerializeField] public VerificaEntorno ve;
    [SerializeField] public MovimientoPJ jugador;

    PuntoEscalado puntoActual;

    float valorAdeAtr;
    float valorArrAba;
    float valorIzqDer;

    private void Awake() {
        ve = GetComponent<VerificaEntorno>();
        jugador = GetComponent<MovimientoPJ>();
    }

    void Update()
    {
        if(!jugador.jugadorColgando){
            if(!jugador.tocaPiso && Input.GetButton("Jump") && !jugador.jugadorEnAccion)
            {
                if(ve.verifExcalar(transform.forward, out RaycastHit infoEscala))
                {
                    puntoActual = infoEscala.transform.GetComponent<PuntoEscalado>();

                    jugador.ponerControl(false);
                    valorAdeAtr = 0.138439f;
                    valorArrAba = 0.0229829f;
                    valorIzqDer = 0.274302f;
                    StartCoroutine(escalarBorde("Idle To Braced Hang", infoEscala.transform, 0.05f, 0.20f, puntoManos: new Vector3(valorAdeAtr,valorArrAba,valorIzqDer)));
                }
            }
        }
        else 
        {
            if(Input.GetButton("Salir"))
            {
                StartCoroutine(saltarDePared());
                return;
            }
            float horizontal = Mathf.Round(Input.GetAxisRaw("Horizontal"));
            float vertical = Mathf.Round(Input.GetAxisRaw("Vertical"));

            var dirEntrada = new Vector2 (horizontal, vertical);

            if(jugador.jugadorEnAccion || dirEntrada == Vector2.zero) return;

            if(puntoActual.puntoSubir && dirEntrada.y == 1)
            {
                StartCoroutine(subir());
                return;
            }

            var vecino = puntoActual.verifVecino(dirEntrada);

            if(vecino == null) return;

            if(vecino.tipoConexion == TipoConexion.salta && Input.GetButton("Jump"))
            {
                puntoActual = vecino.puntoEscalado;

                if(vecino.direccion.y ==1 )
                {
                    valorAdeAtr = 0.132237f;
                    valorArrAba = -0.246836f;
                    valorIzqDer = 0.265829f;
                    StartCoroutine(escalarBorde("Escala Arriba", puntoActual.transform, 0.34f, 0.64f,puntoManos: new Vector3(valorAdeAtr,valorArrAba,valorIzqDer)));
                }
                else if(vecino.direccion.y == -1)
                {
                    valorAdeAtr = 0.132237f;
                    valorArrAba = -0.246836f;
                    valorIzqDer = 0.265829f;
                    StartCoroutine(escalarBorde("Escala Abajo", puntoActual.transform, 0.31f, 0.68f,puntoManos: new Vector3(valorAdeAtr,valorArrAba,valorIzqDer)));
                }
                else if(vecino.direccion.x == 1)
                {
                    valorAdeAtr = 0.072237f;
                    valorArrAba = -0.1993841f;
                    valorIzqDer = 0.22595432f;
                    StartCoroutine(escalarBorde("Escala Derecha", puntoActual.transform, 0.20f, 0.51f,puntoManos: new Vector3(valorAdeAtr,valorArrAba,valorIzqDer)));
                }
                else if(vecino.direccion.x == -1)
                {
                    valorAdeAtr = 0.072237f;
                    valorArrAba = -0.1993841f;
                    valorIzqDer = 0.22595432f;
                    StartCoroutine(escalarBorde("Escala Izquierda", puntoActual.transform, 0.20f, 0.51f,puntoManos: new Vector3(valorAdeAtr,valorArrAba,valorIzqDer)));
                }
            }
            else if(vecino.tipoConexion == TipoConexion.mueve)
            {
                puntoActual = vecino.puntoEscalado;

                if(vecino.direccion.x == 1)
                {
                    valorAdeAtr = 0.144946f;
                    valorArrAba = -0.224916f;
                    valorIzqDer = 0.2581082f;
                    StartCoroutine(escalarBorde("Desplaza Der", puntoActual.transform, 0f, 0.30f,puntoManos: new Vector3(valorAdeAtr,valorArrAba,valorIzqDer)));
                }
                else if(vecino.direccion.x == -1)
                {
                    valorAdeAtr = 0.144946f;
                    valorArrAba = -0.224916f;
                    valorIzqDer = -0.2581082f;
                    StartCoroutine(escalarBorde("Desplaza Izq", puntoActual.transform, 0f, 0.30f,puntoManos: new Vector3(valorAdeAtr,valorArrAba,valorIzqDer)));
                }
            }
            
        }
    }

    IEnumerator escalarBorde (string nombreAnima, Transform puntoBorde, float comparaTiempoInicio, float comparaTiempoFinal, AvatarTarget mano = AvatarTarget.RightHand, Vector3? puntoManos = null)
    {
        var parametrosCompara = new ParametroComparaObjetivo()
        {
            posicion = posicionManos(puntoBorde, mano, puntoManos),
            parteCuerpo = mano,
            pesoPosicion = Vector3.one,
            tiempoInicio = comparaTiempoInicio,
            tiempoFin = comparaTiempoFinal
        };

        var rotacionGuardada = Quaternion.LookRotation(-puntoBorde.forward);

        yield return jugador.hagaAccion(nombreAnima, parametrosCompara, rotacionGuardada, true);

        jugador.jugadorColgando = true;
    }

    Vector3 posicionManos(Transform borde, AvatarTarget mano, Vector3? puntoManos)
    {
        var valorPuntoManos = (puntoManos!= null)? puntoManos.Value : new Vector3(valorAdeAtr, valorArrAba, valorIzqDer);

        var direccionMano = (mano == AvatarTarget.RightHand)? borde.right : -borde.right;
        return borde.position + borde.forward * valorAdeAtr + Vector3.up * valorArrAba - direccionMano * valorIzqDer;
    }

    IEnumerator saltarDePared()
    {
        jugador.jugadorColgando = false;
        yield return jugador.hagaAccion("Jump From Wall");
        jugador.resetearRotacion();
        jugador.ponerControl(true);
    }

    IEnumerator subir()
    {
        jugador.jugadorColgando = false;
        yield return jugador.hagaAccion("Braced Hang To Crouch");
        yield return new WaitForSeconds(0.5f);
        jugador.activaControlador();
        jugador.resetearRotacion();
        jugador.ponerControl(true);
    }
}