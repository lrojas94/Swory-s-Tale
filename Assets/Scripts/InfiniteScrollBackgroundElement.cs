using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InfiniteScrollBackgroundElement : MonoBehaviour
{
    [SerializeField]
    private ScrollableBackground scrollableBackground;
    private List<GameObject> instances;
    void Awake()
    {
        GenerateSprites();
    }

    // Update is called once per frame
    void Update()
    { 
        if (GameController.Instance.status == GameStatus.Scrolling)
        {
            var sprite = scrollableBackground.sprite;

            for (var i = 0;  i < instances.Count; i++)
            {
                var instance = instances[i];
                var screenPos = Camera.main.WorldToScreenPoint(instance.transform.position);
                if (screenPos.x + 2 * (sprite.bounds.size.x * sprite.pixelsPerUnit) < 0)
                {
                    Debug.Log($"{screenPos.x} .. {sprite.rect.size.x} .. {sprite.bounds.size}");
                    var lastInstance = instances.Last();
                    instance.transform.position = new Vector3(lastInstance.transform.position.x + (1 * scrollableBackground.scale) + (scrollableBackground.offset / sprite.pixelsPerUnit), transform.position.y, transform.position.z);
                    instances.RemoveAt(i);
                    instances.Add(instance);
                    i--;
                }
            }
        }
    }

    void GenerateSprites()
    {

        var sprite = scrollableBackground.sprite;
        var screenWidth = Screen.width;
        var spriteWidth = sprite.bounds.size.x * sprite.pixelsPerUnit * scrollableBackground.scale;
        var test =  Mathf.Tan(Mathf.Rad2Deg * Camera.main.fieldOfView / 2) * Mathf.Abs(Vector3.Distance(transform.position,Camera.main.transform.position));
        Debug.Log(test);
        var spriteCount = 2 * (int)(Mathf.Ceil(screenWidth / spriteWidth));

        instances = new List<GameObject>();

        
        for (var i = -spriteCount; i <= spriteCount; i++)
        {
            // instantiate this amount of prefabs:
            GameObject instance = new GameObject(scrollableBackground.name);
            instance.transform.parent = transform;
            if (scrollableBackground.scale > 1)
            {
                instance.transform.localScale = new Vector3(scrollableBackground.scale, scrollableBackground.scale, scrollableBackground.scale);
            }

            SpriteRenderer renderer = instance.AddComponent<SpriteRenderer>();
            ScrollWithGame scrollWithGame = instance.AddComponent<ScrollWithGame>();
            scrollWithGame.scrollSpeed = scrollableBackground.scrollSpeed;
            renderer.sprite = sprite;
            instance.transform.position = new Vector3(i * scrollableBackground.scale + (scrollableBackground.offset / sprite.pixelsPerUnit), transform.position.y, transform.position.z);
            instances.Add(instance);
        }
    }
}
