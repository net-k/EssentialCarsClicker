namespace stickin.tripletile
{
    public class BackHint : Hint
    {
        public override bool Run(Game g)
        {
            var game = g as TripleTileGame;
            return true;
        }
    }
}