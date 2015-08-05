using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.OthelloLogic
{
    public class Player
    {
        private readonly string m_Name;
        private readonly eSlotType m_MyColor;
        private readonly bool m_Human;
        
        // Constructor - initialize the player stats.
        public Player(string i_Name, eSlotType i_ColorType, bool i_Human)
        {
            m_Name = i_Name;
            m_MyColor = i_ColorType;
            m_Human = i_Human;
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public eSlotType SlotType
        {
            get
            {
                return m_MyColor;
            }
        }

        // Returns true if player1 chose to play against a real player, otherwise false.
        public bool IsHuman
        {
            get
            {
                return m_Human;
            }
        }
    }
}
