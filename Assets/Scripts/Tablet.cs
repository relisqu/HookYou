using System;
using System.Linq;
using DefaultNamespace;
using DG.Tweening;
using Player_Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tablet : MonoBehaviour
{
    [SerializeField] private TextPopup TextPopup;
    [SerializeField] private TMPro.TMP_Text Text;
    [SerializeField] private float TextCorruptionValue;
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
        TextPopup.ShowText();
    }

    private void HideText()
    {
        TextPopup.HideText();
    }

    public void ResetText()
    {
        Text.SetText(_defaultText);
    }

    private string _defaultText;

    private void Start()
    {
        _defaultText = Text.text;
    }

    public void CorruptText()
    {
        var textString = Text.text;
        var newText = "";
        foreach (var word in textString.Split(' '))
        {
            if (Random.value < TextCorruptionValue)
            {
                var newWord = (new string(' ', word.Length + 1));
                newText += newWord;
            }
            else
            {
                var text = word;
                if (Random.value > TextCorruptionValue * 2.3f)
                {
                    var rand =  new string(text.ToCharArray().OrderBy(x=>Guid.NewGuid()).ToArray());
                    newText += rand + " ";
                }
                else
                {
                    newText += word + " ";
                }

            }
        }

        Text.SetText(newText);
    }
}