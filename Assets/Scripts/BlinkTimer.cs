using UnityEngine;
using UnityEngine.Rendering;
using System;

public class BlinkTimer: MonoBehaviour
{
    int love = 0;
    public const float eyeContactEnd = 7;
    float eyeContactTimer = 0f;

    public GameObject MouseHoverManager;
    MouseHover mh;

    public GameObject topLid;
    public GameObject botLid;
    public GameObject redAura;

    public GameObject monster;
    System.Random rand = new System.Random();

    float yPosEnd; //2.5    // Height of lids mid blink
    float yPosStart = 8.0f; // Height of lids outside of screen
    const int yInt = 10; // blink speed interval
    int blinkState = 0;  // 0 = not blinking, 1 = eyes close, 2 = eyes open

    public const int blinkEnd = 5; // End timer for blink
    float blinkTimer = 0f;          // Keeps track of timer iterator

    Renderer redRenderer; // Renderer for red aura
    Color redAuraColor;   // Color of renderer

    int monsterState = 0; // 0 = not there, 5 = death
    const float monsterSizeStart = 0.325f;
    float monsterSize;

    void Start()
    {
        monsterState = 0;
        monsterSize = monster.transform.localScale.x;
        mh = MouseHoverManager.GetComponent<MouseHover>();

        yPosEnd = topLid.transform.position.y;
        redRenderer = redAura.GetComponent<Renderer>();
        redAuraColor = redRenderer.material.color;
        UpdateRedness(0f);

        topLid.transform.position = new Vector3(0, yPosStart, 0);
        botLid.transform.position = new Vector3(0, -yPosStart, 0);
    }

    public void ResetMonster()
    {
        monsterState = -1;
        UpdateMonster();
    }

    /* Increment monsterState and change sprite
     */
    void UpdateMonster()
    {
        if (!mh.hitMonster())
        {
            monsterState++;
            if (monsterState == 0)
                monsterSize = monsterSizeStart;
            else
                monsterSize += 0.1f;
        }

        monster.transform.position = new Vector3(rand.Next(-9, 9), monster.transform.position.y, monster.transform.position.z);
        // TODO: change sprites
        monster.transform.localScale = new Vector3(monsterSize, monsterSize, monsterSize);
        // TODO: add condition for 5th state
    }

    /* Updates redness transparency.
     * If a=0, it sents transparency to 0
     * If a is anything else, it increments transparency by a.
     * I know it's bad design but I didn't want to rewrite it.
     */
    void UpdateRedness(float a)
    {
        if (a == 0)
            redAuraColor.a = a;
        else if (redAuraColor.a + a <= 1f)
            redAuraColor.a += a;
        redRenderer.material.color = redAuraColor;
    }

    public void UpdateLove(int val)
    {
        love += val;

Debug.Log("Love: " + love);
    }

    void LoveIncrement()
    {
        if (eyeContactTimer < eyeContactEnd && mh.hitDate())
        {
            eyeContactTimer += Time.deltaTime;
            if (eyeContactTimer >= eyeContactEnd)
            {
                // love++;
UpdateLove(1);
                Debug.Log("Love: " + love);
                eyeContactTimer = 0f;
            }
        }
        else
        {
            eyeContactTimer = 0f;
        }
    }

    void Blink()
    {
        // If blinkState = 0, increment timer for blink and redness
        if (blinkTimer < blinkEnd && blinkState == 0)
        {
            blinkTimer += Time.deltaTime;
            // Redness starts after half the time
            if (blinkTimer >= blinkEnd / 2)
                UpdateRedness(Time.deltaTime / 2);
        }
        // Once timer is done, blink and reset
        else
        {
            BlinkLogic();
            blinkTimer = 0f;
        }
    }

    /* If blinkState = 0, set blinkState to 1
     * If blinkState = 1, close eyes and clear redness once closed
     * If blinkState = 2, open eyes
     */
    void BlinkLogic(bool usingEyedrops = false)
    {
        if (blinkState == 0)
        {
            blinkState = 1;
        }
        if (blinkState == 1)
        {
            if (topLid.transform.position.y >= yPosEnd)
            {
                topLid.transform.Translate(0, -yInt * Time.deltaTime, 0);
                botLid.transform.Translate(0, yInt * Time.deltaTime, 0);
            }
            else
            {
                blinkState = 2;
                UpdateRedness(0f);
                if (usingEyedrops)
                    monsterState = -1;
                UpdateMonster();
            }
        }
        if (blinkState == 2)
        {
            if (topLid.transform.position.y <= yPosStart)
            {
                topLid.transform.Translate(0, yInt * Time.deltaTime, 0);
                botLid.transform.Translate(0, -yInt * Time.deltaTime, 0);
            }
            else
                blinkState = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Blink();
        LoveIncrement();
    }
}
