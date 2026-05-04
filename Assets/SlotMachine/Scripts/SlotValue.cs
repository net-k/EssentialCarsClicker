namespace SlotMachine
{
    public enum SlotValue { 
        none, 
        seven, // 777 フィーバー状態。壁が出現し、コイン10 枚が 20 数秒間発生しつつけ、画面もミラーボールみたいなエフェクトになる (旧 sword) 
        key, // コイン 10 枚払い出し    
        shield, // コイン 30 枚払い出し
        dia, // 赤いコイン(1枚あたり5コイン分)、10枚払い出し
        wall,  // wall 壁が出現、確率低 (旧名 box )
        prize, // コイン 100 枚払い出し (旧名 ring)
        coin  // コインタワー出現
    }
}