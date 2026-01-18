using UnityEngine;
using UnityEngine.Rendering;

public class Heart : MonoBehaviour
{
    Renderer heartRenderer;
    Color heartColor;

    int animate = 0; //0 nothing, 1 fade in, 2 fade out
    float heartEnd = 1f;
    float heartTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        heartRenderer = GetComponent<Renderer>();
        heartColor = heartRenderer.material.color;
        heartColor = new Color(1, 1, 1, 0);
    }

    public void ShowHeart(int val)
    {
        if(val > 0)
            heartColor = new Color(1, 1, 1, 0);
        else if (val < 0)
            heartColor = new Color(0, 0, 0, 0);

        if (val != 0)
        {
            heartRenderer.material.color = heartColor;
            animate = 1;
        }
        else
            animate = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (animate == 1)
        {
            if (heartTimer < heartEnd)
            {
                heartTimer += Time.deltaTime;
            }
            if (heartTimer >= heartEnd)
                animate = 2;
        }
        else if (animate == 2)
        {
            if (heartTimer > 0)
            {
                heartTimer -= Time.deltaTime;
            }
            if (heartTimer <= 0)
                animate = 0;
        }

        heartRenderer.material.color = new Color(heartColor.r, heartColor.g, heartColor.b, heartTimer);
    }
}
