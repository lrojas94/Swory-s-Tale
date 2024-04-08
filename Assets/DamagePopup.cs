using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    TextMeshPro textMesh;
    private float dissapearTimer;
    private float dissapearSpeed;
    private float moveYSpeed = 0.75f;
    private Color textColor;

    private const float DISSAPEAR_TIMER_MAX = 0.25f;


    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }
    public void Setup(int damage)
    {
        Debug.Log(textMesh);
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

        textMesh.SetText(damage.ToString());
        dissapearTimer = DISSAPEAR_TIMER_MAX;
        dissapearSpeed = 4f;
        textColor = textMesh.color;
    }

    public static DamagePopup ShowDamage(int damage, Vector3 position)
    {
        Debug.Log(damage);
        GameObject instance = Instantiate(AssetManager.Instance.DamagePopupPrefab);
        instance.transform.position = position;
        DamagePopup damagePopup = instance.GetComponent<DamagePopup>();
        damagePopup.Setup(damage);

        return damagePopup;
    }

    private void Update()
    {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        if (dissapearTimer >= DISSAPEAR_TIMER_MAX * 0.5f)
        {
            float increaseSpeed = 2f;
            transform.localScale += Vector3.one * increaseSpeed * Time.deltaTime;
        } else
        {
            float decreaseSpeed = 1f;
            transform.localScale -= Vector3.one * decreaseSpeed * Time.deltaTime;
        }
        dissapearTimer -= Time.deltaTime;
        if (dissapearTimer <= 0)
        {
            textColor.a -= dissapearSpeed * Time.deltaTime;
            textMesh.color = textColor;
        }

        if (textColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
