using gs.chef.onnectnext.timer;

namespace gs.chef.game.timer
{
    public class LevelTimer : AbsChefTimer
    {
        public static string TimerName => "LevelTimer";

        public LevelTimer(TimerModel timerModel) : base(timerModel)
        {
            
        }
    }
}