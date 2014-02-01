using System;
using System.Text;

namespace WorldOfCSharp
{
    public class RightPane
    {
        private static int HPBarLength = (Globals.CONSOLE_WIDTH - Globals.GAME_FIELD_BOTTOM_RIGHT.X) - (HIT_POINTS_STRING.Length + 8);
        private Unit unit = new Unit(0,0,0,'\0', ConsoleColor.Red, "temp"); 
        private Coordinate topRight = new Coordinate(Globals.GAME_FIELD_BOTTOM_RIGHT.X + 1, 0);
        private StringBuilder hitPointsBar = new StringBuilder(HPBarLength);
        private const string HIT_POINTS_STRING = "Hit Points: ";
        private const string GAME_TIME_STRING = "Current Time: ";
        private StringBuilder timeLabel = new StringBuilder();
        private string hitPointsLabel;

        public RightPane(Unit unit)
        {
            this.unit = unit;

            ConsoleTools.WriteOnPosition(string.Format("{0}", this.unit.Name), topRight.X + (HPBarLength / 2 + (this.unit.Name.Length / 2)), topRight.Y, ConsoleColor.Yellow);
            ConsoleTools.WriteOnPosition(HIT_POINTS_STRING, topRight.X, topRight.Y + 2, ConsoleColor.Cyan);
            ConsoleTools.WriteOnPosition(GAME_TIME_STRING, Globals.GAME_FIELD_BOTTOM_RIGHT.X + 2, Globals.CONSOLE_HEIGHT - 2, ConsoleColor.Cyan);

            Update(this.unit);
        }

        public void Update(Unit unit)
        {
            ShowHPBar(unit);
            ShowGameTime(unit);
        }

        private void ShowHPBar(Unit unit)
        {
            hitPointsLabel = string.Format("{0}/{1} ", unit.Stats.CurrentHitPoints, unit.Stats.MaxHitPoints);
            ConsoleTools.WriteOnPosition(hitPointsLabel.ToString(), topRight.X + HIT_POINTS_STRING.Length, topRight.Y + 2, ConsoleColor.Cyan);

            int bars = (int)(unit.Stats.CurrentHitPoints / ((double)unit.Stats.MaxHitPoints / (double)HPBarLength));
            hitPointsBar.Append('\u2588', bars);
            if (bars < HPBarLength)
                hitPointsBar.Append(' ', Globals.CONSOLE_WIDTH - (topRight.X + HIT_POINTS_STRING.Length + hitPointsLabel.Length + hitPointsBar.Length));

            ConsoleColor color = ConsoleColor.Red;
            if (((double)unit.Stats.CurrentHitPoints / (double)unit.Stats.MaxHitPoints) > 0.9)
                color = ConsoleColor.DarkGreen;
            else
                if (((double)unit.Stats.CurrentHitPoints / (double)unit.Stats.MaxHitPoints) > 0.7)
                    color = ConsoleColor.Green;
            else
                if (((double)unit.Stats.CurrentHitPoints / (double)unit.Stats.MaxHitPoints) > 0.5)
                    color = ConsoleColor.Yellow;
            else
                if (((double)unit.Stats.CurrentHitPoints / (double)unit.Stats.MaxHitPoints) > 0.2)
                    color = ConsoleColor.DarkYellow;
            
            ConsoleTools.WriteOnPosition(hitPointsBar.ToString(), topRight.X + HIT_POINTS_STRING.Length + hitPointsLabel.Length, topRight.Y + 2, color);
            hitPointsBar.Clear();
        }

        private void ShowGameTime(Unit unit)
        {
            ConsoleTools.WriteOnPosition(string.Format("\t{0} year", GameEngine.GameTime.Year),
                Globals.GAME_FIELD_BOTTOM_RIGHT.X + GAME_TIME_STRING.Length + 2, Globals.CONSOLE_HEIGHT - 2);

            timeLabel.Append(' ', ((Globals.CONSOLE_WIDTH - Globals.GAME_FIELD_BOTTOM_RIGHT.X) - GameEngine.GameTime.ToString().Length) - 2);
            timeLabel.Append(GameEngine.GameTime.ToString());
            ConsoleTools.WriteOnPosition(timeLabel.ToString(), Globals.GAME_FIELD_BOTTOM_RIGHT.X + 1, Globals.CONSOLE_HEIGHT - 1);
            timeLabel.Clear();
        }
    }
}
