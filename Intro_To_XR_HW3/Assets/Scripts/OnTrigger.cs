using UnityEngine;

public class OnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject redLight;
    [SerializeField] private GameObject greenLight;
    [SerializeField] private GameObject InstructionText;
    [SerializeField] private GameObject SuccessText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ButtonCollider" || other.name == "LeverCollider")
        {
            redLight.SetActive(false);
            greenLight.SetActive(true);
            InstructionText.SetActive(false);
            SuccessText.SetActive(true);
        }
    }
}
