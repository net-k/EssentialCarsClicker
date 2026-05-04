# Git Add, Commit, Push スキル

Git の `add` → `commit` → `push` を一度に実行するスキルです。  
Copilot author trailer を自動追加します。

## 使い方

```
git-sync "コミットメッセージ"
```

例：
```
git-sync "SoldOutView と Presenter を実装"
```

## 処理内容

1. `git add .` - すべてのファイルをステージング
2. `git commit -m "メッセージ"` - コミット（Copilot trailer を自動追加）
3. `git push` - リモートリポジトリにプッシュ

## 機能

- ✅ 自動的に Copilot author trailer を追加
- ✅ ステップごとのログ出力
- ✅ エラーで即座に停止（`set -e`）

## 実装例

```bash
#!/bin/bash
set -e

COMMIT_MESSAGE="${1:-default commit}"
PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/../.."

cd "$PROJECT_ROOT"

echo "🔄 Git Sync を開始します..."
git add .
echo "✅ ステージング完了"

COMMIT_WITH_TRAILER="${COMMIT_MESSAGE}

Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>"

git commit -m "$COMMIT_WITH_TRAILER"
echo "✅ コミット完了"

git push
echo "🚀 プッシュ完了"
```
