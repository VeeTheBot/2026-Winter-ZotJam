using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class PlayDialogue : MonoBehaviour
{
    // The populated dialogue list (entries are manually added in the inspector)
    DialogueList list;
    // The index of the current dialogue we're on
    int index;
    // The index of the current choice dialogue we're inside. -1 means we're not inside a choice
    int choiceIndex;
    int choiceDialogueIndex;
    // The timer's total time
    float totalTime;
    // The timer's current time (ticks down to zero)
    float currentTime;

    /// Objects manually grabbed from the hierarchy
        // The speaker's name
        [SerializeField] private TMP_Text name;
        // The speaker's dialogue box
        [SerializeField] private TMP_Text text;
        // The timer element (only shown for choices; hidden otherwise)
        [SerializeField] private Slider timer;
        // The date's sprite
        [SerializeField] private SpriteRenderer renderer;
        // Name and Timer parent
        [SerializeField] private GameObject nameAndTimer;
        // Choice buttons parent
        [SerializeField] private GameObject choiceButtons;
        // Button children
        [SerializeField] private TMP_Text button1;
        [SerializeField] private TMP_Text button2;
        [SerializeField] private TMP_Text button3;
    
    // Script with love update
    private BlinkTimer loveUpdate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Fetch the love update
        loveUpdate = FindAnyObjectByType<BlinkTimer>();
        if(loveUpdate == null)
        {
            // Debug.LogError("Love Update not found!");
        }

        // Fetch the dialogue list
        list = GetComponent<DialogueList>();

        // Initialize the indices
        index = choiceDialogueIndex = 0;
        choiceIndex = -1;

        // Preload the first dialogue
        PreloadDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        // Tick down the timer and update the timer display
        currentTime -= Time.deltaTime;
        timer.value = currentTime / totalTime;

        // If the timer reaches the end...
        if(currentTime <= 0)
        {
            // If the user is inside a choice, the time has run out and they haven't selected an option
            if(list.GetDialogueList()[index].choices.Count != 0)
            {
                // If there is no more dialogue, exit out of the choice
                try
                {
                    if(choiceDialogueIndex >= list.GetDialogueList()[index].choices[choiceIndex].dialogueList.Count)
                    {
                        choiceIndex = -1;
                        choiceDialogueIndex = 0;
                    }
                }
                catch(Exception e)
                {
                    choiceIndex = 3;
                    choiceDialogueIndex = 0;
                }
            }

            // If a choice index is active, progress down the choice dialogue
            if(choiceIndex != -1)
            {
                PreloadChoiceDialogue();
            }

            // Otherwise, move to the next dialogue
            else
            {
                index++;
                PreloadDialogue();
            }
        }
    }

    void PreloadDialogue()
    {
        // If the index is out-of-bounds, don't update
        if(index < list.GetDialogueList().Count)
        {
            // Run the scene inject if properly set up
            if(list.GetDialogueList()[index].name.ToLower().Equals("scene"))
            {
                SceneManager.LoadScene(list.GetDialogueList()[index].text);
            }

            // Update the name and dialogue
            name.text = list.GetDialogueList()[index].name;
            text.text = list.GetDialogueList()[index].text;

            // Reset the timer
            ResetTimer(list.GetDialogueList()[index].time);

            // If an image was provided (and the renderer is provided), update the sprite
            if(list.GetDialogueList()[index].sprite != null && renderer != null)
            {
                renderer.sprite = list.GetDialogueList()[index].sprite;
            }

            // If the name is blank (i.e., the narrator), hide the speaker name textbox and the timer
            nameAndTimer.SetActive(!name.text.Equals(""));

            // If the dialogue has choices...
            if(list.GetDialogueList()[index].choices.Count != 0)
            {
                // Show the timer
                timer.gameObject.SetActive(true);

                // Show the buttons
                choiceButtons.SetActive(true);

                // Update the button texts
                button1.text = list.GetDialogueList()[index].choices[0].option;
                button2.text = list.GetDialogueList()[index].choices[1].option;
                button3.text = list.GetDialogueList()[index].choices[2].option;
            }

            // Otherwise, hide the timer and the buttons
            else
            {
                timer.gameObject.SetActive(false);
                choiceButtons.SetActive(false);
            }
        }
    }

    void ResetTimer(float t)
    {
        totalTime = currentTime = t;
    }

    public void ClickChoice(int option)
    {
        choiceIndex = option;
        choiceDialogueIndex = 0;
        PreloadChoiceDialogue();
    }

    void PreloadChoiceDialogue()
    {
        // Update the love on the first dialogue
        if(choiceDialogueIndex == 0)
        {
            loveUpdate.UpdateLove(list.GetDialogueList()[index].choices[choiceIndex].love);
        }

        // Update the name and dialogue
        name.text = list.GetDialogueList()[index].choices[choiceIndex].dialogueList[choiceDialogueIndex].name;
        text.text = list.GetDialogueList()[index].choices[choiceIndex].dialogueList[choiceDialogueIndex].text;

        // Reset the timer
        ResetTimer(list.GetDialogueList()[index].choices[choiceIndex].dialogueList[choiceDialogueIndex].time);
        
        // If an image was provided (and the renderer is provided), update the sprite
        if(list.GetDialogueList()[index].choices[choiceIndex].dialogueList[choiceDialogueIndex].sprite != null && renderer != null)
        {
            renderer.sprite = list.GetDialogueList()[index].choices[choiceIndex].dialogueList[choiceDialogueIndex].sprite;
        }

        // Hide the timer and the buttons
        timer.gameObject.SetActive(false);
        choiceButtons.SetActive(false);

        // Move to the next choice dialogue
        choiceDialogueIndex++;
    }
}
