using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] RectTransform cover, face;
    [SerializeField] TextMeshProUGUI cardNumberText;
    float cardFlipSpeed = 1f;
    

    public void AssignCard(CardData data)
    {
        cardNumberText.text = data.cardNumber.ToString();
    }

    public void Show()
    {
        RotateAroundYAxis(cover, 90f, () =>
        {
            cover.gameObject.SetActive(false);
            face.gameObject.SetActive(true);
            RotateAroundYAxis(face, 180, () =>
            {

            });
        });
    }

    public void Hide()
    {
        RotateAroundYAxis(face, 90f, () =>
        {
            face.gameObject.SetActive(false);
            cover.gameObject.SetActive(true);
            RotateAroundYAxis(cover, 0f, () =>
            {

            });
        });
    }


    void RotateAroundYAxis(RectTransform targetTransform, float targetRotationInY, Action callback = null)
    {
        StartCoroutine(RotateRectTransform(targetTransform, new Vector3(0f, targetRotationInY, 0f), callback));
    }

    IEnumerator RotateRectTransform(RectTransform targetTransform, Vector3 targetRotation, Action callback = null)
    {
        Vector3 startingRotation = targetTransform.rotation.eulerAngles;
        float elapsed = 0f;

        while (elapsed < cardFlipSpeed / 2)
        {
            targetTransform.rotation = Quaternion.Euler(Vector3.Lerp(startingRotation, targetRotation, elapsed / (cardFlipSpeed / 2)));
            elapsed += Time.deltaTime;
            yield return null;
        }

        callback?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Show();
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Hide();
        }
    }
}
