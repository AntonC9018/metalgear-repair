using UnityEngine;

class UnitCamMovement : MonoBehaviour
{
    public float speed = 20f;

    public void FreeMovement()
    {
        Vector3 camPos = transform.position;
        if (Input.mousePosition.x > Screen.width - 30 || Input.GetKey(KeyCode.D))
        {
            camPos.x += speed * Time.deltaTime;
        }
        if (Input.mousePosition.x < 30 || Input.GetKey(KeyCode.A))
        {
            camPos.x -= speed * Time.deltaTime;
        }

        if (Input.mousePosition.y > Screen.height - 30 || Input.GetKey(KeyCode.W))
        {
            camPos.z += speed * Time.deltaTime;
        }
        if (Input.mousePosition.y < 30 || Input.GetKey(KeyCode.S))
        {
            camPos.z -= speed * Time.deltaTime;
        }
        transform.position = camPos;
    }
}
