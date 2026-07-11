using UnityEngine;
using UnityEngine.EventSystems;

public class NoDeselectInputModule : StandaloneInputModule
{   //hierachy에서 eventSystem에서 Standalone Input Module를 지우고 해당 script를 삽입
    private GameObject lastSelected;

    public override void Process()
    {
        // 매 프레임 처리 직전, 현재 선택된 오브젝트가 있으면 기억해둠
        if (eventSystem.currentSelectedGameObject != null)
        {
            lastSelected = eventSystem.currentSelectedGameObject;
        }

        base.Process(); // 마우스/키보드 입력 등 기본 처리 실행

        // 처리 후 선택이 null로 풀렸는데, 선택지 패널이 열려있는 상태라면 복구
        if (eventSystem.currentSelectedGameObject == null &&
            lastSelected != null &&
            UIManager.Instance.panelData.isChoice)
        {
            eventSystem.SetSelectedGameObject(lastSelected);
        }
    }
}