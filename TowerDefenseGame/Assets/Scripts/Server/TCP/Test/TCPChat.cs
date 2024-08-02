using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TCPChat : MonoBehaviour
{
    public static TCPChat instance;
    private void Awake()
    {
        instance = this;
    }

    public TMP_InputField sendInput;
    public RectTransform chatContent;
    public TextMeshProUGUI chatText;
    public ScrollRect chatScrollRect;

    public void ShowMessage(string _data)
    {
        chatText.text += chatText.text == "" ? _data : "\n" + _data;

        Fit(chatText.GetComponent<RectTransform>());
        Fit(chatContent);
        Invoke("ScrollDelay", 0.03f);
    }

    public void Fit(RectTransform _rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);

    public void ScrollDelay() => chatScrollRect.verticalScrollbar.value = 0;

}
