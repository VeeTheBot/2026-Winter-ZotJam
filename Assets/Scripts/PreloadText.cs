using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class PreloadText : MonoBehaviour
{
    // A plaintext file containing all valid names and their alternate identifiers (assigned in the inspector)
    public TextAsset namesFile;
    // A dictionary of all names and their possible identifiers
    private Dictionary<string, string> names;

    // The dialogue files to load (assigned in the inspector)
    public List<TextAsset> dialogueFiles;
    // The dialogue lines and choices generated
    private List<Line> lines;
    // The current line to display
    private Line currentLine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the names dictionary
        names = new Dictionary<string, string>();
        NameAssignment();

        // Translate from plaintext into events
        lines = new List<Line>();
        TranslateText();
    }



    // Gather the names and their shorthand identifiers from the names file
    private void NameAssignment()
    {
        // Goes through each line of the names file
        foreach(string line in namesFile.text.Split("\n"[0]))
        {
            // Splits the line into its individual names
            string[] words = line.Split(" ");
            // Add the name-id pair to the dictionary
            names.Add(words[0], words[2]);
        }
    }



    // Go through all files provided in the inspector and translate them into dialogue to display and events to run
    public void TranslateText()
    {
        // Go through all files
        foreach(TextAsset file in dialogueFiles)
        {
            string[] linesFromFile = file.text.Split("\n"[0]);

            Line previousLine = null;

            // Go through all the lines in the file
            foreach(string line in linesFromFile)
            {
                string[] words = line.Split(" ");
                Line newLine = new Line();

                // Check what type of line this is

                // If the line is narration...
                if(words[0].Contains("\""))
                {
                    // The last word is the time
                    float time = float.Parse(words[words.Length-1]);
                    // All other words are part of the dialogue.
                    string dialogue = "";
                    for(int z = 0; z < words.Length-1; z++)
                    {
                        dialogue += words[z];
                    }
                    dialogue = dialogue.Substring(1, dialogue.Length-2);

                    newLine = new Dialogue("", dialogue, time);
                }

                // Else, if the line is dialogue...
                else if(names.ContainsKey(words[0]) || names.ContainsValue(words[0]))
                {
                    // The first word is the name
                    string name = words[0];
                    // The last word is the time
                    float time = float.Parse(words[words.Length-1]);
                    // All other words are part of the dialogue.
                    string dialogue = "";
                    for(int z = 1; z < words.Length-1; z++)
                    {
                        dialogue += words[z];
                    }
                    dialogue = dialogue.Substring(1, dialogue.Length-2);

                    newLine = new Dialogue(name, dialogue, time);
                }

                // Else, if the line is a menu...
                else if(words[0].Contains("menu"))
                {
                    // Start gathering options
                }

                // Else, if the line is an option...
                else if(words[0].Contains("option"))
                {
                    // Add options to the menu
                }

                // Else, if the line is a menu end...
                else if(words[0].Contains("menuEnd"))
                {
                    // Close the menu
                }

                // Else, if the line is a love increment...
                else if(words[0].Contains("love"))
                {}

                // Else, if the line is a resume...
                else if(words[0].Contains("resume"))
                {}

                // Else, if the line is a return...
                else if(words[0].Contains("return"))
                {}

                // Add the new line
                lines.Add(newLine);

                // Connect the previous line
                if(previousLine != null)
                {
                    previousLine.Next = lines.Last();
                }

                // Move on to the next line
                previousLine = lines.Last();
            }
        }
    }

    public void IncrementLove(sbyte p)
    {
        // Increment the love by p points
    }



    // Meaning "a line of text in the text file", not "a line of dialogue"
    [System.Serializable]
    public class Line
    {
        // The next line to load
        protected Line next;
        // How long the "line" should stay on-screen for (this becomes more relevant in subclasses; it's useless here)
        protected float time;

        public Line(float t = 0) { time = t; }

        public Line Next
        {
            get => next;
            set => next = value;
        }
    }

    // The actual text to be spoken aloud
    [System.Serializable]
    public class Dialogue : Line
    {
        // Name of the speaker
        string name;
        // Dialogue to say
        string dialogue;

        public Dialogue(string n, string d, float t)
        {
            name = n;
            dialogue = d;
            time = t;
        }
    }
}

