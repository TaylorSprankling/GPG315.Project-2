using System.Collections.Generic;
using DialogueGraphPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Example script a developer may create to use the dialogue graph
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueData _dialogueData;
    
    [SerializeField] private GameObject _languagePanel;
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private GameObject _restartPanel;
    
    [SerializeField] private Image _speakerPortrait;
    [SerializeField] private TextMeshProUGUI _speakerNameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _choiceButtonPrefab;
    [SerializeField] private GameObject _choiceButtonScrollView;
    [SerializeField] private GameObject _choiceButtonContainer;
    
    private Languages _chosenLanguage = Languages.English;
    private Dictionary<string, RuntimeDialogueNode> _nodeDictionary = new();
    private RuntimeDialogueNode _currentDialogueNode;
    
    public enum Languages
    {
        English = -1,
        Español,
    }
    
    public void ChooseLanguage(int languageIndex)
    {
        _chosenLanguage = (Languages)languageIndex;
        _languagePanel.SetActive(false);
        switch (_chosenLanguage)
        {
            default:
            case Languages.English:
                _continueButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue...";
                break;
            case Languages.Español:
                _continueButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continuar...";
                break;
        }
        StartDialogue();
    }
    
    public void Continue()
    {
        if (_currentDialogueNode == null) return;
        
        if (!string.IsNullOrEmpty(_currentDialogueNode.NextNodeID))
        {
            ShowNode(_currentDialogueNode.NextNodeID);
        }
        else
        {
            EndDialogue();
        }
    }
    
    public void Restart()
    {
        _restartPanel.SetActive(false);
        _languagePanel.SetActive(true);
    }
    
    private void StartDialogue()
    {
        foreach (RuntimeDialogueNode node in _dialogueData.AllNodes)
        {
            _nodeDictionary.Add(node.NodeID, node);
        }
        
        if (!string.IsNullOrEmpty(_dialogueData.EntryNodeID))
        {
            ShowNode(_dialogueData.EntryNodeID);
        }
        else
        {
            EndDialogue();
        }
    }
    
    private void ShowNode(string nodeID)
    {
        if (!_nodeDictionary.TryGetValue(nodeID, out RuntimeDialogueNode value))
        {
            EndDialogue();
            return;
        }
        
        _currentDialogueNode = value;
        
        _dialoguePanel.SetActive(true);
        _speakerPortrait.sprite = _currentDialogueNode.SpeakerPortrait;
        
        switch (_chosenLanguage)
        {
            default:
            case Languages.English:
                _speakerNameText.SetText(_currentDialogueNode.SpeakerName);
                _dialogueText.SetText(_currentDialogueNode.DialogueText);
                break;
            case Languages.Español:
                _speakerNameText.SetText(_currentDialogueNode.LocalizedName[(int)Languages.Español]);
                _dialogueText.SetText(_currentDialogueNode.LocalizedText[(int)Languages.Español]);
                break;
        }
        
        foreach (Transform child in _choiceButtonContainer.transform)
        {
            Destroy(child.gameObject);
        }
        
        if (_currentDialogueNode.Branches.Count > 0)
        {
            _continueButton.SetActive(false);
            _choiceButtonScrollView.SetActive(true);
            int i = 0;
            foreach (BranchData branchData in _currentDialogueNode.Branches)
            {
                i++;
                Button button = Instantiate(_choiceButtonPrefab, _choiceButtonContainer.transform).GetComponent<Button>();
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                
                switch (_chosenLanguage)
                {
                    default:
                    case Languages.English:
                        buttonText.text = $"<b>{i}.</b>" + branchData.BranchText;
                        break;
                    case Languages.Español:
                        buttonText.text = $"<b>{i}.</b>" + branchData.LocalizedText[(int)Languages.Español];
                        break;
                }
                
                if (!string.IsNullOrEmpty(branchData.NextNodeID))
                {
                    button.onClick.AddListener(() => ShowNode(branchData.NextNodeID));
                }
                else
                {
                    EndDialogue();
                }
            }
        }
        else
        {
            _continueButton.SetActive(true);
            _choiceButtonScrollView.SetActive(false);
        }
    }
    
    private void EndDialogue()
    {
        _dialoguePanel.SetActive(false);
        _currentDialogueNode = null;
        _continueButton.SetActive(false);
        _choiceButtonScrollView.SetActive(false);
        foreach (Transform child in _choiceButtonContainer.transform)
        {
            Destroy(child.gameObject);
        }
        _nodeDictionary.Clear();
        _restartPanel.SetActive(true);
    }
}