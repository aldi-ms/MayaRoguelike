using System;
using System.Text;

namespace Maya
{
    public class SideBar
    {
        private const string HIT_POINTS_STRING = "HP: ";
        private const string GAME_TIME_STRING = "Current Time: ";
        private static int HPBarLength = (Globals.CONSOLE_WIDTH - Globals.GAME_FIELD_BOTTOM_RIGHT.X) - (HIT_POINTS_STRING.Length + 8);
        private string hitPointsLabel;
        private string name;
        private int width;
        private Unit unit;
        private Coordinate topRight = new Coordinate(Globals.GAME_FIELD_BOTTOM_RIGHT.X + 1, 0);
        private StringBuilder hitPointsBar = new StringBuilder(HPBarLength);
        private StringBuilder timeLabel = new StringBuilder();

        public SideBar(Unit unit)
        {
            this.unit = unit;
            this.width = Globals.CONSOLE_WIDTH - topRight.X;
            this.name = CutStringIfTooLong(unit.Name);

            ConsoleTools.WriteOnPosition(CutStringIfTooLong(GameEngine.MapName), XCoordToCenterString(GameEngine.MapName, true), topRight.Y, ConsoleColor.DarkGray);
            ConsoleTools.WriteOnPosition(name, XCoordToCenterString(name), topRight.Y + 2, ConsoleColor.Yellow);
            ConsoleTools.WriteOnPosition(HIT_POINTS_STRING, topRight.X, topRight.Y + 4, ConsoleColor.Cyan);
            ConsoleTools.WriteOnPosition(GAME_TIME_STRING, Globals.GAME_FIELD_BOTTOM_RIGHT.X + 2, Globals.CONSOLE_HEIGHT - 2, ConsoleColor.Cyan);

            Update(this.unit);
        }

        /// <summary>
        /// Update the Side Pane information for the unit.
        /// </summary>
        /// <param name="unit">The unit for which is displayed information.</param>
        public void Update(Unit unit)
        {
            ShowHPBar(unit);
            ShowGameTime(unit);
            ShowAttributes(unit);
        }

        /// <summary>
        /// Call on map change to update the map title in the Side Pane.
        /// </summary>
        public void MapChange()
        {
            string mapName = CutStringIfTooLong(GameEngine.MapName);
            ConsoleTools.WriteOnPosition(mapName, XCoordToCenterString(mapName), topRight.Y, ConsoleColor.DarkGray);
        }

        private void ShowHPBar(Unit unit)
        {
            hitPointsLabel = string.Format("{0}/{1} ", unit.Attributes.CurrentHealth, unit.Attributes.MaxHealth);
            ConsoleTools.WriteOnPosition(hitPointsLabel.ToString(), topRight.X + HIT_POINTS_STRING.Length, topRight.Y + 4, ConsoleColor.Cyan);

            int bars = (int)(unit.Attributes.CurrentHealth / ((double)unit.Attributes.MaxHealth / (double)HPBarLength));
            hitPointsBar.Append('\u2588', bars);
            hitPointsBar.Append('\u2591', HPBarLength - bars);

            if (bars < HPBarLength)
                hitPointsBar.Append(' ', Globals.CONSOLE_WIDTH - (topRight.X + HIT_POINTS_STRING.Length + hitPointsLabel.Length + hitPointsBar.Length));

            ConsoleColor color = ConsoleColor.Red;
            if (((double)unit.Attributes.CurrentHealth / (double)unit.Attributes.MaxHealth) > 0.9)
                color = ConsoleColor.DarkGreen;
            else
                if (((double)unit.Attributes.CurrentHealth / (double)unit.Attributes.MaxHealth) > 0.7)
                    color = ConsoleColor.Green;
            else
                    if (((double)unit.Attributes.CurrentHealth / (double)unit.Attributes.MaxHealth) > 0.5)
                    color = ConsoleColor.Yellow;
            else
                        if (((double)unit.Attributes.CurrentHealth / (double)unit.Attributes.MaxHealth) > 0.3)
                    color = ConsoleColor.DarkYellow;
            
            ConsoleTools.WriteOnPosition(hitPointsBar.ToString(), topRight.X + HIT_POINTS_STRING.Length + hitPointsLabel.Length, topRight.Y + 4, color);
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

        private decimal oldAttrPrint;
        private void ShowAttributes(Unit unit)
        {
            string del = new string(' ', width);

            int attributesPrint = 1;
            for (int i = 0; i < 6; i++)
                attributesPrint *= unit.Attributes[i] + 1;
            attributesPrint += unit.Experience.Level;

            int mid = topRight.X + ((Globals.CONSOLE_WIDTH - topRight.X) / 2);
            if (attributesPrint != this.oldAttrPrint)
            {
                //clear the rows for the new info print
                ConsoleTools.WriteOnPosition(del, topRight.X, topRight.Y + 10);
                ConsoleTools.WriteOnPosition(del, topRight.X, topRight.Y + 11);
                ConsoleTools.WriteOnPosition(del, topRight.X, topRight.Y + 12);

                //print the new attributes
                ConsoleTools.WriteOnPosition(string.Format("STR: {0}", unit.Attributes[0]), topRight.X, topRight.Y + 10, ConsoleColor.DarkGray);
                ConsoleTools.WriteOnPosition(string.Format("DEX: {0}", unit.Attributes[1]), mid, topRight.Y + 10, ConsoleColor.DarkGray);
                ConsoleTools.WriteOnPosition(string.Format("CON: {0}", unit.Attributes[2]), topRight.X, topRight.Y + 11, ConsoleColor.DarkGray);
                ConsoleTools.WriteOnPosition(string.Format("WIS: {0}", unit.Attributes[3]), mid, topRight.Y + 11, ConsoleColor.DarkGray);
                ConsoleTools.WriteOnPosition(string.Format("SPI: {0}", unit.Attributes[4]), topRight.X, topRight.Y + 12, ConsoleColor.DarkGray);
                ConsoleTools.WriteOnPosition(string.Format("LUCK: {0}", unit.Attributes[5]), mid, topRight.Y + 12, ConsoleColor.DarkGray);
                this.oldAttrPrint = attributesPrint;
            }

            ConsoleTools.WriteOnPosition(del, topRight.X, topRight.Y + 14);
            ConsoleTools.WriteOnPosition(string.Format("LVL: {0}", unit.Experience.Level), topRight.X, topRight.Y + 14, ConsoleColor.White);
            ConsoleTools.WriteOnPosition(string.Format("EXP: {0} / {1}", unit.Experience.XP, unit.Experience.ExpPointsArray[unit.Experience.Level]),
                mid, topRight.Y + 14, ConsoleColor.White);
        }
        
        /// <summary>
        /// Check if the string length is larger than the Side Pane width.
        /// </summary>
        /// <param name="str">String to test.</param>
        /// <returns>Returns the whole string if str.Length is less width of the pane.</returns>
        private string CutStringIfTooLong(string str)
        {
            if (str.Length >= width)
                return this.unit.Name.Substring(0, width - 2);
            return str;
        }

        /// <summary>
        /// Find the 'x' coordinate to start writing a centered (in the Side Pane) string.
        /// </summary>
        /// <param name="str">The string you want to center.</param>
        /// <param name="checkLength">True if the string length is to be checked (to the Side Pane width).</param>
        /// <returns>'x' coordinate.</returns>
        private int XCoordToCenterString(string str, bool checkLength = false)
        {
            if (!checkLength)
                return topRight.X + (width / 2 - str.Length / 2);
            return topRight.X + (width / 2 - CutStringIfTooLong(str).Length / 2);
        }
    }
}
