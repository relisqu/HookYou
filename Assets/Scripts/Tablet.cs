using System;
using DG.Tweening;
using Player_Scripts;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    [SerializeField] private Transform TextTransform;
    [SerializeField] private float TextSpeed;
    private PlayerMovement _player;

    private void Awake()
    {
        HideText();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            ShowText();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            HideText();
        }
    }

    private void ShowText()
    {
        TextTransform.DOScaleY(1f, TextSpeed).SetEase(Ease.OutCubic);
    }

    private void HideText()
    {
        TextTransform.DOScaleY(0f, TextSpeed).SetEase(Ease.OutCubic);
    }
}