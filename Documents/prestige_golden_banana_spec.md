# プレステージ・ゴールデンバナナ 仕様書

> 作成日: 2026-03-05
> 調査対象: `Assets/Banana Clicker Assets/Scripts/` 以下のコード
> ステータス: コード実装ベースの現状仕様（未実装箇所あり）

---

## 1. プレステージ（Prestige）

### 1.1 概要

プレステージはゲームを一定進行させた後にリセットし、永続的なボーナスを得る「周回強化」システム。
リセット後のプレイは倍率ボーナスにより加速する。

---

### 1.2 発動条件

| 条件 | 値 |
|------|----|
| 累積バナナ獲得数 | **1兆（1,000,000,000,000）以上** |

- 累積カウンター `bananaTrillionCount` がバナナ獲得のたびに積み上がり、1以上でプレステージボタンが有効化される。
- ボタン有効化タイミング: `StatsWindow` を開いたとき（`OpenStatsWindow()` 内）。

**関連コード:** `BC_Click.cs` L156–169

```csharp
public void OpenStatsWindow()
{
    var prestigebtn = GameObject.Find("PrestigeButton").GetComponent<Button>();
    if (bananaTrillionCount >= 1)
        prestigebtn.interactable = true;
    else
        prestigebtn.interactable = false;
}
```

---

### 1.3 付与量の計算式

プレステージ実行時に付与される量（プレステージLv / ゴールデンバナナ）は以下の計算式で決まる。

```
付与量 = Floor( bananaTrillionCount ^ (1/3) )
       = Floor( 兆バナナ数 の 立方根 )
```

| 兆バナナ数 | 付与量 |
|-----------|--------|
| 1兆       | 1      |
| 8兆       | 2      |
| 27兆      | 3      |
| 64兆      | 4      |
| 125兆     | 5      |

**関連コード:** `BC_Click.cs` L178–201 `GetCubeRoot()` + `BuyPrestige()`

---

### 1.4 プレステージ実行時の処理フロー

```
BuyPrestige() 呼び出し
    ↓
gb = GetCubeRoot(bananaTrillionCount)  // 付与量を計算
    ↓
gb >= 1 であれば実行
    ↓
PrestigeclearGame()                    // ゲーム状態をリセット
    ↓
PrestigeLevel += gb                    // プレステージLv加算（永続）
GoldenBananas += gb                    // ゴールデンバナナ加算（永続）
```

---

### 1.5 リセットされるもの・されないもの

**リセットされる（初期値に戻る）**

| データ | 初期値 |
|--------|--------|
| 所持バナナ数（`bananas`） | 0 |
| クリック倍率（`bananasPerClick`） | 1 |
| 端数バナナカウンター（`BananaCount`） | 0 |
| 兆バナナカウンター（`bananaTrillionCount`） | 0 |
| 全建物アイテムの購入数（`count`） | 0 |
| 全建物アイテムのコスト（`cost`） | `baseCost`（初期コスト）|
| 全建物アイテムのアンロック状態（`IsUnlocked`） | false |
| 全アップグレードの購入数・コスト・アンロック状態 | 初期値 |
| PlayerPrefs 全データ（`PlayerPrefs.DeleteAll()`） | — |

**リセットされない（永続）**

| データ | 説明 |
|--------|------|
| `PrestigeLevel` | プレステージの累積レベル |
| `GoldenBananas` | ゴールデンバナナの累積所持数 |

**関連コード:** `BC_bananaPerSec.cs` L328–369 `PrestigeclearGame()`

---

### 1.6 プレステージボーナスの効果

プレステージLvが1以上になると、以下のボーナスが恒常的に適用される。

#### クリックボーナス

```
クリック取得量 = bananasPerClick + (bananasPerClick × PrestigeLevel / 100)
              = bananasPerClick × (1 + PrestigeLevel / 100)
```

**関連コード:** `BC_Click.cs` L119–132 `Clicked()`

#### BPS（バナナ毎秒）ボーナス

```
実効BPS = 建物合計BPS + (建物合計BPS × PrestigeLevel / 100)
        = 建物合計BPS × (1 + PrestigeLevel / 100)
```

**関連コード:** `BC_bananaPerSec.cs` L154–163 `GetBananasPerSec()`

#### ボーナス倍率まとめ

| PrestigeLevel | クリック倍率 | BPS倍率 |
|--------------|------------|--------|
| 0            | ×1.00      | ×1.00  |
| 1            | ×1.01      | ×1.01  |
| 10           | ×1.10      | ×1.10  |
| 100          | ×2.00      | ×2.00  |

> プレステージLv上限: **なし**（コード上の上限設定なし）

---

### 1.7 プレステージ関連 UI

| UI要素 | 変数名 | 説明 |
|--------|--------|------|
| PrestigeButton | `GameObject.Find("PrestigeButton")` で取得 | プレステージ実行ボタン（条件未達時は非活性） |
| StatsWindow | `public GameObject StatsWindow` | プレステージ情報を含む統計ウィンドウ |
| プレステージLv表示 | `public Text StatsWindowPrestigeLevel` | StatsWindow内："Prestige Level: XX" |
| プレステージ表示（BPS横） | `public UnityEngine.UI.Text PrestigeDisplay` | メイン画面："Prestige: XX" |

---

### 1.8 データ保存

| キー | 型 | 保存場所 |
|------|----|---------|
| `"PrestigeLevel"` | double | PlayerPrefs（※ES3移行未対応） |

> **注意:** プロジェクト方針（CLAUDE.md）ではES3を使用するが、PrestigeLevelは現在PlayerPrefsで保存されている。

---

## 2. ゴールデンバナナ（Golden Banana）

### 2.1 概要

プレステージ実行時に得られる特殊通貨。将来的なアップグレードやショップでの使用を想定した設計だが、**現時点では消費・使用ロジックは未実装**。

---

### 2.2 入手方法

ゴールデンバナナはプレステージ実行時にのみ付与される。

```
付与量 = Floor( 兆バナナ数 ^ (1/3) )  // プレステージLvと同量
```

プレステージLvと常に同じ量が同時加算される。入手方法はプレステージのみ。

**関連コード:** `BC_Click.cs` L190–191

```csharp
PrestigeLevel += gb;
GoldenBananas += gb;   // プレステージLvと同量を付与
```

---

### 2.3 効果・用途

**現在の実装状態: 未実装**

コード上、ゴールデンバナナは加算・保存・表示のみ行われており、消費・使用するロジックは一切存在しない。

| 機能 | 実装状態 |
|------|---------|
| 獲得（プレステージ時） | 実装済み |
| 保存・読み込み | 実装済み（PlayerPrefs） |
| UI表示 | 実装済み |
| 消費（購入・強化等） | **未実装** |
| ゴールデンバナナ専用ショップ | **未実装** |
| 特別効果・ボーナス | **未実装** |

---

### 2.4 データ保存

| キー | 型 | 保存場所 |
|------|----|---------|
| `"GoldBananas"` | double | PlayerPrefs（※ES3移行未対応） |

保存形式: `SetDouble()` / `GetDouble()` 経由（doubleをstring "R"フォーマットで PlayerPrefs に格納）

**関連コード:** `BC_bananaPerSec.cs` L205, L384

---

### 2.5 ゴールデンバナナ関連 UI

| UI要素 | 変数名 | 表示テキスト例 |
|--------|--------|--------------|
| BPS横の表示 | `public UnityEngine.UI.Text GBananasDisplay` | `"Golden Bananas 1.23M"` |
| StatsWindow内表示 | `public Text StatsWindowGoldBananaCount` | `"Golden Bananas: 5"` |

---

## 3. 両システムの関係

### 3.1 設計上の位置づけ

| 項目 | プレステージLv | ゴールデンバナナ |
|------|--------------|----------------|
| 獲得タイミング | プレステージ実行時 | プレステージ実行時（同時） |
| 付与量 | 同一 | 同一 |
| 現在の役割 | ゲームボーナス倍率の付与 | 将来の専用通貨（未実装） |
| 保存 | PlayerPrefs `"PrestigeLevel"` | PlayerPrefs `"GoldBananas"` |

### 3.2 値の関係

現状の実装では、プレステージLvとゴールデンバナナは**常に同じ値になる**（初期値0から同量ずつ積み上がる）。

将来的にゴールデンバナナを消費するシステムが実装された場合、両者の値は分岐する設計になっている。

---

## 4. 未実装・課題

| 項目 | 内容 | 優先度 |
|------|------|--------|
| ゴールデンバナナの消費ロジック | ショップや特殊アップグレードへの適用 | 未定 |
| PrestigeLevel / GoldenBananas の ES3 移行 | PlayerPrefs → ES3（CLAUDE.md 方針準拠） | 高 |
| BuyPrestige() のUI接続確認 | シーンから PrestigeButton → BuyPrestige() の紐付き検証が必要 | 中 |
| プレステージLv上限の設定 | 現在無制限 | 低 |

---

## 5. 関連ファイル一覧

| ファイル | 役割 |
|---------|------|
| `Assets/Banana Clicker Assets/Scripts/BC_Click.cs` | プレステージ発動条件・BuyPrestige・クリックボーナス・GoldenBananas フィールド |
| `Assets/Banana Clicker Assets/Scripts/BC_bananaPerSec.cs` | PrestigeclearGame・BPSボーナス・セーブ/ロード |
| `Assets/SushiClicker/Scripts/LevelSaveDataManager.cs` | 建物・アップグレードのES3保存（Prestige/GBは未対応） |
