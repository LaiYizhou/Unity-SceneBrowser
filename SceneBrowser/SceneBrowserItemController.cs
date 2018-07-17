using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneBrowserItemController : MonoBehaviour
{
    private static readonly Color _sceneColor = new Color(127 / 255f, 230 / 255f, 130 / 255f);
    private static readonly Color _childColor = new Color(125 / 255f, 193 / 255f, 240 / 255f);

    [SerializeField] private Image _bg;
    [SerializeField] private Text _name;
    [SerializeField] private Button _toggleBtn;
    [SerializeField] private Text _toggleBtnText;
    [SerializeField] private Button _expandBtn;
    [SerializeField] private Text _expandBtnText;

    private SceneBrowserItemController _prefab;
    private GameObject _targetGameObject;
    private int _depth;
    private List<SceneBrowserItemController> _children;

    // Use this for initialization
    private void Awake()
    {

        _toggleBtn.onClick.AddListener(Toggle);
        _expandBtn.onClick.AddListener(Expand);
        _children = new List<SceneBrowserItemController>();

    }

    public void SetTarget(string displayName, GameObject target = null, int depth = 0, SceneBrowserItemController prefab = null)
    {
        if (target == null)
        {
            _bg.color = _sceneColor;
            _toggleBtn.gameObject.SetActive(false);
            _expandBtn.gameObject.SetActive(false);
        }
        else
        {
            _bg.color = _childColor;
            _toggleBtn.gameObject.SetActive(true);
            _toggleBtnText.text = target.activeSelf ? "On" : "Off";
            if (target.transform.childCount > 0)
            {
                _expandBtn.gameObject.SetActive(true);
                _expandBtnText.text = _children.Count > 0 ? "Collapse" : "Expand";
            }
            else
            {
                _expandBtn.gameObject.SetActive(false);
            }
        }
        _targetGameObject = target;
        _name.text = displayName;
        _prefab = prefab;
        _depth = depth;
    }

    private void Toggle()
    {
        if (_targetGameObject == null)
        {
            gameObject.SetActive(false);
            return;
        }

        _targetGameObject.SetActive(!_targetGameObject.activeSelf);
        _toggleBtnText.text = _targetGameObject.activeSelf ? "On" : "Off";
    }

    private void Expand()
    {
        if (_children.Count > 0)
        {
            foreach (var child in _children.ToArray())
            {
                Destroy(child.gameObject);
                _children.Remove(child);
            }
        }
        else
        {
            if (_targetGameObject == null) return;
            var siblingIndex = transform.GetSiblingIndex();
            foreach (Transform targetChild in _targetGameObject.transform)
            {
                siblingIndex++;
                var browserItem = CreateBrowserItem(siblingIndex);
                _children.Add(browserItem);
                browserItem.SetTarget(string.Format("{0}{1}{2}", new string(' ', (_depth + 1) * 2), "├", targetChild.name), targetChild.gameObject, _depth + 1, _prefab);
            }
        }

        _expandBtnText.text = _children.Count > 0 ? "Collapse" : "Expand";
    }

    private SceneBrowserItemController CreateBrowserItem(int siblingIndex)
    {
        var itemController = Instantiate(_prefab);
        itemController.transform.SetParent(transform.parent);
        itemController.transform.SetSiblingIndex(siblingIndex);
        itemController.transform.localScale = Vector3.one;
        ;
        return itemController;
    }
}
