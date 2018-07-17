using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class SceneBrowserController : MonoBehaviour
{
    [SerializeField] private SceneBrowserItemController _prefab;

    [SerializeField] private Transform _browserContent;
    [SerializeField] private Button _closeBtn;

    // Use this for initialization
    protected void Start()
    {

        _closeBtn.onClick.AddListener(OnClose);
        GetComponent<Canvas>().sortingOrder = 32767;

        OnView();

    }

    private void OnView()
    {
        for (int i = _browserContent.childCount - 1; i >= 0; i--)
        {
            Destroy(_browserContent.GetChild(i).gameObject);
        }

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);

            if (scene.name != "SceneBrowser" && scene.name != "Main")
            {
                var browserItem = CreateBrowserItem();
                browserItem.SetTarget(string.Format("[{0}]", scene.name));

                var rootGameObjects = scene.GetRootGameObjects().OrderBy(o => o.transform.GetSiblingIndex());
                foreach (var rootGo in rootGameObjects)
                {
                    var depth = 1;
                    browserItem = CreateBrowserItem();
                    browserItem.SetTarget(string.Format("{0}{1}{2}", new string(' ', depth * 2), "├", rootGo.name), rootGo, depth, _prefab);
                }

            }
        }

        //base.OnActivated();
    }

    private SceneBrowserItemController CreateBrowserItem()
    {
        var itemController = Instantiate(_prefab);
        itemController.transform.SetParent(_browserContent);
        itemController.transform.localScale = Vector3.one;
        return itemController;
    }

    private void OnClose()
    {

        this.gameObject.SetActive(false);
        SceneManager.UnloadSceneAsync("SceneBrowser");
        Resources.UnloadUnusedAssets();

        Input.ResetInputAxes();
    }
}
