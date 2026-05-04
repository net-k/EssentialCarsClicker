# Unity用 Geminiプロジェクト設定

このドキュメントは、AIアシスタントがプロジェクトの構造、規約、ワークフローを理解するためのコンテキストを提供します。

## 1. プロジェクト概要

- **プロジェクト名:** CoinPusher
- **概要:** モバイルプラットフォーム向けの3Dコインプッシャーゲームです。
- **ターゲットプラットフォーム:** Android, iOS
- **Unityバージョン:** 2022.3.62f2
- **主要なアセット/フレームワーク:**
    - **Google Mobile Ads:** 広告表示用。
    - **I2 Localization:** 多言語対応用。
    - **KumaFramework:** プロジェクトで使用されているカスタムフレームワーク。
    - **ShisenSho, Stickin, TitleMatch:** プロジェクトに含まれる他のゲームモジュールまたはアセット。

## 2. プロジェクト構造

- **コアスクリプト:** `Assets/CoinPusher/Scripts/`
- **プレハブ:** `Assets/CoinPusher/Prefabs/`
- **シーン:** `Assets/CoinPusher/Scenes/`
- **アートアセット (モデル, テクスチャ):** `Assets/CoinPusher/Models/`, `Assets/CoinPusher/Images/`
- **エディタスクリプト:** `Assets/CoinPusher/Editor/`
- **広告関連スクリプト:** `Assets/CoinPusher/Scripts/Ad/` および `Assets/ShisenSho/Scripts/Framework/Ad/` のような他のアセットからの共有スクリプト。

## 3. コーディング規約

### 命名規則

- **クラス:** `PascalCase` (例: `CoinManager`)
- **メソッド:** `PascalCase` (例: `AddCoin`)
- **public/シリアライズ化されたフィールド:** `camelCase` (例: `videoRewardAmount`)
- **privateフィールド:** `camelCase` (例: `coinManager`)
- **インターフェース:** `IPascalCase` (例: `ICoinService`)

### スクリプティング実践

- **名前空間:** スクリプトを整理するために名前空間を使用します。特にフレームワークや明確な機能ごとに使用します (例: `namespace CoinPusher.UI`, `namespace Quiz.Framework.Ad.AdMob`)。
- **Header属性:** Inspectorのフィールドを整理するために `[Header("セクション名")]` を使用します。
- **Tooltip属性:** publicフィールドの目的をInspectorで説明するために `[Tooltip("説明")]` を使用します。
- **依存性の注入:** `CoinManager`や`AdManager`のような依存関係は、Inspector経由で割り当てられるか、`Awake()`または`Start()`で`GetComponent` / `FindObjectOfType`を使用して取得されることがよくあります。
- **プリプロセッサディレクティブ:** プラットフォーム固有のコードのために `#if UNITY_EDITOR` や `#if UNITY_IOS` のようなディレクティブに注意してください。

## 4. ワークフロー

### シーン管理

- 主要なゲームロジックは、シーン内に存在するマネージャーオブジェクト（例: `GameManager`, `CoinManager`, `OutOfCoinsManager`）から開始されることがよくあります。
- UI要素は、中央の`Canvas`の下で管理され、独自のマネージャースクリプトを持つことがあります。

### 広告統合

- 広告ロジックは `AdManager.cs` に集約されています。
- リワード広告は、シーン内に存在することが期待される `Quiz.Framework.Ad.AdMob.AdMobRewardVideo` によって処理されます。

### プロジェクトのビルド

- ビルドは通常、Unity Editor (`File > Build Settings...`) を通じて作成されます。
- 出力先ディレクトリは `Builds/Android` と `Builds/iOS` です。

## 5. AIアシスタント向けガイドライン

- 新しいスクリプトを作成する際は、`Assets/CoinPusher/Scripts/` 内の適切なサブフォルダに配置してください。
- 既存のスクリプトを修正する際は、確立されたコーディングスタイルと規約に従ってください。
- GameObjectに新しいコンポーネントを追加する場合、特に指示がない限り、Unity Editorで設定されるものと想定してください。コードから `AddComponent<T>()` を使用してコンポーネントを追加することもできます。
- シーンの変更が必要なタスク（例: スクリプトで簡単に見つけられない特定のGameObjectへのコンポーネントの追加）については、Unity Editorで実行する必要がある手順を説明してください。
- 異なるアセット（`CoinPusher`, `ShisenSho`など）間の相互依存関係に注意し、ユーザーの指示に従って、既存の共有スクリプトを適切に使用してください。