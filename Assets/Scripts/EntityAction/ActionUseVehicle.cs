using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ActionUseVehicleProperties : ActionInteractProperties
{
    public Vehicle m_Vihecle;
    public VehicleInputControl m_VeicleInput;
    public GameObject m_Hint;
}


public class ActionUseVehicle :ActionInteract
{




    [SerializeField]
    [Header("Action Component")]
    private CharacterController m_CharacterController;

    [SerializeField]
    private GameObject m_Headlights;

    [SerializeField]
    private AudioSource m_EngineSound;

    [SerializeField]
    private Turret m_Turret;

    [SerializeField]
    private ShootingVehicleInputControl m_ShootingVehicleInputControl;

    [SerializeField]
    private BoxCollider m_BoxCollider;

    [SerializeField]
    private CharacterMovement m_CharacterMovement;

    [SerializeField]
    private GameObject m_VisualModel;

    [SerializeField]
    private ThirdPersonCamera m_Camera;

    [SerializeField]
    private CheracterInputController m_CheracterInputController;


    private bool InVehicle;
    private void Start()
    {
        EventOnStart.AddListener(OnActionStarted);

        EventOnEnd.AddListener(OnActionEnded);
     
    }

    private void Update()
    {
        if (InVehicle == true)
        {
            IsCanEnd = (Properties as ActionUseVehicleProperties).m_Vihecle.LinearVelocity < 2;
            (Properties as ActionUseVehicleProperties).m_Hint.SetActive(IsCanEnd);
        }
    }

    private void OnDestroy()
    {
        EventOnStart.RemoveListener(OnActionStarted);

        EventOnEnd.RemoveListener(OnActionEnded);
    }

    private void OnActionStarted()
    {
        ActionUseVehicleProperties prop = Properties as ActionUseVehicleProperties;
      

        InVehicle = true;
        // Camera
        //m_Camera.SetTargetOffset(new Vector3(0.4f, 2f, -3f));
        prop.m_VeicleInput.AssignCamera(m_Camera);
       

        //Vehaicle Input
        prop.m_VeicleInput.enabled = true;

        // Caracter Input 
        m_CheracterInputController.enabled = false;

        // Character Movement
        m_CharacterController.enabled = false;
        m_CharacterMovement.enabled = false;

        //Character Movement Vehicle
        if (m_Turret != null && m_ShootingVehicleInputControl != null)
        {
            m_Turret.enabled = true;
            m_ShootingVehicleInputControl.enabled = true;
        }
    

        // Hide Visual Model
        m_VisualModel.transform.localPosition = m_VisualModel.transform.localPosition + new Vector3(0, 10000, 0);

        // Collider 
        m_BoxCollider.enabled = false;
        
        m_EngineSound.enabled = true;
        m_Headlights.SetActive(true);


    }

    private void OnActionEnded()
    {
        
        ActionUseVehicleProperties prop = Properties as ActionUseVehicleProperties;

        InVehicle = false;
        // Camera 
        m_CheracterInputController.AssignCamera(m_Camera);


        //Vehaicle Input
        prop.m_VeicleInput.enabled = false;

        // Caracter Input 
        m_CheracterInputController.enabled = true;

        // Character Movement
        m_Owner.position = prop.InteractTransform.position;
        m_CharacterController.enabled = true;
        m_CharacterMovement.enabled = true;

        //Character Movement Vehicle
        if (m_Turret != null && m_ShootingVehicleInputControl != null)
        {
            m_Turret.enabled = false;
            m_ShootingVehicleInputControl.enabled = false;
        }
       

        // Show Visual Model
        m_VisualModel.transform.localPosition = new Vector3(0, 0, 0);

        // Collider 
        m_BoxCollider.enabled = true;


        m_EngineSound.enabled = false;
        m_Headlights.SetActive(false);
    }
}
