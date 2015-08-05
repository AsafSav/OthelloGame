using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.OthelloLogic
{
    public class OthelloSlot
    {
        private readonly int m_Row;
        private readonly int m_Column;
        private eSlotType m_Type;

        // Constructor which initialize the specified slot to be empty and with a position in the table.
        public OthelloSlot(int i_Row, int i_Column) 
        {
            m_Row = i_Row;
            m_Column = i_Column;
            m_Type = eSlotType.Empty;
        }

        public int Row
        {
            get
            {
                return m_Row;
            }
        }

        public int Column
        {
            get 
            {
                return m_Column;
            }
        } 

        public eSlotType SlotType
        {
            get
            {
                return m_Type;
            }

            set
            {
                m_Type = value;
            }
        }
    }
}
