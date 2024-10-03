using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPJ : MonoBehaviour
{
    private CharacterController controlador;
    private GameObject camara;

    [Header("Estadísticas Básicas")]
    [SerializeField] private float velocidad;
    [SerializeField] private float alturaDeSaltoMinima = 2f;  // Altura mínima de salto
    [SerializeField] private float alturaDeSaltoMaxima = 5f;  // Altura máxima de salto
    [SerializeField] private float tiempoMaximoSalto = 1.5f;  // Tiempo máximo para alcanzar la altura máxima
    [SerializeField] private float tiempoDeGiro;

    [Header("Datos Del Piso")]
    [SerializeField] private Transform detectaPiso;
    [SerializeField] private float distanciaPiso;
    [SerializeField] private LayerMask mascaraPiso;

    private float velocidadGiro;
    private float gravedad = -9.81f;
    private Vector3 velocity;
    private bool tocaPiso;
    private bool estaCargandoSalto;
    private float tiempoPresionadoSalto;

    private void Start() {
        controlador = GetComponent<CharacterController>();
        camara = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update() {
        tocaPiso = Physics.CheckSphere(detectaPiso.position, distanciaPiso, mascaraPiso);

        if (tocaPiso && velocity.y < 0) {
            velocity.y = -2f;
        }

        // Comienza a cargar el salto si el botón es presionado y está en el piso
        if (Input.GetButtonDown("Jump") && tocaPiso) {
            tiempoPresionadoSalto = 0f;
            estaCargandoSalto = true;
        }

        // Incrementa el tiempo de presionado mientras el botón sigue presionado
        if (Input.GetButton("Jump") && estaCargandoSalto) {
            tiempoPresionadoSalto += Time.deltaTime;

            // Limitar el tiempo de carga a 1.5 segundos
            tiempoPresionadoSalto = Mathf.Clamp(tiempoPresionadoSalto, 0f, tiempoMaximoSalto);
        }

        // Cuando se suelta el botón, realiza el salto
        if (Input.GetButtonUp("Jump") && estaCargandoSalto) {
            estaCargandoSalto = false;

            // Calcular la altura en función del tiempo que se mantuvo presionado
            float alturaActual = Mathf.Lerp(alturaDeSaltoMinima, alturaDeSaltoMaxima, tiempoPresionadoSalto / tiempoMaximoSalto);

            // Realizar el salto en función de la altura calculada
            velocity.y = Mathf.Sqrt(alturaActual * -2f * gravedad);
        }

        // Aplicar la gravedad
        velocity.y += gravedad * Time.deltaTime;
        controlador.Move(velocity * Time.deltaTime);

        // Movimiento horizontal
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direccion = new Vector3(horizontal, 0f, vertical).normalized;

        if (direccion.magnitude >= 0.1f) {
            float objetivoAngulo = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg + camara.transform.eulerAngles.y;
            float angulo = Mathf.SmoothDampAngle(transform.eulerAngles.y, objetivoAngulo, ref velocidadGiro, tiempoDeGiro);
            transform.rotation = Quaternion.Euler(0, angulo, 0);

            Vector3 mover = Quaternion.Euler(0, objetivoAngulo, 0) * Vector3.forward;
            controlador.Move(mover.normalized * velocidad * Time.deltaTime);
        }
    }
}
