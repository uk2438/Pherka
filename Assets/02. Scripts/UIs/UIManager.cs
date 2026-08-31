using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DialogueSystem;
using System;
using TMPro;
using UnityEngine.TextCore.Text;

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
    [SerializeField] private Button choiceButton1;
    [SerializeField] private Button choiceButton2;
    [SerializeField] private TMP_Text choiceButton1Text;
    [SerializeField] private TMP_Text choiceButton2Text;
    [SerializeField] private GameObject choicePanel;


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

        if (!line.hasChoices)
        {
            TextAnim.Instance.SetText(line.sentence);
        }

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

        if (!line.hasChoices)
        {
            TextAnim.Instance.SetText(line.sentence);
        }
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

        if (choicePanel != null) choicePanel.SetActive(false);

        panelData.isChoice = true;

        if (choicePanel != null) choicePanel.SetActive(true);

        choiceButton1Text.text = line.choice1Text;
        choiceButton1.onClick.RemoveAllListeners();
        int next1 = line.choice1NextLineIdx;
        choiceButton1.onClick.AddListener(() => onNextLineSelected(0, next1));

        choiceButton2Text.text = line.choice2Text;
        choiceButton2.onClick.RemoveAllListeners();
        int next2 = line.choice2NextLineIdx;
        choiceButton2.onClick.AddListener(() => onNextLineSelected(1, next2));

        // 키보드 조작을 위해 첫 번째 버튼을 선택 상태로 설정
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(choiceButton1.gameObject);

        Input.ResetInputAxes(); // CheckPanel에서 쓰신 것과 동일한 목적 (방향키 입력 잔상 제거)
    }

    public void HideChoices()
    {
        EventSystem.current.SetSelectedGameObject(null);

        OutlineDeactive(choiceButton1);
        OutlineDeactive(choiceButton2);

        if (choicePanel != null) choicePanel.SetActive(false);

        StartCoroutine(ClearChoiceFlagNextFrame());
    }

    private void OutlineDeactive(Button button)
    {
        Transform outlineTransform = button.transform.Find("ChoiceOutline");

        if (outlineTransform != null)
        {
            GameObject choiceOutline = outlineTransform.gameObject;

            Image img = choiceOutline.GetComponent<Image>();
            img.color = new Color(1, 1, 1, 0);
        }
    }

    private IEnumerator ClearChoiceFlagNextFrame()
    {//프레임 대기를 하지 않으면 PlayerController에서 Update함수를 바로 통과되기 때문에 space가 두번 눌린 판정이 되어 TextAnim가 정상적으로 실행이 되지 않음
        yield return null; // 한 프레임 대기
        panelData.isChoice = false; // 다음 프레임에 가서야 끔
    }
}