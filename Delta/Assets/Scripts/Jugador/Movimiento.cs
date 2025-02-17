using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPJ : MonoBehaviour
{
    public CharacterController controlador;
    private GameObject camara;

    [Header("Estadísticas Básicas")]
    [SerializeField] public float velocidadMovimiento = 5; // Velocidad de movimiento
    [SerializeField] private float alturaDeSaltoMinima = 2f;
    [SerializeField] private float alturaDeSaltoMaxima = 5f;
    [SerializeField] private float tiempoMaximoSalto = 1.5f;
    [SerializeField] private float tiempoDeGiro = 0.1f;
    [SerializeField] private bool saltando;
    [SerializeField] public bool tocaPiso;
    public bool jugadorColgando {get; set;}
    public bool jugadorEnAccion {get; private set;}

    [Header("Datos Del Piso")]
    [SerializeField] private Transform detectaPiso;
    [SerializeField] private float distanciaPiso;
    [SerializeField] private LayerMask mascaraPiso;

    [Header("Animaciones")]
    [SerializeField] private Animator animador;

    [SerializeField] private controlParkour parkour;
    private bool tieneControlJugador = true; // Control del jugador
    private float velocidadGiro;
    private float velocidadBase;
    private float gravedad = -9.81f;
    public Vector3 velocidad; // Vector de velocidad
    private bool estaCargandoSalto;
    private float tiempoPresionadoSalto;
    private Quaternion rotacionGuardada; // Variable para guardar la rotación cuando se pierde el control


    private void Start() {
        controlador = GetComponent<CharacterController>();
        parkour = GetComponent<controlParkour>();
        camara = GameObject.FindGameObjectWithTag("MainCamera");
        velocidadBase = velocidadMovimiento;
        saltando = false;
    }

    private void Update() {
        if (!tieneControlJugador) 
            return;

        // Solo retorna si el jugador está colgando
        if (jugadorColgando)
            return;

        if (jugadorEnAccion)
        {
            saltando = false;
            tocaPiso = true;
        }
        tocaPiso = Physics.CheckSphere(detectaPiso.position, distanciaPiso, mascaraPiso);

        if (tocaPiso && velocidad.y < 0) {
            velocidad.y = -2f;
        }


        if (Input.GetButtonDown("Jump") && tocaPiso) {
            tiempoPresionadoSalto = 0f;
            estaCargandoSalto = true;
        }

        if (Input.GetButton("Jump") && estaCargandoSalto) {
            tiempoPresionadoSalto += Time.deltaTime;
            tiempoPresionadoSalto = Mathf.Clamp(tiempoPresionadoSalto, 0f, tiempoMaximoSalto);
        }

        if (Input.GetButtonUp("Jump") && estaCargandoSalto) {
            estaCargandoSalto = false;
            float alturaActual = Mathf.Lerp(alturaDeSaltoMinima, alturaDeSaltoMaxima, tiempoPresionadoSalto / tiempoMaximoSalto);
            velocidad.y = Mathf.Sqrt(alturaActual * -2f * gravedad);
            saltando = true;
        }

        velocidad.y += gravedad * Time.deltaTime;

        controlador.Move(velocidad * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftControl)) {
            velocidadMovimiento = velocidadBase * 2;
        } else {
            velocidadMovimiento = velocidadBase;
        }

        if (tieneControlJugador) { // Solo si el jugador tiene control, el movimiento y la rotación ocurren.
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direccion = new Vector3(horizontal, 0f, vertical).normalized;

            if (direccion.magnitude >= 0.1f) {
                float objetivoAngulo = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg + camara.transform.eulerAngles.y;
                float angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, objetivoAngulo, ref velocidadGiro, tiempoDeGiro);
                transform.rotation = Quaternion.Euler(0, angulo, 0);

                Vector3 mover = Quaternion.Euler(0, objetivoAngulo, 0) * Vector3.forward;
                controlador.Move(mover.normalized * velocidadMovimiento * Time.deltaTime);
            }

            ActualizarAnimacion(direccion.magnitude);
        }
    }

    private void ActualizarAnimacion(float magnitudMovimiento) {
        float cantMov;

        if (magnitudMovimiento == 0) {
            cantMov = 0f; // Idle
        } else if (Input.GetKey(KeyCode.LeftControl)) {
            cantMov = 1f; // Corriendo
        } else {
            cantMov = 0.5f; // Caminando
        }

        animador.SetBool("tocaPiso", tocaPiso);
        animador.SetBool("salta",saltando);
        animador.SetBool("colgando", jugadorColgando);
        animador.SetFloat("cantMovimiento", cantMov, 0.2f, Time.deltaTime);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(detectaPiso.position, distanciaPiso);
    }

    public IEnumerator hagaAccion(string NombreAnima, ParametroComparaObjetivo pco = null, Quaternion rotacionGuardada = new Quaternion(), bool MirarAObstaculo = false, float DelayAccionParkour = 0f)
    {
        jugadorEnAccion = true;
        animador.CrossFadeInFixedTime(NombreAnima, 0.2f);
        yield return null;

        // Espera hasta que el animador haya cambiado al estado correcto y no esté en transición
        var estadoAnimacion = animador.GetNextAnimatorStateInfo(0);
        while (animador.IsInTransition(0) || !estadoAnimacion.IsName(NombreAnima))
        {
            yield return null;
            estadoAnimacion = animador.GetCurrentAnimatorStateInfo(0); // Actualiza el estado actual
        }
        var nombreAnima = animador.GetNextAnimatorStateInfo(0);
        if(!nombreAnima.IsName(NombreAnima))
            Debug.Log("nombre de animacion incorrecto");

        float tiempoInicioRota = (pco != null)? pco.tiempoInicio : 0f;
        float cronometro = 0f;
        while (cronometro <= estadoAnimacion.length)
        {
            cronometro += Time.deltaTime;

            float cronometroNormalizado = cronometro/ estadoAnimacion.length;

            if (MirarAObstaculo && cronometroNormalizado > tiempoInicioRota)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionGuardada, 600f * Time.deltaTime);
            }

            if (pco != null)
            {
                comparaObjetivo(pco); // Esto ahora debería funcionar
            }

            // Salir del bucle si está en transición (para evitar continuar después de que cambie el estado)
            if (animador.IsInTransition(0) && cronometro > 0.5f)
            {
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(DelayAccionParkour);
        jugadorEnAccion = false;
    }


    
    public void comparaObjetivo(ParametroComparaObjetivo parametroComparaObjetivo)
    {
        animador.MatchTarget(parametroComparaObjetivo.posicion, transform.rotation, parametroComparaObjetivo.parteCuerpo, new MatchTargetWeightMask(parametroComparaObjetivo.pesoPosicion,0), parametroComparaObjetivo.tiempoInicio, parametroComparaObjetivo.tiempoFin);
    }

    // Nueva función para establecer el control del jugador
    public void ponerControl(bool tieneControl) {
        this.tieneControlJugador = tieneControl;
        controlador.enabled = tieneControl;

        if(!tieneControl) {
            animador.SetFloat("cantMovimiento", 0f);
            // Guardamos la rotación actual cuando se pierde el control
            rotacionGuardada = transform.rotation;
        }
    }

    public void activaControlador()
    {
        controlador.enabled = enabled;
    }

    public void resetearRotacion()
    {
        rotacionGuardada = transform.rotation;
    }

    public void FinDeSalto() {
        saltando = false;
    }

}

public class ParametroComparaObjetivo
{
    public Vector3 posicion;
    public AvatarTarget parteCuerpo;
    public Vector3 pesoPosicion;
    public float tiempoInicio;
    public float tiempoFin;
}
