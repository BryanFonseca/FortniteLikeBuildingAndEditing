using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorTerceraPersona : MonoBehaviour 
{

    private Animator anim;
    private float vertical;
    private float horizontal;
    private float horizontalCaminar;
    public float sensibilidad = 5f;
    private Vector3 velocidad;
    public bool enSuelo = true;
    public Transform comprobadorSuelo;
    public float radioComprobadorSuelo = .4f;
    public LayerMask MascaraSuelo;
    public LayerMask IgnoreRaycast;

    private bool Choca = false;

    private CharacterController cController;
    public float capaAnimador;

    public ControladorCamara controladorCam;
	public Transform PuntoMira;

    private bool building;

    public GameObject planos;
    public GameObject lapiz;

    public GameObject puntoRotacion;
    public Transform spine;

    

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        cController = gameObject.GetComponent<CharacterController>();
        controladorCam.GetComponent<ControladorCamara>();
    }

    private void Update()
    {
        if (gameObject.GetComponent<SistemaConstruccion_01>().puedeConstruir || GameObject.Find("Main Camera").GetComponent<EditorScript>().editando || gameObject.GetComponent<Apuntador>().ap)
            Construir(true);
        else
            Construir(false);
        RotacionX();
        Ejes();
        ComprobacionSuelo();
        SetParametrosAnimator();
        Apuntar();
        anim.SetLayerWeight(1, 1);
    }

    void Ejes()
    {
        bool logico = gameObject.GetComponent<SistemaConstruccion_01>().puedeConstruir || GameObject.Find("Main Camera").GetComponent<EditorScript>().editando;//|| gameObject.GetComponent<Apuntador>().ap;
        anim.SetBool("Construyendo", logico);

        //capaAnimador = Mathf.Lerp(capaAnimador, 1, Time.deltaTime * 5);
        //horizontalCaminar = Input.GetAxis("Horizontal");
        //horizontalCaminar = Mathf.Lerp(horizontalCaminar, Mathf.Clamp(Input.GetAxis("Horizontal"), -.5f, .5f), Time.deltaTime * 15);
        horizontalCaminar = Mathf.MoveTowards(horizontalCaminar, Mathf.Clamp(Input.GetAxis("Horizontal"), -.5f, .5f), Time.deltaTime*2);
        horizontal = Mathf.Lerp(horizontal, Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1), Time.deltaTime * 5f);

        float velocidadMagnitud = cController.velocity.magnitude;
        RaycastHit wallHit;

        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward, out wallHit, 1f))
        {
            if (wallHit.collider.tag == "Pared" && wallHit.distance <= 1f)
            {
                if (velocidadMagnitud <= 1)
                {
                    Choca = true;
                }
                else
                {
                    Choca = false;
                }
            }
        }
        else
        {
            Choca = false;
        }
    
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (!Choca)
                {
                    vertical = Mathf.MoveTowards(vertical, 1, Time.deltaTime * 3);
                }
                else
                {
                    vertical = Mathf.Lerp(vertical, cController.velocity.magnitude / 8, Time.deltaTime * 4.5f);
                }
                
            }
            else
            {
                if (!Choca)
                {
                    vertical = Mathf.MoveTowards(vertical, .5f, Time.deltaTime* 3);
                }
                else
                {
                    vertical = Mathf.Lerp(vertical, cController.velocity.magnitude/2, Time.deltaTime * 3);
                }
                
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.S))
            {
                vertical = Mathf.MoveTowards(vertical, -1, Time.deltaTime * 3);
            }
            else
            {
                vertical = Mathf.MoveTowards(vertical, 0, Time.deltaTime * 2);
            }
        }
        
    }

    void RotacionX()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * sensibilidad * Input.GetAxis("Mouse X") * 10);
    }

    void CaidaGravitatoria()
    {
        cController.Move(velocidad*Time.deltaTime);
    }

    void ComprobacionSuelo()
    {        
        bool otroSuelo = Physics.CheckSphere(comprobadorSuelo.position, radioComprobadorSuelo, IgnoreRaycast);
        enSuelo = Physics.CheckSphere(comprobadorSuelo.position, radioComprobadorSuelo, MascaraSuelo) || otroSuelo;
        if (!enSuelo)
        {            
            velocidad += new Vector3(0, -9.8f * Time.deltaTime, 0);
            //CaidaGravitatoria();
        }
        else
        {;
            velocidad = Vector3.zero;
        }
    }

    void SetParametrosAnimator()
    {
        anim.SetFloat("Vertical", vertical);
        anim.SetFloat("MouseX", horizontal);
        anim.SetFloat("Horizontal", horizontalCaminar);
        //anim.SetBool("Cayendo",!enSuelo);
    }

    void Apuntar()
    {       
        /*
        if (Input.GetKey(KeyCode.Mouse1))
        {
            capaAnimador = Mathf.Lerp(capaAnimador, 1, Time.deltaTime * 3);
            anim.speed = .75f;
        }
        else
        {
            capaAnimador = Mathf.Lerp(capaAnimador, 0, Time.deltaTime * 3);
            anim.speed = 1f;
        }
        anim.SetLayerWeight(1, capaAnimador);
        */
    }

    public void Construir(bool valor)
    {
        /*
        if (valor)
        {
            capaAnimador = Mathf.Lerp(capaAnimador, 1, Time.deltaTime * 5);
        }
        else
        {
            capaAnimador = Mathf.Lerp(capaAnimador, 0, Time.deltaTime * 5);
        }
        */
        
    }

    void OnAnimatorIK()
    {
        anim.SetLookAtWeight(1);
        anim.SetLookAtPosition(PuntoMira.position);

        
        //Vector3 temp = puntoRotacion.transform.localRotation.eulerAngles;
        //temp.Scale(new Vector3(-1, 1, -1));

        //Quaternion rot = spine.transform.rotation;
        /*
        if (apuntando)
        {
            rot = Quaternion.Lerp(rot, Quaternion.Euler(temp), Time.deltaTime * 4);
        }
        else
        {
            print("aquí");
            rot = Quaternion.Lerp(rot, rotReturn, Time.deltaTime * 4);
        }
        */
        //Quaternion qTemp = Quaternion.Euler(temp);

        //anim.SetBoneLocalRotation(HumanBodyBones.Spine, rot);        
        //anim.SetBoneLocalRotation(HumanBodyBones.Spine, Quaternion.Euler(temp));        
    }
}
