namespace stickin.tripletile
{
    public class ShuffleHint : Hint
    {
        public override bool Run(Game g)
        {
            var game = g as TripleTileGame;
            game.ShuffleBoard();
            
            return true;
        }
    }
}