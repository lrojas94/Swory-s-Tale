using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance;
    public GameObject DamagePopupPrefab;
    public GameObject ExplosionPrefab;

    private void Awake()
    {
        if (Instance == null) {
           Instance = this;
        }
    } 
}
