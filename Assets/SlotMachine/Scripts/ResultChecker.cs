namespace SlotMachine.Scripts
{
    public static class ResultChecker
    {
        public static int Check(SlotValue[] reelValues)
        {
            if (reelValues.Length < 3)
            {
                return 0;
            }

            if (reelValues[0] != SlotValue.none && reelValues[0] == reelValues[1] && reelValues[0] == reelValues[2])
            {
                switch (reelValues[0])
                {
                    case SlotValue.wall:
                        return 100;
                    case SlotValue.coin:
                        return 200;
                    case SlotValue.dia:
                        return 300;
                    case SlotValue.key:
                        return 400;
                    case SlotValue.prize:
                        return 500;
                    case SlotValue.shield:
                        return 600;
                    case SlotValue.seven:
                        return 700;
                }
            }
            
            return 0;
        }
    }
}