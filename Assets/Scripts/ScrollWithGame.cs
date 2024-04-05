using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollWithGame : MonoBehaviour
{
    public float scrollSpeed;

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.status == GameStatus.Scrolling)
        {
            transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        }   
    }
}
