using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Basic example script a developer may create to use this tool
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private RuntimeDialogueGraph _runtimeGraph;
    
    [SerializeField] private GameObject _dialoguePanel;
    [SerializeField] private Image _speakerPortrait;
    [SerializeField] private TextMeshProUGUI _speakerNameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _choiceButtonPrefab;
    [SerializeField] private GameObject _choiceButtonScrollView;
    [SerializeField] private GameObject _choiceButtonContainer;
    
    private Dictionary<string, RuntimeDialogueNode> _nodeLookup = new();
    private RuntimeDialogueNode _currentDialogueNode;
    
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
    
    private void Start()
    {
        foreach (RuntimeDialogueNode node in _runtimeGraph.AllNodes)
        {
            _nodeLookup.Add(node.NodeID, node);
        }
        
        if (!string.IsNullOrEmpty(_runtimeGraph.EntryNodeID))
        {
            ShowNode(_runtimeGraph.EntryNodeID);
        }
        else
        {
            EndDialogue();
        }
    }
    
    private void ShowNode(string nodeID)
    {
        if (!_nodeLookup.TryGetValue(nodeID, out RuntimeDialogueNode value))
        {
            EndDialogue();
            return;
        }
        
        _currentDialogueNode = value;
        
        _dialoguePanel.SetActive(true);
        _speakerPortrait.sprite = _currentDialogueNode.SpeakerPortrait;
        _speakerNameText.SetText(_currentDialogueNode.SpeakerName);
        _dialogueText.SetText(_currentDialogueNode.DialogueText);
        
        foreach (Transform child in _choiceButtonContainer.transform)
        {
            Destroy(child.gameObject);
        }
        
        if (_currentDialogueNode.BranchesData.Count > 0)
        {
            _continueButton.SetActive(false);
            _choiceButtonScrollView.SetActive(true);
            foreach (BranchData branchData in _currentDialogueNode.BranchesData)
            {
                Button button = Instantiate(_choiceButtonPrefab, _choiceButtonContainer.transform).GetComponent<Button>();
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = branchData.BranchText;
                button.onClick.AddListener(() =>
                                           {
                                               if (!string.IsNullOrEmpty(branchData.NextNodeID)) ShowNode(branchData.NextNodeID);
                                               else EndDialogue();
                                           });
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
    }
}