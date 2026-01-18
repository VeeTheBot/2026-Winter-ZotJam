using UnityEngine;
using UnityEngine.Rendering;
using System;

public class BlinkTimer: MonoBehaviour
{
    int love = 0;
    bool lookingAtDate = false;
    public const float eyeContactEnd = 7;
    float eyeContactTimer = 0f;

    public GameObject MouseHoverManager;
    MouseHover mh;

    public GameObject topLid;
    public GameObject botLid;
    public GameObject redAura;

    private GameObject monsterCurr;
    public GameObject monster1;
    public GameObject monster2;
    public GameObject monster3;
    //public GameObject monster4;
    System.Random rand = new System.Random();
    Vector3 banishment = new Vector3(-20, 3, 0);
    Vector3 monster3Pos;
    Vector3 monster4Pos;

    float yPosEnd; //2.5    // Height of lids mid blink
    float yPosStart = 8.0f; // Height of lids outside of screen
    const int yInt = 10; // blink speed interval
    int blinkState = 0;  // 0 = not blinking, 1 = eyes close, 2 = eyes open

    public const int blinkEnd = 5; // End timer for blink
    float blinkTimer = 0f;          // Keeps track of timer iterator

    Color redAuraColor;   // Color of renderer

    int monsterState = 0; // 0 = not there, 5 = death

    void Start()
    {
        monster3Pos = monster3.transform.position;
        //monster4Pos = monster4.transform.position;
        monsterState = 0;
        monsterCurr = monster1;
        monster1.transform.position = banishment;
        monster2.transform.position = banishment;
        monster3.transform.position = banishment;
        //monster4.transform.position = banishment;
        mh = MouseHoverManager.GetComponent<MouseHover>();
        mh.SetMonsterCollider(monsterCurr);
        UpdateMonster();

        yPosEnd = topLid.transform.position.y;
        redAuraColor = redAura.GetComponent<Renderer>().material.color;
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
            monsterCurr.transform.position = banishment;
            if (monsterState == 1)
            {
                monsterCurr = monster1;
                monsterCurr.transform.position = new Vector3(0, 2, 0);
            }
            else if (monsterState == 2)
            {
                monsterCurr = monster2;
                monsterCurr.transform.position = new Vector3(0, 0, 0);
            }
            else if (monsterState == 3)
            {
                monsterCurr = monster3;
                monsterCurr.transform.position = monster3Pos;
            }
            /*else if (monsterState == 4)
            {
                monsterCurr = monster4;
                monsterCurr.transform.position = monster4Pos;
            }*/
            mh.SetMonsterCollider(monsterCurr);
        }

        if(monsterState < 3)
            monsterCurr.transform.position = new Vector3(rand.Next(-6, 7), monsterCurr.transform.position.y, monsterCurr.transform.position.z);
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
        redAura.GetComponent<Renderer>().material.color = redAuraColor;
    }

    public void UpdateLove(int val)
    {
        love += val;
    }

    void LoveIncrement()
    {
        if(lookingAtDate != mh.hitDate())
        {
            lookingAtDate = mh.hitDate();
            eyeContactTimer = 0f;
        }
        if (eyeContactTimer < eyeContactEnd)
        {
            eyeContactTimer += Time.deltaTime;
            if (eyeContactTimer >= eyeContactEnd)
            {
                if (mh.hitDate())
                    UpdateLove(1);
                else
                    UpdateLove(-1);
                Debug.Log("Love: " + love);
                eyeContactTimer = 0f;
            }
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
