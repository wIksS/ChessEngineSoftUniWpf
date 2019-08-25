namespace ChessEngine.Data
{
    using ChessEngine.Data.Common;

    //Class that represents colors of the players
    public class Side
    {

        //Initialize a side with  given type
        public Side(SideType typeSide)
        {
            this.SideType = typeSide;
        }

        //Get and set the type side
        public SideType SideType { get; set; }

        //Return true if the side is white and false if the side is black
        public bool IsWhite()
        {
            return this.SideType == SideType.White;
        }

        //Return the color of the enemy`
        public SideType GetEnemySideType()
        {
            if (this.SideType == SideType.White)
            {
                return SideType.Black;
            }
            return SideType.White;
        }
    }
}
