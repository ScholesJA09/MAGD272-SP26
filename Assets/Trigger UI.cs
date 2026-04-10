using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public string roomName;
    public RoomTitleUI roomTitleUI;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            roomTitleUI.ShowTitle(roomName);
        }
    }
}