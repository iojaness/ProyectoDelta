using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Menu Parkour/Crear Nueva Accion de Parkour")]
public class NuevaAccionDeParkour : ScriptableObject
{
    [Header("Altura Obstaculo")]
    [SerializeField] string nombreAnima;
    [SerializeField] float alturaMinima;
    [SerializeField] float alturaMaxima;

    [Header("Largo Obstaculo")]
    [SerializeField] float largoMinimo;
    [SerializeField] float largoMaximo;

    [Header("Rotar Jugador Hacia Obstaculo")]
    [SerializeField] bool mirarAObstaculo;
    [SerializeField] float delayAccionParkour;
    public Quaternion rotacionGuardada { get; set; }

    [Header("TargetMatching")]
    [SerializeField] bool permitirTargetMatching = true;
    [SerializeField] AvatarTarget comparaParteDelCuerpo;
    [SerializeField] float comparaTiempoInicio;
    [SerializeField] float comparaTiempoFinal;
    [SerializeField] Vector3 pesoComparaPosicion = new Vector3(0, 1, 0);

    public Vector3 ComparaPosicion { get; set; }

    public bool verifSiPosible(infoObstaculo datosToque, Transform jugador)
    {
        // Verifica la altura
        float verifAltura = datosToque.infoAltura.point.y - jugador.position.y;
        if (verifAltura < alturaMinima || verifAltura > alturaMaxima)
        {
            return false;
        }

        // Calcular la distancia entre los puntos de impacto para el largo del obstáculo
        float verifLargo = Vector3.Distance(datosToque.infoToque.point, datosToque.infoLargo.point);
        Debug.Log("Largo del obstáculo (distancia absoluta): " + verifLargo);

        if (verifLargo < largoMinimo || verifLargo > largoMaximo)
        {
            return false;
        }

        // Opcional: rotar al jugador hacia el obstáculo
        if (mirarAObstaculo)
        {
            rotacionGuardada = Quaternion.LookRotation(-datosToque.infoToque.normal);
        }

        if (permitirTargetMatching)
        {
            ComparaPosicion = datosToque.infoAltura.point;
        }

        return true;
    }

    public string NombreAnima => nombreAnima;
    public bool MirarAObstaculo => mirarAObstaculo;
    public float DelayAccionParkour => delayAccionParkour;
    public bool PermitirTargetMatching => permitirTargetMatching;
    public AvatarTarget ComparaParteDelCuerpo => comparaParteDelCuerpo;
    public float ComparaTiempoInicio => comparaTiempoInicio;
    public float ComparaTiempoFinal => comparaTiempoFinal;
    public Vector3 PesoComparaPosicion => pesoComparaPosicion;
}
