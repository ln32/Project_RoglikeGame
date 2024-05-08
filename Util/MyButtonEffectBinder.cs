using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyButtonEffectBinder : MonoBehaviour
{
    private Button button;
    [SerializeField] Image[] images;

    void Start()
    {
        // Button 컴포넌트 가져오기
        button = GetComponent<Button>();

        // 클릭 이벤트 핸들러 등록
        button.onClick.AddListener(OnButtonClick);

        // EventTrigger 컴포넌트 가져오기 또는 추가하기
        EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        // 마우스가 버튼 위에 올라갔을 때의 이벤트 핸들러 등록
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((data) => { OnButtonHover(); });
        eventTrigger.triggers.Add(pointerEnter);

        // 마우스가 버튼에서 나갔을 때의 이벤트 핸들러 등록
        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((data) => { OnButtonExit(); });
        eventTrigger.triggers.Add(pointerExit);
    }

    void OnButtonClick()
    {
        for (int i = 0; i < images.Length; i++)
        {
            Graphic m_TargetGraphic = images[i];
            if (m_TargetGraphic == null)
                return;

            m_TargetGraphic.color = new Color(m_TargetGraphic.color.r, m_TargetGraphic.color.g, m_TargetGraphic.color.b, button.colors.pressedColor.a);
        }
    }

    void OnButtonHover()
    {
        for (int i = 0; i < images.Length; i++)
        {
            Graphic m_TargetGraphic = images[i];
            if (m_TargetGraphic == null)
                return;

            m_TargetGraphic.color = new Color(m_TargetGraphic.color.r, m_TargetGraphic.color.g, m_TargetGraphic.color.b, button.colors.pressedColor.a);
        }
    }

    void OnButtonExit()
    {
        for (int i = 0; i < images.Length; i++)
        {
            Graphic m_TargetGraphic = images[i]; 
            if (m_TargetGraphic == null)
                return;

            m_TargetGraphic.color = new Color(m_TargetGraphic.color.r, m_TargetGraphic.color.g, m_TargetGraphic.color.b, button.colors.pressedColor.a);
        }
    }
}