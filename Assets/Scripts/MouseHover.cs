using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseHover : MonoBehaviour
{
    public Collider2D enemyCollider;
    Collider2D dateCollider;

    //public void SetDateCollider(GameObject date)
    void Start()
    {
        dateCollider = GameObject.Find("table placeholder_0").GetComponent<Collider2D>();
    }

    Vector3 mouseToWorld()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    // Returns true if the mouse is over the monster
    public bool hitMonster()
    {
        return enemyCollider.OverlapPoint(mouseToWorld());
    }

    // TODO: Returns true if the mouse is over the date
    public bool hitDate()
    {
        return dateCollider.OverlapPoint(mouseToWorld());
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hitDate());
    }
}