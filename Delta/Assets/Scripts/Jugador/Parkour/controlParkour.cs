using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlParkour: MonoBehaviour
{
    [SerializeField] public VerificaEntorno verifEnt;
    [SerializeField] public Animator animador;
    [SerializeField] public MovimientoPJ jugador;

    [Header("Acciones de Parkour")]
    public List<NuevaAccionDeParkour> nuevaAccionDeParkour;

    void Update()
    {
        animador.SetBool("enAccion", jugador.jugadorEnAccion);
        if(!jugador.tocaPiso && !jugador.jugadorEnAccion && !jugador.jugadorColgando)
        {
            var datosToque = verifEnt.verifObstaculo();

            if(datosToque.toca)
            {
                foreach (var accion in nuevaAccionDeParkour)
                {
                    if(accion.verifSiPosible(datosToque, transform))
                    {
                        StartCoroutine(hagaAccionParkour(accion));
                        break;
                    }
                }
            }
        }
    }

    IEnumerator hagaAccionParkour(NuevaAccionDeParkour accion)
    {
        jugador.ponerControl(false);

        ParametroComparaObjetivo parametroComparaObjetivo = null;
        if(accion.PermitirTargetMatching)
        {
            parametroComparaObjetivo = new ParametroComparaObjetivo()
            {
                posicion = accion.ComparaPosicion,
                parteCuerpo = accion.ComparaParteDelCuerpo,
                pesoPosicion = accion.PesoComparaPosicion,
                tiempoInicio = accion.ComparaTiempoInicio,
                tiempoFin = accion.ComparaTiempoFinal
            };
        }

        yield return jugador.hagaAccion(accion.NombreAnima, parametroComparaObjetivo, accion.rotacionGuardada, accion.MirarAObstaculo, accion.DelayAccionParkour);

        jugador.ponerControl(true);
    }

    public void comparaObjetivo(NuevaAccionDeParkour accion)
    {
        animador.MatchTarget(accion.ComparaPosicion, transform.rotation, accion.ComparaParteDelCuerpo, new MatchTargetWeightMask(accion.PesoComparaPosicion,0), accion.ComparaTiempoInicio, accion.ComparaTiempoFinal);
    }
}
