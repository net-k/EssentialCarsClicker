using SushiCatcher.Achievement.AchievementList.AchievementListItemButton;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using System.Threading.Tasks;
using SushiCatcher;

namespace SushiCatcher.Achievement.AchievementListDialog
{
    public class AchievementListDialogPresenter : MonoBehaviour
    {
        [SerializeField]
        private AchievementListDialogView _view;
        
        [SerializeField]
        private GameObject _scrollViewContent = null;

        private AchievementManager _achievementManager;

        [Inject]
        void Construct(AchievementManager achievementManager)
        {
            _achievementManager = achievementManager;
        }
        
        // Start is called before the first frame update
        async void Start()
        {
            await CreateAchievementList();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        async Task CreateAchievementList()
        {
            string address = "Assets/SushiCoinPusher/Prefabs/AchievementListItemButton.prefab";
            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var prefab = handle.Result;
                foreach( var achievementData in _achievementManager.GetAchievementListUnlocked())
                {
                    CreateAchievementListItemButton(achievementData.id, achievementData.target_id, achievementData.goal_value, prefab);
                }
            }
            else
            {
                Debug.LogError($"Failed to load Addressable asset: {address}");
            }
        }
        
        void CreateAchievementListItemButton(int achievementId, int targetId, int goalValue, GameObject prefab)
        {
            var go = Instantiate(prefab, _scrollViewContent.transform);
            var presenter = go.GetComponent<AchievementListItemButtonPresenter>();
            
            string achievementTitle = _achievementManager.GetAchievementTitle(achievementId,targetId, goalValue);
            
            bool isCleared = _achievementManager.IsAchievementCleared(achievementId);
            int currentValue = AchievementSaveDataManager.Instance.LoadProgress(targetId);
            presenter.Initialize(achievementTitle, isCleared, achievementId, currentValue, goalValue );
        }
    }
}
