using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private static Transform DamagePopupText;
    [SerializeField]
    private float damageAmount = 30;
    [SerializeField]
    private bool isWeakPoint = true;

    public static DamageText Create(Vector3 position, int damageAmount, bool isWeakPoint)
    {
        Transform damagePopupTransform = Instantiate(DamagePopupText, position, Quaternion.identity);
        DamageText damagePopup = damagePopupTransform.GetComponent<DamageText>();
        damagePopup.Setup(30, true);

        return damagePopup;
    }

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isWeakPoint)
    {
        
        textMesh.SetText(damageAmount.ToString());
        Color normalHitColor = new Color(1f, 0.92f, 0.016f, 1f);
        Color weakPointHitColor = new Color(1f, 0f, 0f, 1f);

        if (!isWeakPoint)
        {
            textMesh.fontSize = 36;
            textColor = normalHitColor;
        }
        else
        {
            textMesh.fontSize = 45;
            textColor = weakPointHitColor;

        }
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;
    }

    private void Update()
    {
        float moveYSpeed = 20f;
        float scaleTextAmount = 1f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            transform.localScale += Vector3.one * scaleTextAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}