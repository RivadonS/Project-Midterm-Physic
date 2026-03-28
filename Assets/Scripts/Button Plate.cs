using UnityEngine;

public class ButtonPlate : MonoBehaviour
{
    public Door targetDoor; // ลาก Object ประตูมาใส่ช่องนี้ใน Inspector

    void OnTriggerEnter(Collider other)
    {
        // ถ้าผู้เล่น หรือกล่อง (Box) มาทับปุ่ม ให้เปิดประตู
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            targetDoor.OpenDoor();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            targetDoor.CloseDoor();
        }
    }
}
