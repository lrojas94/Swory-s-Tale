using System.Collections;
using UnityEngine;

public enum Element
{
    None = 0,
    Thunder = 1,
    Ground = 2,
    Water = 3
}
public static class ElementDictionary
{
    public static float GetElementMultiplier(Element baseElement, Element targetElement)
    {
        switch (baseElement) {
            case Element.Thunder:
                if (targetElement == Element.Ground) return 0.9f;
                if (targetElement == Element.Water) return 1.1f;
                return 1f;
            case Element.Ground:
                if (targetElement == Element.Water) return 0.9f;
                if (targetElement == Element.Thunder) return 1.1f;
                return 1f;
            case Element.Water:
                if (targetElement == Element.Thunder) return 0.9f;
                if (targetElement == Element.Ground) return 1.1f;
                return 1f;
            default:
                return 1f;

        }
    }
} 