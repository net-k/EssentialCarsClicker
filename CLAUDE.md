# SushiClicker プロジェクト ガイド

## プロジェクト概要

Unity製ハイブリッドゲーム。寿司をモチーフにしたコインプッシャー（`SushiCoinPusher`）がメインコンテンツで、クリッカーゲーム機能（`SushiClicker`）が並行開発中。
バナナクリッカー（`Banana Clicker Assets`）の既存ロジックをベースにクリッカー部分を実装している。

- **エンジン:** Unity 6000系
- **言語:** C#
- **主要ライブラリ:** Zenject（DI）、ES3（Easy Save 3、データ保存）、UniRx（リアクティブ）、I2 Localization（多言語）、TextMesh Pro、SRDebugger（デバッグ）、CSVSerializer（マスターデータ）

---

## フォルダ構成

```
Assets/
├── SushiCoinPusher/          # コインプッシャーゲーム実装（メイン）
│   ├── Scripts/
│   │   ├── AppFramework/     # アプリ共通基盤
│   │   │   ├── Boot/         # 起動・初期化（BootInitializer）
│   │   │   ├── Constants/    # 定数定義（GameConstants）
│   │   │   ├── DI/           # シーン固有インストーラー
│   │   │   ├── DailyBonusSystem/ # デイリーボーナス
│   │   │   ├── MasterData/   # マスターデータ管理（AchievementMaster、StageMaster、MasterLoader）
│   │   │   ├── SaveData/     # データ永続化マネージャー群
│   │   │   └── UI/           # 共通UIコンポーネント（CommonDialog、Count）
│   │   ├── Features/         # 機能別スクリプト
│   │   │   ├── Achievement/  # 実績システム
│   │   │   ├── Collection/   # コレクション・ショップ
│   │   │   ├── GameMenu/     # ゲームメニューダイアログ
│   │   │   ├── Life/         # ライフ（体力）システム
│   │   │   ├── OutOfCoinsPopup/ # コイン切れポップアップ
│   │   │   ├── Prize/        # プライズ（報酬）管理
│   │   │   ├── Rewards/      # リワード広告定数
│   │   │   ├── Scene/        # シーン管理（SushiCatcherSceneManager）
│   │   │   ├── Slot/         # スロット機能
│   │   │   ├── SlotTrigger/  # スロットトリガー
│   │   │   ├── Stage/        # ステージ管理
│   │   │   ├── Title/        # タイトル画面
│   │   │   └── VFX/          # ビジュアルエフェクト
│   │   ├── DebugFeatures/    # デバッグ機能（SROptions）
│   │   └── Installers/       # DI設定（ProjectInstaller、CoinPusherSceneInstaller）
│   ├── Prefabs/
│   ├── Resources/
│   │   └── Master/           # CSVマスターデータ（achievement_master.csv、stage_master.csv）
│   ├── Scenes/
│   └── Textures/
├── SushiClicker/             # クリッカーゲーム機能（開発中）
│   ├── Scripts/              # MVP Presenter/View スクリプト群
│   │   ├── BuildingItemPresenter.cs / BuildingItemView.cs
│   │   ├── FooterPresenter.cs / FooterView.cs
│   │   ├── ItemDialogPresenter.cs / ItemDialogView.cs
│   │   ├── LevelGaugePresenter.cs / LevelGaugeView.cs
│   │   ├── LevelSaveDataManager.cs
│   │   ├── LevelUpDialogPresenter.cs / LevelUpDialogView.cs
│   │   ├── MenuDialogPresenter.cs / MenuDialogView.cs
│   │   ├── PlayerLevelManager.cs
│   │   ├── UpgradeDialogPresenter.cs / UpgradeDialogView.cs
│   │   ├── UpgradeItemPresenter.cs / UpgradeItemView.cs
│   │   └── VelocityPresenter.cs / VelocityView.cs
│   ├── Prefabs/
│   └── Textures/
├── Banana Clicker Assets/    # バナナクリッカー基盤（再利用中）
│   └── Scripts/
│       ├── BC_Click.cs       # バナナ数・クリック処理
│       ├── BC_ItemManager.cs # 建物アイテム管理
│       ├── BC_bananaPerSec.cs # BPS（バナナ毎秒）計算
│       ├── BC_upgradeManager.cs # アップグレード管理
│       └── BC_currencyConverter.cs # 通貨表示変換
├── CoinPusher/               # コインプッシャーのシーンアセット・スクリプト
│   └── Scripts/              # CoinManager、PrizeSpawner、Pusher 等
├── KumaFramework/            # 内製フレームワーク（UI基盤、Master基盤）
│   ├── Master/               # MasterBase<T>（CSV読み込み基盤）
│   ├── UI/
│   │   ├── PresenterBase.cs  # Show()/Hide() を持つPresenter基底クラス
│   │   └── Dialog/           # DialogPresenterBase（ダイアログチェーン対応）
│   ├── TextEffect/           # フローティングテキストエフェクト
│   └── BuildVersion/         # ビルドバージョン管理
├── ShisenSho/Scripts/Framework/
│   └── SingletonMonoBehaviour.cs  # シングルトン基底クラス（実体はここ）
├── Plugins/                  # サードパーティプラグイン
│   ├── UniRx/                # リアクティブ拡張
│   └── Zenject/              # DIコンテナ
├── StompyRobot/              # SRDebugger（デバッグUI）
└── CSVSerializer/            # CSVデシリアライザー
```

### 主要シーン

| シーン名 | 役割 | `SushiCaterScene` enum値 |
|---------|------|--------------------------|
| `Boot.unity` | 起動・初期化 | - |
| `TitleScene.unity` | タイトル画面 | `Title` |
| `BClickerScene.unity` | メインゲーム（クリッカー＋コインプッシャー） | `Game` |
| `Achievement.unity` | 実績画面 | `Achievement` |
| `Collection.unity` | コレクション・ショップ | `Collection` |
| `SupportScene.unity` | サポート画面 | `Support` |

---

## ネームスペース一覧

レガシーコードのリファクタリング途中のため、ネームスペースが複数存在する。

| ネームスペース | 用途 |
|--------------|------|
| `SushiClicker` | クリッカーゲーム機能（Presenter/View/Manager） |
| `SushiCatcher` | シーン管理、コインプッシャーゲームシーン |
| `SushiCoinPusher.Installers` | Zenject DIインストーラー |
| `Quiz.Infrastructure` | GameConstants（定数定義） |
| `Quiz.Framework.Life` | LifeManager |
| `App` | CoinSaveDataManager、LifeSaveDataManager |
| `TohoReversi.Master` | MasterBase<T>、MasterLoader（レガシー名） |
| `SushiCatcher.Master` | AchievementMaster、StageMaster |
| `SushiCatcher.SaveData` | AchievementSaveDataManager 等 |
| `KumaFramework` | UI基盤（PresenterBase、DialogPresenterBase） |
| （グローバル） | SingletonMonoBehaviour、BC_Click 等のレガシークラス |

---

## アーキテクチャ

### DI（依存性注入）: Zenject

コインプッシャー系のロジッククラスはZenjectによるDIで管理する。

```csharp
// Installers/ProjectInstaller.cs でシングルトン登録
namespace SushiCoinPusher.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AchievementManager>().AsSingle();
            Container.Bind<LifeManager>().AsSingle();
            Container.Bind<MasterLoader>().AsSingle();
            Container.Bind<AchievementMaster>().AsSingle();
            Container.Bind<StageMaster>().AsSingle();
            Container.Bind<DailyBonus>().AsSingle();
        }
    }
}

// 使用側は [Inject] でメソッドインジェクション
[Inject]
private void Construct(AchievementManager achievementManager)
{
    _achievementManager = achievementManager;
}
```

### UIパターン: MVP（Presenter/View）

ビジネスロジックと表示を分離する。全Presenterは `KumaFramework.PresenterBase` または `KumaFramework.DialogPresenterBase` を継承する。

```csharp
// PresenterBase: Show()/Hide() でGameObjectのアクティブ状態を制御
public class MenuDialogPresenter : PresenterBase
{
    [SerializeField] private MenuDialogView _view = null;

    private void Awake()
    {
        _view.OnCloseButtonClick
            .Subscribe(_ => Hide())
            .AddTo(this);
        Hide(); // 初期非表示
    }
}

// DialogPresenterBase: 前のダイアログを記憶してチェーン遷移できる
public class SomeDialogPresenter : DialogPresenterBase
{
    // ShowAndHidePrevious() / HideAndShowPrevious() で連鎖可能
}

// View: 表示のみ担当。ボタンはIObservableとして公開
public class MenuDialogView : MonoBehaviour
{
    [SerializeField] private Button _closeButton = null;

    public IObservable<Unit> OnCloseButtonClick => _closeButton.OnClickAsObservable();
}
```

### マネージャー: SingletonMonoBehaviour

`Assets/ShisenSho/Scripts/Framework/SingletonMonoBehaviour.cs` に定義されたシングルトン基底クラス。SaveDataManager群はこれを継承する。`FindObjectOfType` は内部実装で使われているが、呼び出し側は `Instance` プロパティ経由でアクセスする。

```csharp
public class CoinSaveDataManager : SingletonMonoBehaviour<CoinSaveDataManager>
{
    public int LoadCoin() { ... }
    public void SaveCoin(int coin) { ... }
}

// 呼び出し
CoinSaveDataManager.Instance.SaveCoin(100);
```

### データ保存: ES3（Easy Save 3）

新規コードではPlayerPrefsを使わない。全てES3で永続化する。
**注意:** `BC_ItemManager` 等のレガシースクリプトにはPlayerPrefsが残存しているが、新規コードではES3へ移行済みの `LevelSaveDataManager` を使う。

```csharp
// 保存
ES3.Save<int>(key, value);

// 読み込み（デフォルト値付き）
ES3.Load<int>(key, defaultValue);

// キー存在確認（初回起動時の初期化）
if (!ES3.KeyExists(key))
{
    ES3.Save<int>(key, GameConstants.InitialCoinNum);
}
```

### マスターデータ: MasterBase<T> + CSV

CSVファイルは `SushiCoinPusher/Resources/Master/` に配置し、`Resources.Load()` で読み込む。

```csharp
// KumaFramework/Master/MasterBase.cs（namespace: TohoReversi.Master）
public abstract class MasterBase<T>
{
    protected bool Load(string path) // Resources以下のパス
}

// 派生クラスの実装例
namespace SushiCatcher.Master
{
    public class AchievementMaster : MasterBase<AchievementData>
    {
        public override bool Load() => base.Load("Master/achievement_master");
        public AchievementData FindById(int id) { ... }
    }
}

// MasterLoaderで一括ロード（Zenject経由で注入）
public class MasterLoader
{
    MasterLoader(StageMaster stageMaster, AchievementMaster achievementMaster) { }
    public void Load() { /* 未ロードのマスターを全てロード */ }
}
```

### 非同期処理: UniRx + C# Event

ボタン押下などUIイベントはUniRxを使う。ゲームロジックのイベント通知はC# Eventを使う。

```csharp
// View側: ボタンはIObservableとして公開
public IObservable<Unit> OnBuildingButtonClick => _buildingButton.OnClickAsObservable();

// Presenter側: Subscribeで購読（.AddTo(this) でライフサイクル管理）
_view.OnBuildingButtonClick
    .Subscribe(_ => _itemDialogPresenter.Show())
    .AddTo(this);

// ゲームロジック: C# Event
public event Action<int> OnAchievementCleared;
OnAchievementCleared?.Invoke(achievementId);
```

### プレイヤーレベルシステム（SushiClicker）

累積バナナ数に基づいてレベルが上がる。しきい値は `10^(N+3)`（Lv1=1万、Lv2=10万…）。

```csharp
// PlayerLevelManager（namespace: SushiClicker）
// BC_Click.AddBananas() の末尾から呼び出す
PlayerLevelManager.Instance.CheckLevelUp(totalBananas);

// レベルアップ時にイベント発行
public event Action<int, int, double> OnLevelUp; // (fromLevel, toLevel, reward)
```

---

## コーディング規約

### 命名規則

| 対象 | 規則 | 例 |
|------|------|----|
| privateフィールド | アンダースコア + camelCase | `_lifeNum`, `_achievementManager` |
| SerializeFieldフィールド | privateに `[SerializeField]` を付ける | `[SerializeField] private Button _closeButton;` |
| publicプロパティ | PascalCase | `public int LifeNum { get; private set; }` |
| メソッド | PascalCase、動詞始まり | `LoadCoin()`, `SaveCoin()`, `IsMax()` |
| 定数 | PascalCase または static readonly | `InitialCoinNum`, `LifeMaxNum` |
| イベント | `On` 接頭辞 | `OnAchievementCleared`, `OnLevelUp` |
| クラス・インターフェース | PascalCase | `CoinSaveDataManager`, `ILocalizeTarget` |
| 列挙型 | PascalCase | `LifeType.Default` |
| データ保存キー | `RecordType_XXX` 形式でクラス内に閉じ込める | `RecordType_Coin`, `RecordType_Level` |

### フィールド宣言

```csharp
// SerializeField は必ず private に付ける（publicにしない）
[SerializeField]
private AchievementListDialogView _view = null;

// DI inject済みフィールドは private のみ
private AchievementManager _achievementManager;

// データ保存キー
private readonly string RecordType_Level = "PlayerLevel_Level";

// キーが複合の場合はメソッドで組み立てる
private string GetLifeKey(LifeType lifeType) => $"{RecordType_Life}_{lifeType}";
```

### アクセス修飾子

- フィールドは原則 `private`
- `public` にするのはAPIとして外部公開するプロパティやメソッドのみ
- `[SerializeField]` を使うことでInspectorから設定しつつ `private` を維持する

### コメントの書き方

#### XMLドキュメントコメント（公開メソッド・プロパティに付ける）

```csharp
/// <summary>
/// コインを消費する
/// </summary>
/// <param name="amount">消費するコイン数</param>
/// <returns>消費後のコイン数</returns>
public int ConsumeCoin(int amount) { ... }
```

#### インラインコメント（処理の意図が自明でない箇所）

```csharp
// コイン不足の場合は処理しない
if (currentCoin < amount) return -1;

// 1ライフ回復に必要な時間（秒）
private int _recoveryUnitSeconds = 60 * 60;
```

- コメントは**日本語**で書く
- 「何をするか」ではなく「なぜするか・背景」を書く
- 自明なコードにはコメントを付けない

#### デバッグログ

```csharp
Debug.Log($"Achievement Cleared: {data.title}");
Debug.LogWarning("Failed to load resource at path: " + path);
Debug.LogError("Not enough coins");
```

- 文字列補間（`$"..."`）を使う
- 重大度に応じてLog / LogWarning / LogErrorを使い分ける

### イベント購読のライフサイクル（C# Event）

```csharp
private void Awake()
{
    // Awake で登録
    _achievementManager.OnAchievementCleared += OnAchievementCleared;
}

private void OnDestroy()
{
    // OnDestroy で必ず解除（メモリリーク防止）
    if (_achievementManager != null)
        _achievementManager.OnAchievementCleared -= OnAchievementCleared;
}
```

---

## ゲームシステム概要

### ライフシステム

- 初期値: 5ハート（`GameConstants.InitialHeartNum`）
- 最大値: 5ハート（`GameConstants.LifeMaxNum`）
- 回復時間: 1時間 / 1ハート
- `LifeManager`（Zenject管理）→ `LifeSaveDataManager`（ES3保存）
- `LifeSaveDataManager.LifeType.Default` が現在の唯一のライフ種別

### コインシステム

- 初期値: 100コイン（`GameConstants.InitialCoinNum`）
- `CoinSaveDataManager`（`namespace: App`）で一元管理
- コインとキャッシュ（`RecordType_Cash`）を分けて管理
- コイン切れ時は `OutOfCoinsPopup` でリワード広告を誘導

### プレイヤーレベルシステム（SushiClickerモジュール）

- `PlayerLevelManager`（`namespace: SushiClicker`、`SingletonMonoBehaviour`）
- レベルしきい値: `10^(N+3)`（Lv1=10,000、Lv2=100,000、…）
- レベルアップ報酬: BPS × 3600秒 × レベル差
- `LevelSaveDataManager` でレベル・建物所持数・コストをES3保存

### 実績システム

```
PrizeCollectionManager（プライズ獲得、OnPrizeCountChanged イベント）
    → AchievementManager（Initialize() で購読登録、Zenject管理）
    → AchievementSaveDataManager（進捗・クリア状態を ES3 保存）
    → 達成判定 → OnAchievementCleared イベント（int achievementId）
    → AchievementNotification（ポップアップキュー）
    → AchievementNotificationDialog（UI表示）
```

実績タイトルはI2 Localizationで動的生成（`key_Achievement_Title_Type_1` テンプレート）。
アンロック条件: `initial_unlock`フラグ、前実績クリア（`next_unlock_id`）、ステージクリア（`clear_unlock_achievement_id`）。

### シーン遷移

```csharp
// SushiCatcherSceneManager（static）経由で遷移する
// namespace: SushiCatcher
SushiCatcherSceneManager.Load(SushiCaterScene.Title);
SushiCatcherSceneManager.Load(SushiCaterScene.Collection);
```

### マスターデータ（CSV）

| ファイル | クラス | 主要フィールド |
|---------|--------|--------------|
| `achievement_master.csv` | `AchievementMaster` / `AchievementData` | id, title, target_id, goal_value, sort_order, initial_unlock, next_unlock_id |
| `stage_master.csv` | `StageMaster` | stage_no, prize_image, clear_unlock_achievement_id |

### デバッグ機能

- SRDebugger（`StompyRobot/SRDebugger`）: デバッグUI。`#if USE_SRDEBUGGER` フラグで制御
- `BootInitializer.InitializeApp()`: `RuntimeInitializeOnLoadMethod(BeforeSceneLoad)` で全シーン共通初期化
- `SROptions.Life.cs`: ライフのデバッグ操作オプション

```csharp
// #if UNITY_EDITOR ブロックでエディタ専用デバッグメソッドを追加できる
#if UNITY_EDITOR
public void DebugResetLevel()
{
    _currentLevel = 0;
    ES3.Save<int>(RecordType_Level, _currentLevel);
}
#endif
```

---

## 注意事項・禁止事項

- `public` フィールドを使わない → `[SerializeField] private` にする（`BC_ItemManager` 等レガシー除く）
- 新規コードで `FindObjectOfType` を使わない → Zenjectで注入するか `SingletonMonoBehaviour.Instance` を使う
- 新規コードでPlayerPrefsを使わない → ES3を使う（`BC_ItemManager` はレガシーで残存）
- `new` でMonoBehaviourを生成しない → Instantiateを使う
- UIイベントはUniRx（`OnClickAsObservable()`）を使う
- ゲームロジックのイベントはC# Event（`Action<T>`）を使う
- C# Eventの購読はOnDestroyで必ず解除する（UniRxは `.AddTo(this)` で自動解除）
- 新規ネームスペースは既存の命名パターンに従い、レガシー名（`Quiz.`, `TohoReversi.`）は踏襲しない

## 既知の技術的負債

- `BC_ItemManager` 内でPlayerPrefsが残存している（`LevelSaveDataManager` で順次移行中）
- ネームスペースがレガシー名（`Quiz.Infrastructure`, `TohoReversi.Master` 等）のままのクラスがある
- `SingletonMonoBehaviour` が `ShisenSho` フォルダ（別ゲームのフォルダ）に定義されている
- `AchievementManager` は `Initialize()` / `Uninitialize()` を手動で呼ぶ必要がある
