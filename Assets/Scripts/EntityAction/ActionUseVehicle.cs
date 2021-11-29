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
        prop.m_VeicleInput.AssignCamera(m_Camera);

        //Vehaicle Input
        prop.m_VeicleInput.enabled = true;

        // Caracter Input 
        m_CheracterInputController.enabled = false;

        // Character Movement
        m_CharacterController.enabled = false;
        m_CharacterMovement.enabled = false;

        // Hide Visual Model
        m_VisualModel.transform.localPosition = m_VisualModel.transform.localPosition + new Vector3(0, 10000, 0);

      


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

        // Show Visual Model
        m_VisualModel.transform.localPosition = new Vector3(0, 0, 0);

       

    }
}
