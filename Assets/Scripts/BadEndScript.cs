using UnityEngine;

public class BadEndScript : MonoBehaviour
{
    public GameObject blackScreen;
    public GameObject m1;
    public GameObject m2;
    public GameObject m3;
    public AudioSource monsterSound;
    Renderer renderer;
    Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = blackScreen.GetComponent<Renderer>();
        color = renderer.material.color;
        ShowMonster(0);
        renderer.material.color = new Color(color.r, color.g, color.b, 0);
    }

    Vector3 banishment = new Vector3(-20, 3, 0);
    public void ShowBlackScreen()
    {
        renderer.material.color = new Color(color.r, color.g, color.b, color.a - Time.deltaTime/10);
        color = renderer.material.color;
    }
    public void ShowMonster(int m)
    {
        if(m == 0)
        {
            m1.transform.position = banishment;
            m2.transform.position = banishment;
            m3.transform.position = banishment;

        }
        else if (m == 1)
        {
            m1.transform.position = new Vector3(6, 2, 0);
            m2.transform.position = banishment;
            m3.transform.position = banishment;
        }
        else if (m== 2)
        {
            m2.transform.position = new Vector3(-6, 0, 0);
            m1.transform.position = banishment;
            m3.transform.position = banishment;
        }
        else if (m == 3)
        {
            m3.transform.position = Vector3.zero;
            m1.transform.position = banishment;
            m2.transform.position = banishment;
        }
    }

    bool playSound = true;
    public void PlayMonster()
    {
        if (playSound)
        {
            Debug.Log("hit");
            monsterSound.Play();
            playSound = false;
            renderer.material.color = new Color(color.r, color.g, color.b, 1);
        }
        //ShowMonster(0);
        ShowBlackScreen();
    }

    float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 2+3+3+2+3+3+4+5+5 + 3+5+5 + 9 + 3 + 3+3+4+3+3+3+2+4+3+3+3+3)
        {
            renderer.material.color = new Color(color.r, color.g, color.b, 1);
        }
        else if (timer >= 2+3+3+2+3+3+4+5+5 + 3+5+5 + 9 + 3)
        {
            PlayMonster();
        }
        else if (timer >= 2+3+3+2+3+3+4+5+5 + 3+5+5 + 9)
        {
            ShowMonster(3);
        }
        else if (timer >= 2+3+3+2+3+3+4+5+5 + 3+5+5)
        {
            ShowMonster(2);
        }
        else if (timer >= 2+3+3+2+3+3+4+5+5)
        {
            ShowMonster(1);
        }
    }
}
