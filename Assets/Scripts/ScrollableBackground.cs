using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName="ScrollableBackground", menuName = "Scrollable Background")]
public class ScrollableBackground: ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public float scrollSpeed = 1;
    public float offset = 0;
    public float scale = 1f;
}
