using UnityEngine;

// Reset the love back to zero (so that the value doesn't transfer over from previous playthroughs)
public class ResetLove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
Debug.Log("Love: " + FindAnyObjectByType<BlinkTimer>().GetLove());
        FindAnyObjectByType<BlinkTimer>().SetLove(0);
Debug.Log("Love: " + FindAnyObjectByType<BlinkTimer>().GetLove());
    }
}
