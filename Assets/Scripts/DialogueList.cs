using System.Collections.Generic;
using UnityEngine;

public class DialogueList : MonoBehaviour
{
    // A list of all the dialogue (entries are manually added in the inspector)
    [SerializeField] private List<Dialogue> dialogueList;

    [System.Serializable]
    public class Dialogue
    {
        // Can be left blank for a "narrator" type of speaker
        public string name;
        public string text;
        // Must not be less than or equal to 0
        public float time;
        // If left blank, the image doesn't update
        public Sprite sprite;
        // Optional
        public List<Choice> choices;
    }

    public List<Dialogue> GetDialogueList() { return dialogueList; }



    [System.Serializable]
    public class Choice
    {
        // The player's dialogue
        public string option;
        // How much love the player gains (whether positive or negative)
        public sbyte love;
        // The dialogue(s) the date says in response
        public List<Dialogue> dialogueList;
    }
}
