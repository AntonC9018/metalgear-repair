using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputController : MonoBehaviour
{
    const int RIGHT = 1;
    const int LEFT = 0;
    public GameObject prefab;
    public List<Steering> selectedAgentsSteeringComponents = new List<Steering>(); 

    enum SelectionState
    {
        Selecting,
        None
    }

    enum ControllerState
    {
        Player,
        Unit
    }

    private Vector3 selectionStartMousePosition;
    private SelectionState selectionState = SelectionState.None;
    private Ray selectionStartRay;

    public Camera playerCamera;
    public Camera unitCamera;
    private ControllerState currentControllerState;
    private CharacterControl playerCharacterControl;
    private PlayerCamMovement playerCameraController;
    private UnitCamMovement unitCameraController;

    public KeyCode changeControllerStateKey;


    // Start is called before the first frame update
    void Start()
    {
        currentControllerState = ControllerState.Player;
        playerCharacterControl =
            GameObject.FindGameObjectsWithTag("PlayerManager")[0].GetComponent<CharacterControl>();
        playerCameraController =
            playerCamera.GetComponent<PlayerCamMovement>();
        unitCameraController =
            unitCamera.GetComponent<UnitCamMovement>();
        unitCamera.enabled = false;
        playerCamera.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(changeControllerStateKey)) //Keycode.N
        {
            if (currentControllerState == ControllerState.Player)
            {
                unitCamera.enabled = true;
                playerCamera.enabled = false;
                selectionState = SelectionState.None;
                currentControllerState = ControllerState.Unit;
            }
            else
            {
                unitCamera.enabled = false;
                playerCamera.enabled = true;
                selectionState = SelectionState.None;
                currentControllerState = ControllerState.Player;
            }
            print("Current State " + currentControllerState.ToString());
        }
        if (currentControllerState == ControllerState.Player)
        {
            PlayerController();
            playerCameraController.FollowPlayer();
        }
        else if (currentControllerState == ControllerState.Unit)
        {
            UnitController();
            unitCameraController.FreeMovement();
        }
        else
        {
            print("No State");
        }
    }

    void PlayerController()
    {
        playerCharacterControl.MoveCharacter();
    }

    // Update is called once per frame
    void UnitController()
    {
        var camera = unitCamera;
        if (Input.GetMouseButtonDown(LEFT))
        {
            if (selectionState == SelectionState.None)
            {
                selectionStartMousePosition = Input.mousePosition;
                selectionStartRay = camera.ScreenPointToRay(Input.mousePosition);
                selectionState = SelectionState.Selecting;
            }
            else
            {
                // TODO: draw the green selection thing
            }
        }

        else if (Input.GetMouseButtonUp(LEFT))
        {

            foreach (var selectedAgentSteeringComponent in selectedAgentsSteeringComponents) {
                selectedAgentSteeringComponent.BeDeselected();
            }

            if (selectionState == SelectionState.Selecting)
            {
                //var selectionStartPoint = 
                var selectionEndRay = camera.ScreenPointToRay(Input.mousePosition);
                selectedAgentsSteeringComponents = 
                    GetUnitsInSelectionBox(selectionStartRay, selectionEndRay);
                selectionState = SelectionState.None;
            }
        }

        if (Input.GetMouseButtonDown(RIGHT) && selectionState == SelectionState.None)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool didHit = Physics.Raycast(ray, out hit);

            if (didHit)
            {
                foreach (var steeringAgent in selectedAgentsSteeringComponents)
                {
                    steeringAgent.MoveTo(hit.point);
                }
            }
        }
    }

    float Lerp(float perc, float first, float second)
    {
        return perc * first + (1 - perc) * second;
    }

    List<Steering> GetUnitsInSelectionBox(Ray start, Ray end)
    {
        List<Steering> result = new List<Steering>();

        RaycastHit hitStart;
        RaycastHit hitEnd;

        if (!Physics.Raycast(start, out hitStart)) return result;
        if (!Physics.Raycast(end, out hitEnd)) return result;

        Instantiate(prefab, hitStart.point, Quaternion.identity);
        Instantiate(prefab, hitEnd.point, Quaternion.identity);

        print("yes");
        //return result;
        float dx = 1.0f;
        float numStepsX = Mathf.Abs((hitStart.point.x - hitEnd.point.x) / dx);
        float numStepsZ = Mathf.Abs((hitStart.point.z - hitEnd.point.z) / dx);
        

        for (float i = 0; i <= numStepsX; i++)
        {
            for (float j = 0; j <= numStepsZ; j++)
            {
                float percStartX = i / numStepsX;
                float percStartZ = j / numStepsZ;
                float x = Lerp(percStartX, hitStart.point.x, hitEnd.point.x);
                float y = Lerp(percStartX, hitStart.point.y, hitEnd.point.y) + 5;
                float z = Lerp(percStartZ, hitStart.point.z, hitEnd.point.z);


                Ray ray = new Ray(new Vector3(x, y, z), Vector3.down);
                RaycastHit hit;
                bool didHit = Physics.Raycast(ray, out hit);

                if (didHit)
                {
                    var character = hit.transform.gameObject.GetComponent<Steering>();
                    if (character != null)
                    {
                        result.Add(character);
                        character.BeSelected();
                    }

                    Instantiate(prefab, hit.point, Quaternion.identity);
                }
            }
        }

        return result.Distinct().ToList();
    }
}
