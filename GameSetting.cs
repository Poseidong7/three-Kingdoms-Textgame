namespace TextRPG
{
    public static class GameSettings
    {
        //텍스트 속도 배율 (기본 : 1.0)
        //값이 작을수록 빠름 (0.5 = 2배 빠름, 2.0 = 2배 느림)
        public static double TextSpeedMultiplier = 1.0;
        
        // 사용자가 선택할 옵션
        public enum SpeedOption
        {
            Slow,   //느림
            Normal, //보통
            Fast    //빠름
        }

        
        //속도 설정 함수
        public static void SetTextSpeed(SpeedOption option)
        {
            switch (option)
            {
                case SpeedOption.Slow:
                    TextSpeedMultiplier = 1.5; //1.5배 느리게
                    break;
                case SpeedOption.Normal:
                    TextSpeedMultiplier = 1.0; //정속
                    break;
                case SpeedOption.Fast:
                    TextSpeedMultiplier = 0.5; //2배 빠르게
                    break;
            }
        }
    }
}