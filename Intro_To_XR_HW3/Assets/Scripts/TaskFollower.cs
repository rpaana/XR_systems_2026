using UnityEngine;

public class TaskFollower : MonoBehaviour
{
    [SerializeField] private GameObject SocketLight;
    [SerializeField] private GameObject PunchButtonLight;
    [SerializeField] private GameObject LeverLight;
    [SerializeField] private GameObject RedPanel;
    [SerializeField] private GameObject GreenPanel;

    // This method disables the red panel and enables the green panel when all the lights are active.
    private void Update()
    {
        // We check the red panel first so that when the panel is already green, this is not executed again.
        // I think it will reduce the lag a little bit.
        if (RedPanel.activeSelf && SocketLight.activeSelf && PunchButtonLight.activeSelf && LeverLight.activeSelf)
        {
            RedPanel.SetActive(false);
            GreenPanel.SetActive(true);
        }
    }
}
