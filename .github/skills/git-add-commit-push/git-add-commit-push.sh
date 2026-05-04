#!/bin/bash

# Git Add, Commit, Push スキル実装
# 使い方: git-sync "コミットメッセージ"

set -e

COMMIT_MESSAGE="${1:-default commit}"
PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)/../.."

cd "$PROJECT_ROOT"

echo "🔄 Git Sync を開始します..."
echo ""

# Step 1: git add
echo "📝 ステージング中..."
git add .
echo "✅ すべてのファイルをステージしました"
echo ""

# Step 2: git status
echo "📊 現在の状態:"
git status --short | head -10
echo ""

# Step 3: git commit with trailer
echo "💾 コミット中..."
COMMIT_WITH_TRAILER="${COMMIT_MESSAGE}

Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>"

git commit -m "$COMMIT_WITH_TRAILER"
echo "✅ コミットしました"
echo ""

# Step 4: git push
echo "🚀 プッシュ中..."
git push
echo "✅ プッシュしました"
echo ""

echo "🎉 完了！"
