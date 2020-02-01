using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputController : MonoBehaviour
{
    const int RIGHT = 1;
    const int LEFT = 0;
    public Camera camera;
    public GameObject prefab;
    public List<Steering> selectedAgentsSteeringComponents = new List<Steering>(); 

    enum SelectionState
    {
        Selecting,
        None
    }
    
    private SelectionState selectionState = SelectionState.None;
    private Vector3 selectionStartCoord;
    private Ray selectionStartRay;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(LEFT))
        {
            print("LEFT PUSHED");
            if (selectionState == SelectionState.None)
            {
                selectionStartCoord = Input.mousePosition;
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
            print("LEFT Released");

            foreach (var selectedAgentSteeringComponent in selectedAgentsSteeringComponents) {
                selectedAgentSteeringComponent.BeDeselected();
            }

            if (selectionState == SelectionState.Selecting)
            {
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

        Vector3 currentOrigin = start.origin;
        Vector3 currentDirection = start.direction;

        float dx = 0.015f;
        float numStepsX = Mathf.Abs((end.origin.x - start.origin.x) / dx);
        float numStepsZ = Mathf.Abs((end.origin.z - start.origin.z) / dx);
        

        for (float i = 0; i <= numStepsX; i++)
        {
            for (float j = 0; j <= numStepsZ; j++)
            {
                float percStartX = i / numStepsX;
                float percStartZ = j / numStepsZ;
                float x = Lerp(percStartX, start.origin.x, end.origin.x);
                float y = Lerp(percStartX, start.origin.y, end.origin.y);
                float z = Lerp(percStartZ, start.origin.z, end.origin.z);
                float dirx = Lerp(percStartX, start.direction.x, end.direction.x);
                float diry = Lerp(percStartX, start.direction.y, end.direction.y);
                float dirz = Lerp(percStartZ, start.direction.z, end.direction.z);


                Ray ray = new Ray(new Vector3(x, y, z), new Vector3(dirx, diry, dirz));
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

                    //Instantiate(prefab, hit.point, Quaternion.identity);
                }
            }
        }

        return result.Distinct().ToList();
    }
}
