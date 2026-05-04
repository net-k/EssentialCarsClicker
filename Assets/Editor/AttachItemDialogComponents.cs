using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using SushiClicker;

/// <summary>
/// BClickerScene の ItemDialog GameObject に
/// ItemDialogPresenter と ItemDialogView を一括でアタッチするエディタユーティリティ。
/// </summary>
public static class AttachItemDialogComponents
{
    [MenuItem("Tools/SushiClicker/Attach ItemDialog Components")]
    public static void Attach()
    {
        // BClickerScene を開く（保存確認あり）
        const string scenePath = "Assets/Banana Clicker Assets/_Scenes/BClickerScene.unity";
        var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        // シーン内の "ItemDialog" という名前の GameObject を検索
        var itemDialog = GameObject.Find("ItemDialog");
        if (itemDialog == null)
        {
            Debug.LogError("[AttachItemDialogComponents] 'ItemDialog' GameObject がシーン内に見つかりません。");
            return;
        }

        // ItemDialogView のアタッチ（既にあればスキップ）
        var view = itemDialog.GetComponent<ItemDialogView>();
        if (view == null)
        {
            view = itemDialog.AddComponent<ItemDialogView>();
            Debug.Log("[AttachItemDialogComponents] ItemDialogView をアタッチしました。");
        }
        else
        {
            Debug.Log("[AttachItemDialogComponents] ItemDialogView は既にアタッチ済みです。");
        }

        // ItemDialogPresenter のアタッチ（既にあればスキップ）
        var presenter = itemDialog.GetComponent<ItemDialogPresenter>();
        if (presenter == null)
        {
            presenter = itemDialog.AddComponent<ItemDialogPresenter>();
            Debug.Log("[AttachItemDialogComponents] ItemDialogPresenter をアタッチしました。");
        }
        else
        {
            Debug.Log("[AttachItemDialogComponents] ItemDialogPresenter は既にアタッチ済みです。");
        }

        // シーンをダーティにしてから保存
        EditorUtility.SetDirty(itemDialog);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("[AttachItemDialogComponents] 完了：BClickerScene を保存しました。");
    }
}
