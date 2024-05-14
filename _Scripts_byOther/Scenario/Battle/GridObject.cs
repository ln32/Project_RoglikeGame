using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class GridObject : MonoBehaviour
{
    public BaseAtBattle owner = null;
    public int index;
    public bool isEnemy;
    EventTrigger eventTrigger;
    public delegate void PassiveEffectHandler(BaseAtBattle _target);
    public PassiveEffectHandler EnterOnGrid;
    public PassiveEffectHandler ExitOnGrid;
    public Image imageRect { get; private set; }
    private Image imageBorder;

    public void InitObject()
    {
        imageBorder = GetComponent<Image>();
        imageRect = transform.GetChild(0).GetComponent<Image>();
        imageRect.enabled = false;
    }
    public void PreActive()
    {
        imageBorder.color = Color.red;
    }
    public void PreInactive()
    {
        imageBorder.color = new Color(1f,1f,1f,0.5f);
    }
    public GridObject SetClickEvent()
    {
        Entry downEntry = new();
        // Button 이벤트 추가
        gameObject.AddComponent<Button>().onClick.AddListener(() =>
        {
            OnGridClicked();
        });
        
        return this;
    }
    public GridObject SetDownEvent()
    {
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
        Entry downEntry = new();
        eventTrigger.triggers.Add(downEntry);
        downEntry.eventID = EventTriggerType.PointerDown;
        downEntry.callback.AddListener((data) =>
        {
            OnGridPointerDown();
        });
        return this;
    }
    public GridObject SetEnterEvent()
    {
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
        Entry enterEntry = new();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener((data) =>
        {
            OnGridPointerEnter(this);
        });
        eventTrigger.triggers.Add(enterEntry);
        return this;
    }
    public GridObject SetDragEvent()
    {
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
        Entry dragEntry = new();
        dragEntry.eventID = EventTriggerType.Drag;
        dragEntry.callback.AddListener((data) =>
        {
            // 현재 드래그되고 있는 포인트의 위치 가져오기
            PointerEventData pointerEventData = (PointerEventData)data;
            Vector2 dragPosition = pointerEventData.position;

            OnGridPointerDrag(dragPosition);
        });
        eventTrigger.triggers.Add(dragEntry);
        return this;
    }
    public GridObject SetExitEvent()
    {
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
        Entry exitEntry = new();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener((data) =>
        {
            OnGridPointerExit();
        });
        eventTrigger.triggers.Add(exitEntry);
        return this;
    }
    public GridObject SetUpEvent()
    {
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
        Entry upEntry = new();
        upEntry.eventID = EventTriggerType.PointerUp;
        upEntry.callback.AddListener((data) =>
        {
            OnGridPointerUp();
        });
        eventTrigger.triggers.Add(upEntry);
        
        return this;
    }
    internal void OnGridClicked()
    {
    }

    internal void OnGridPointerDown()
    {
        if (!owner) return;
        if (BattleScenario.battlePatern == BattlePatern.Battle)
        {
            if (GameManager.battleScenario.moveGauge < 10f) return;
        }
        GameManager.battleScenario.OnGridPointerDown();
        if (!isEnemy)
            imageRect.enabled = true;
    }

    internal void OnGridPointerDrag(Vector2 _dragPosition)
    {
        if (!GameManager.battleScenario.isInCharacter) return;
        if (!owner) return;
        if (!GameManager.battleScenario.isDragging) return;//시간에 대한 조건
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
    GetComponent<RectTransform>(),
    _dragPosition,
    GameManager.gameManager.canvasGrid.GetComponent<Canvas>().worldCamera,
    out var localPoint
);
        owner.transform.localPosition = localPoint;
    }

    internal void OnGridPointerEnter(GridObject _grid)
    {
        if (!GameManager.battleScenario.isDragging) return;
        GameManager.battleScenario.gridOnPointer = _grid;
        if (!isEnemy)
            imageRect.enabled = true;
    }

    internal void OnGridPointerExit()
    {
        GameManager.battleScenario.gridOnPointer = null;
        imageRect.enabled = false;
    }
    internal void OnGridPointerUp()
    {
        EventSystem.current.SetSelectedGameObject(null);
        InitBorder();
        if (!owner || !GameManager.battleScenario.isDragging) return;
        if (GameManager.battleScenario.gridOnPointer == null || GameManager.battleScenario.gridOnPointer == this|| !GameManager.battleScenario.gridOnPointer || isEnemy != GameManager.battleScenario.gridOnPointer.isEnemy)
        {
            GameManager.battleScenario.MoveCharacterByGrid(this, this);
        }
        else
        {
            GameManager.battleScenario.MoveCharacterByGrid(this, GameManager.battleScenario.gridOnPointer);

            if (BattleScenario.battlePatern != BattlePatern.OnReady)
            {
                GameManager.battleScenario.moveGauge = 0f;
            }
        }

        GameManager.battleScenario.gridOnPointer = null;
        GameManager.battleScenario.isDragging = false;
        GameManager.IsPaused = false;
    }
    private void InitBorder()
    {
        foreach (var x in BattleScenario.CharacterGrids)
        {
            x.imageRect.enabled = false;
        }
    }
}
