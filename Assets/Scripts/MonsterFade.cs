using UnityEngine;

// Note: Only for use in the intro scene (to fade in) and the outro scene (to fade out)
public class MonsterFade : MonoBehaviour
{
    // The monster's sprite renderer
    private SpriteRenderer renderer;
    // The dialogue list (holds the indices to compare against)
    private PlayDialogue dialogueList;
    // At what dialogue index should the monster fade in? (Assigned in the inspector; -1 means it doesn't fade in)
    [SerializeField] private int fadeInIndex = -1;
    // At what dialogue index should the monster fade out? (Assigned in the inspector; -1 means it doesn't fade out)
    [SerializeField] private int fadeOutIndex = -1;
    // How fast the monster should fade in or out (in seconds)
    [SerializeField] private float fadeTime = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        dialogueList = FindAnyObjectByType<PlayDialogue>();

        // Hide the monster if it's meant to be shown
        if(fadeInIndex != -1)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the fade-in index is reached, fade in the monster
        if(dialogueList.GetIndex() == fadeInIndex)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        }
    }
}
