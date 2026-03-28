using UnityEngine;

public class Door : MonoBehaviour
{
    private Vector3 closedPos;
    public Vector3 openOffset = new Vector3(0,-100,0);
    public float speed = 2f;
    private bool isOpen = false;

    private void Start()
    {
        closedPos = transform.position;

    }

    private void Update()
    {
        Vector3 targetPos = isOpen ? closedPos + openOffset : closedPos;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);

    }

    public void OpenDoor() { isOpen = true; }
    public void CloseDoor() { isOpen = false; }
}
