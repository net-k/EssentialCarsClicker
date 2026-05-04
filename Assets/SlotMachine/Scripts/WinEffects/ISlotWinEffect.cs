namespace SlotMachine.Scripts
{
    public interface ISlotWinEffect
    {
        /// <summary>
        /// このエフェクトが実行可能かどうかを判定します。
        /// </summary>
        /// <param name="symbol">揃った絵柄</param>
        /// <param name="score">獲得スコア</param>
        /// <returns>実行可能であれば true</returns>
        bool IsApplicable(SlotValue symbol, int score);

        /// <summary>
        /// エフェクトを実行します。
        /// </summary>
        /// <param name="symbol">揃った絵柄</param>
        /// <param name="score">獲得スコア</param>
        void Execute(SlotValue symbol, int score);
    }
}