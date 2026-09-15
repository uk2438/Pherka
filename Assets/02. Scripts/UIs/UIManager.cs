using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DialogueSystem;
using System;
using System.Collections.Generic;
using TMPro;

public class UIManager : Singleton<UIManager>
{

    // ── 초상화 index ──────────────────────────────────────────
    private int prevPotraitIdx = -1;
    private int currPotraitIdx = -1;
    // ── 볼륨 슬라이더 ──────────────────────────────────────────
    [Header("볼륨 슬라이더")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    // ── 설정 페이지 ────────────────────────────────────────────
    [Header("설정 페이지")]
    [SerializeField] private Animator pageTurnAnimator; // PausePanel 애니메이터
    [SerializeField] private GameObject buttonList;     // 기본 버튼 목록
    [SerializeField] private GameObject settingUI;      // 설정 UI (SFXSlider, BGMSlider)

    // ── Choice UI ────────────────────────────────────────────
    [Header("선택지 UI")]
    [SerializeField] private Button choiceButtonPrefab;
    [SerializeField] private Transform choiceButtonParent;
    [SerializeField] private GameObject choicePanel;

    private readonly List<Button> choiceButtons = new List<Button>();
    private Coroutine clearChoiceFlagCoroutine;


    public UIData UIData = new UIData();
    public PanelData panelData = new PanelData();
    public MenuData menuData = new MenuData();

    void Start()
    {
        // 슬라이더 범위 설정
        bgmSlider.minValue = 0.0001f;
        bgmSlider.maxValue = 1f;
        sfxSlider.minValue = 0.0001f;
        sfxSlider.maxValue = 1f;

        // 저장된 볼륨 불러오기 (없으면 기본값 1)
        bgmSlider.value = PlayerPrefs.GetFloat("BGM_Volume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX_Volume", 1f);

        // 슬라이더 값 변경 시 함수 자동 호출 등록
        bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);

        // 시작할 때 저장된 볼륨 바로 적용
        SoundManager.Instance.SetBGMVolume(bgmSlider.value);
        SoundManager.Instance.SetSFXVolume(sfxSlider.value);

        // 기본 상태: ButtonList만 보이고 SettingUI는 숨김
        if (buttonList != null) buttonList.SetActive(true);
        if (settingUI != null) settingUI.SetActive(false);
    }

    // ── 설정 페이지 전환 ───────────────────────────────────────

    // 설정 버튼에 연결
    public void OpenSettingPage()
    {
        StartCoroutine(OpenSettingRoutine());
    }

    private IEnumerator OpenSettingRoutine()
    {
        // 1. ButtonList 숨기기
        buttonList.SetActive(false);

        // 2. 페이지 넘김 애니메이션 재생
        if (pageTurnAnimator != null && !menuData.isSetting)
        {
            menuData.isSetting = true;
            pageTurnAnimator.SetTrigger("PageTurn");
            yield return null; // 애니메이션 시작될 때까지 한 프레임 대기
            float animLength = pageTurnAnimator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animLength);
        }

        // 3. ButtonList 다시 보이기 + SettingUI 보이기
        buttonList.SetActive(true);
        settingUI.SetActive(true);
    }

    private void CloseSetting()
    {
        // 1. SettingUI 숨기기
        settingUI.SetActive(false);
        menuData.isSetting = false;
    }

    // ── 슬라이더 콜백 ──────────────────────────────────────────

    private void OnBGMSliderChanged(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
        PlayerPrefs.SetFloat("BGM_Volume", value);
    }

    private void OnSFXSliderChanged(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFX_Volume", value);
    }

    // ── dialogue UI 함수 ────────────────────────────────────────────

    public void SetDialogueBoxActive(bool isActive)
    {
        UIData.dialogueBox.SetBool("isShow", isActive);
    }

    public void UpdatePotrait(CharacterData characterData, DialogueLine line)
    {
        if (characterData == null || characterData.potraits == null || line.potraitIdx < 0 || line.potraitIdx >= characterData.potraits.Length || characterData.potraits[line.potraitIdx] == null)
        {
            HidePotrait();
            return;
        }

        Sprite portraitSprite = characterData.potraits[line.potraitIdx];

        if (UIData.potraitObj != null)
        {
            UIData.potraitObj.SetActive(true);
        }

        if (UIData.potrait != null)
        {
            UIData.potrait.sprite = portraitSprite;
            UIData.potrait.color = Color.white;
        }

        currPotraitIdx = line.potraitIdx;

        if (prevPotraitIdx != currPotraitIdx)
        {
            if (UIData.potraitAnim != null)
            {
                UIData.potraitAnim.SetTrigger("doMove");
            }

            prevPotraitIdx = currPotraitIdx;
        }
    }
    private void HidePotrait()
    {
        if (UIData.potraitObj != null)
        {
            UIData.potraitObj.SetActive(false);
        }

        if (UIData.potrait != null)
        {
            UIData.potrait.sprite = null;
            UIData.potrait.color = new Color(1f, 1f, 1f, 0f);
        }

        currPotraitIdx = -1;
        prevPotraitIdx = -1;
    }

    public void UpdateDialogueUI(ObjectData objData, string nameData, DialogueLine line)
    {

        if (UIData.nameText != null)
        {
            UIData.nameText.text = nameData ?? string.Empty;

        }

        CharacterData characterData = objData != null ? objData.characterData : null;

        UpdatePotrait(characterData, line);

        TextAnim.Instance.SetText(line.sentence);

    }
    public void UpdateMonologueUI(string nameData, DialogueLine line)
    {
        UIData.nameText.text = nameData;
        TextAnim.Instance.SetText(line.sentence);
    }
    public void UpdateCutSceneDialogueUI(CharacterData characterData, string nameData, DialogueLine line)
    {

        if (UIData.nameText != null)
        {
            UIData.nameText.text = nameData ?? string.Empty;
        }

        UpdatePotrait(characterData, line);

        TextAnim.Instance.SetText(line.sentence);
    }

    // ── Pause UI on/off ────────────────────────────────────────────
    public void TogglePause()
    {
        panelData.isPause = !panelData.isPause;
        panelData.pausePanel.SetActive(panelData.isPause);
        CloseSetting();
    }

    // ── Quit UI on/off ────────────────────────────────────────────
    public void OpenQuit()
    {
        SetQuitPanelActive(true);
    }

    public void CancelQuit()
    {
        SetQuitPanelActive(false);
    }

    public void SetQuitPanelActive(bool isActive)
    {
        if (panelData.quitPanel != null)
        {
            panelData.quitPanel.SetActive(isActive);
        }
    }

    public void OpenCheckPanel()
    {
        panelData.checkPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(panelData.firstButton);

        Input.ResetInputAxes();
    }

    public void CloseCheckPanel()
    {
        panelData.checkPanel.SetActive(false);
        Input.ResetInputAxes();
    }
    // ── Choice UI on/off ────────────────────────────────────────────


    public void ShowChoices(DialogueLine line, Action<int, int> onNextLineSelected)
    {
        // 이전 선택지의 지연 종료 취소
        if (clearChoiceFlagCoroutine != null)
        {
            StopCoroutine(clearChoiceFlagCoroutine);
            clearChoiceFlagCoroutine = null;
        }

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        // 이전 버튼 초기화
        foreach (Button button in choiceButtons)
        {
            button.onClick.RemoveAllListeners();
            OutlineDeactive(button);
            button.gameObject.SetActive(false);
        }

        if (choicePanel != null)
            choicePanel.SetActive(false);

        panelData.isChoice = false;

        if (!line.hasChoices)
            return;

        if (choicePanel == null ||
            choiceButtonPrefab == null ||
            choiceButtonParent == null)
        {
            Debug.LogError("선택지 Panel, Button Prefab, Parent를 연결해주세요.", this);
            return;
        }

        // 부족한 개수만큼 생성하고 다음에도 재사용
        while (choiceButtons.Count < line.ChoiceCount)
        {
            Button button = Instantiate(choiceButtonPrefab, choiceButtonParent);
            button.gameObject.SetActive(false);
            choiceButtons.Add(button);
        }

        for (int i = 0; i < line.ChoiceCount; i++)
        {
            Button button = choiceButtons[i];
            DialogueChoice choice = line.choices[i];

            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>(true);

            if (buttonText != null)
                buttonText.text = choice.text;

            button.onClick.RemoveAllListeners();

            int selectedIndex = i;
            int nextLineIdx = choice.nextLineIdx;

            button.onClick.AddListener(() =>
            {
                onNextLineSelected?.Invoke(selectedIndex, nextLineIdx);
            });

            // 활성 선택지끼리 위아래 이동
            Navigation navigation = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnUp = i > 0 ? choiceButtons[i - 1] : null,
                selectOnDown = i < line.ChoiceCount - 1 ? choiceButtons[i + 1] : null
            };

            button.navigation = navigation;

            OutlineDeactive(button);
            button.gameObject.SetActive(true);
        }

        panelData.isChoice = true;
        choicePanel.SetActive(true);

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(choiceButtons[0].gameObject);
        }

        Input.ResetInputAxes();
    }

    public void HideChoices()
    {
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        foreach (Button button in choiceButtons)
        {
            button.onClick.RemoveAllListeners();
            OutlineDeactive(button);
            button.gameObject.SetActive(false);
        }

        if (choicePanel != null)
            choicePanel.SetActive(false);

        if (clearChoiceFlagCoroutine != null)
            StopCoroutine(clearChoiceFlagCoroutine);

        clearChoiceFlagCoroutine = StartCoroutine(ClearChoiceFlagNextFrame());
    }

    private void OutlineDeactive(Button button)
    {
        if (button == null)
            return;

        Transform outlineTransform = button.transform.Find("ChoiceOutline");

        if (outlineTransform == null)
            return;

        Image img = outlineTransform.GetComponent<Image>();

        if (img != null)
            img.color = new Color(1f, 1f, 1f, 0f);
    }

    private IEnumerator ClearChoiceFlagNextFrame()
    {
        // 같은 Space 입력이 다음 대사까지 진행시키는 것을 방지
        yield return null;

        panelData.isChoice = false;
        clearChoiceFlagCoroutine = null;
    }

    
}