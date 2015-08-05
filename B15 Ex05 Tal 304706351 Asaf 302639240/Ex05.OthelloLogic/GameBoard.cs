using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.OthelloLogic
{
    public class GameBoard
    {
        private readonly int m_BoardSize;
        private OthelloSlot[,] m_BoardMatrix;

        // Constructor - creates a table in the specified size and initializes it to be empty OthelloSlots.
        public GameBoard(int i_BoardSize)
        {
            m_BoardSize = i_BoardSize;
            m_BoardMatrix = new OthelloSlot[i_BoardSize, i_BoardSize];
            for (int i = 0; i < i_BoardSize; i++)
            {
                for (int j = 0; j < i_BoardSize; j++)
                {
                    m_BoardMatrix[i, j] = new OthelloSlot(i, j);
                }
            }
        }

        public int Size
        {
            get
            {
                return m_BoardSize;
            }
        }

        public OthelloSlot this[int i_Row, int i_Column]
        {
            get
            {
                return m_BoardMatrix[i_Row, i_Column];
            }
        }
    }
}
