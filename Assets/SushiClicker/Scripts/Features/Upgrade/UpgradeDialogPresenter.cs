using KumaFramework;
using UniRx;
using UnityEngine;

namespace SushiClicker
{
    public class UpgradeDialogPresenter : PresenterBase
    {
        [SerializeField] private UpgradeDialogView _view = null;

        private void Awake()
        {
            _view.OnCloseButtonClick
                .Subscribe(_ => Hide())
                .AddTo(this);
        }

        // TODO: SoldOut 済みアイテムをリスト末尾に移動するソートは現在無効化中
        // private void OnEnable()
        // {
        //     // BC_upgradeManager.Start() で count が読み込まれるため、1フレーム待ってからソートする
        //     StartCoroutine(SortNextFrame());
        // }
        //
        // private IEnumerator SortNextFrame()
        // {
        //     yield return null;
        //     SortUpgradeItems();
        // }
        //
        // /// <summary>
        // /// SoldOut 済みのアイテムをリスト末尾に移動する
        // /// </summary>
        // private void SortUpgradeItems()
        // {
        //     var items = GetComponentsInChildren<UpgradeItemPresenter>(true);
        //     foreach (var item in items)
        //     {
        //         if (item.IsSoldOut)
        //         {
        //             // BC_upgradeManager が親の場合はその transform をリスト末尾に移動
        //             item.ListItemTransform.SetAsLastSibling();
        //         }
        //     }
        // }
    }
}
