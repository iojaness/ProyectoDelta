using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuntoEscalado : MonoBehaviour
{
    public bool puntoSubir;
    public List<Vecino> vecinos;

    void Awake()
    {
        var dobleViaVecinos = vecinos.Where(n => n.dobleVia);
        foreach(var vecino in dobleViaVecinos)
        {
            vecino.puntoEscalado?.crearConexion(this, -vecino.direccion, vecino.tipoConexion, vecino.dobleVia);
        }
    }

    public void crearConexion(PuntoEscalado puntoEscalado, Vector2 direccion, TipoConexion tipoConexion, bool dobleVia)
    {
        var vecino = new Vecino()
        {
            puntoEscalado = puntoEscalado,
            direccion = direccion,
            tipoConexion = tipoConexion,
            dobleVia = dobleVia
        };

        vecinos.Add(vecino);
    }

    public Vecino verifVecino(Vector2 dirEscala)
    {
        Vecino vecino = null;

        if(dirEscala.y != 0)
        {
            vecino = vecinos.FirstOrDefault(n => n.direccion.y == dirEscala.y);
        }

        if(vecino == null && dirEscala.x != 0)
        {
            vecino = vecinos.FirstOrDefault(n => n.direccion.x == dirEscala.x);
        }

        return vecino;
    }

    private void OnDrawGizmos()
    {  
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        foreach(var vecino in vecinos)
        {
            if(vecino.puntoEscalado != null)
            {
                Debug.DrawLine(transform.position, vecino.puntoEscalado.transform.position, (vecino.dobleVia)? Color.cyan :Color.magenta);
            }
            else return;
        }
    }
}

[System.Serializable]
public class Vecino
{
    public PuntoEscalado puntoEscalado;
    public Vector2 direccion;
    public TipoConexion tipoConexion;
    public bool dobleVia;
}

public enum TipoConexion {salta, mueve}