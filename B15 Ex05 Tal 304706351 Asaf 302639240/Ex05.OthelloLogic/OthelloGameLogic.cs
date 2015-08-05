using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.OthelloLogic
{
    public class OthelloGameLogic
    {
        private readonly eSlotType r_Player1SlotType = eSlotType.Black;
        private readonly eSlotType r_Player2SlotType = eSlotType.White;

        private List<OthelloSlot> m_AllEmptySlots;
        private GameBoard m_GameBoard;
        private int m_Player1NumOfCoins, m_Player2NumOfCoins;
        private Player m_Player1;
        private Player m_Player2;
        private bool m_IsGameActive;
        private Player m_ActivePlayer;

        public OthelloGameLogic(int i_BoardSize, bool i_VsHuman)
        {
            m_GameBoard = new GameBoard(i_BoardSize);
            m_AllEmptySlots = new List<OthelloSlot>();
            m_Player1 = new Player("Black", r_Player1SlotType, true);
            m_Player2 = new Player("White", r_Player2SlotType, i_VsHuman);
            m_ActivePlayer = m_Player1;
            m_IsGameActive = true;
        }

        public GameBoard GameBoard
        {
            get
            {
                return m_GameBoard;
            }
        }

        public bool IsGameActive
        {
            get
            {
                return m_IsGameActive;
            }
        }

        // The 'Heart' of the game. If possible - set the current slot with the given players' type.
        public void DoStep(OthelloSlot i_SuspiciousSlot)
        {
            if (m_ActivePlayer.SlotType == r_Player1SlotType)
            {
                m_Player1NumOfCoins++;
            }
            else
            {
                m_Player2NumOfCoins++;
            }

            m_GameBoard[i_SuspiciousSlot.Row, i_SuspiciousSlot.Column].SlotType = m_ActivePlayer.SlotType;
            m_AllEmptySlots.Remove(i_SuspiciousSlot);
            updateBoard(i_SuspiciousSlot, true);

            changeActivePlayer();
            checkActivePlayer(PossibleOthelloMoves());
            if (!m_ActivePlayer.IsHuman && m_IsGameActive)
            {
                DoStep(getSlotFromComputer(PossibleOthelloMoves()));
            }
        }

        public void InitializeDefualtPoints()
        {
            initializeEmptySlots();
            int median = (m_GameBoard.Size / 2) - 1;
            m_GameBoard[median, median].SlotType = r_Player2SlotType;
            m_GameBoard[median, median + 1].SlotType = r_Player1SlotType;
            m_GameBoard[median + 1, median].SlotType = r_Player1SlotType;
            m_GameBoard[median + 1, median + 1].SlotType = r_Player2SlotType;
            m_AllEmptySlots.Remove(m_GameBoard[median, median]);
            m_AllEmptySlots.Remove(m_GameBoard[median, median + 1]);
            m_AllEmptySlots.Remove(m_GameBoard[median + 1, median]);
            m_AllEmptySlots.Remove(m_GameBoard[median + 1, median + 1]);
        }

        public List<OthelloSlot> PossibleOthelloMoves()
        {
            List<OthelloSlot> possibleMoves = new List<OthelloSlot>();
            foreach (OthelloSlot slot in m_AllEmptySlots)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if ((i != 0) || (j != 0))
                        {
                            if (movePerDirection(slot, m_ActivePlayer.SlotType, i, j, false))
                            {
                                possibleMoves.Add(slot);
                            }
                        }
                    }
                }
            }

            return possibleMoves;
        }

        public List<int> GetScore()
        {
            string toReturn = string.Empty;
            List<int> scores = new List<int>();
            scores.Add(getNumOfCoins(m_Player1));
            scores.Add(getNumOfCoins(m_Player2));
            return scores;
        }

        public string getActivePlayersName()
        {
            return string.Format("Othello - {0}'s Turn", m_ActivePlayer.Name);
        }

        // This method return the winner's name.
        public string GetWinner()
        {
            string toReturn = string.Empty;
            if (m_Player1NumOfCoins > m_Player2NumOfCoins)
            {
                toReturn = m_Player1.Name;
            }
            else if (m_Player1NumOfCoins < m_Player2NumOfCoins)
            {
                toReturn = m_Player2.Name;
            }

            return toReturn;
        }

        private int getNumOfCoins(Player i_PlayerToCheck)
        {
            return (i_PlayerToCheck == m_Player1) ? m_Player1NumOfCoins : m_Player2NumOfCoins;
        }

        // Checks each possible move that the computer has and chooses the option that will "flip" the most coins.
        private OthelloSlot getSlotFromComputer(List<OthelloSlot> i_PossibleMoves)
        {
            OthelloSlot toReturn = null;
            int countComputerSlots = 0;
            int maxChange = 0;
            foreach (OthelloSlot slotToCheck in i_PossibleMoves)
            {
                countComputerSlots = updateBoard(slotToCheck, false);
                if (countComputerSlots > maxChange)
                {
                    maxChange = countComputerSlots;
                    toReturn = slotToCheck;
                }
            }

            return toReturn;
        }

        private void initializeEmptySlots()
        {
            m_Player1NumOfCoins = 2;
            m_Player2NumOfCoins = 2;
            m_AllEmptySlots = new List<OthelloSlot>();
            for (int i = 0; i < m_GameBoard.Size; i++)
            {
                for (int j = 0; j < m_GameBoard.Size; j++)
                {
                    if (m_GameBoard[i, j].SlotType == eSlotType.Empty)
                    {
                        m_AllEmptySlots.Add(m_GameBoard[i, j]);
                    }
                }
            }
        }

        // this private method helps us check if the current direction is making the slot a possible move.
        private bool movePerDirection(
            OthelloSlot i_Slot,
            eSlotType i_PlayerType,
            int i_RowMovement,
            int i_ColumnMovement,
            bool i_LastColorWasDifferent)
        {
            bool toReturn = true;
            int rowToCheck = i_Slot.Row + i_RowMovement;
            int columnToCheck = i_Slot.Column + i_ColumnMovement;

            if ((rowToCheck >= 0) && (rowToCheck < m_GameBoard.Size)
                && (columnToCheck >= 0) && (columnToCheck < m_GameBoard.Size))
            {
                OthelloSlot slotToCheck = m_GameBoard[rowToCheck, columnToCheck];
                if (slotToCheck.SlotType == eSlotType.Empty)
                {
                    toReturn = false;
                }
                else if (slotToCheck.SlotType == i_PlayerType)
                {
                    toReturn = i_LastColorWasDifferent;
                }
                else
                {
                    toReturn = movePerDirection(
                        m_GameBoard[rowToCheck, columnToCheck],
                        i_PlayerType,
                        i_RowMovement,
                        i_ColumnMovement,
                        true);
                }
            }
            else
            {
                toReturn = false;
            }

            return toReturn;
        }

        // This important board updates the coins colors after putting a coin on th board.
        private int updateBoard(OthelloSlot i_FromThisSlot, bool i_ToFlipCoins)
        {
            List<OthelloSlot> slotsToFlip = new List<OthelloSlot>();
            eSlotType originalType = i_ToFlipCoins ? i_FromThisSlot.SlotType : r_Player2SlotType;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((i != 0) || (j != 0))
                    {
                        changeSlotsPerDirection(i_FromThisSlot, originalType, i, j, false, ref slotsToFlip);
                    }
                }
            }

            if (i_ToFlipCoins)
            {
                foreach (OthelloSlot slotToFlip in slotsToFlip)
                {
                    slotToFlip.SlotType = originalType;
                    if (originalType == r_Player1SlotType)
                    {
                        m_Player1NumOfCoins++;
                        m_Player2NumOfCoins--;
                    }
                    else
                    {
                        m_Player1NumOfCoins--;
                        m_Player2NumOfCoins++;
                    }
                }
            }

            return slotsToFlip.Count;
        }

        // this method changes in times of need the color of the coins.
        private bool changeSlotsPerDirection(
            OthelloSlot i_FromThisSlot,
            eSlotType i_OriginalType,
            int i_RowMovement,
            int i_ColumnMovement,
            bool i_NeedToChangeSlotsType,
            ref List<OthelloSlot> i_SlotsToFlip)
        {
            bool toReturn = false;
            int rowToCheck = i_FromThisSlot.Row + i_RowMovement;
            int columnToCheck = i_FromThisSlot.Column + i_ColumnMovement;
            if ((rowToCheck >= 0) && (rowToCheck < m_GameBoard.Size)
                && (columnToCheck >= 0) && (columnToCheck < m_GameBoard.Size))
            {
                OthelloSlot slotToCheck = m_GameBoard[rowToCheck, columnToCheck];
                if (slotToCheck.SlotType == eSlotType.Empty)
                {
                    toReturn = false;
                }
                else if (slotToCheck.SlotType == i_OriginalType)
                {
                    if (i_NeedToChangeSlotsType)
                    {
                        toReturn = true;
                    }
                }
                else
                {
                    toReturn = changeSlotsPerDirection(
                        m_GameBoard[rowToCheck, columnToCheck],
                        i_OriginalType,
                        i_RowMovement,
                        i_ColumnMovement,
                        true,
                        ref i_SlotsToFlip);
                    if (toReturn)
                    {
                        i_SlotsToFlip.Add(m_GameBoard[rowToCheck, columnToCheck]);
                    }
                }
            }
            else
            {
                toReturn = false;
            }

            return toReturn;
        }

        // Checks who's the active player and if the game is over.
        private void checkActivePlayer(List<OthelloSlot> i_CurrentPossibleMoves)
        {
            if (i_CurrentPossibleMoves.Count == 0)
            {
                // Checks if the current player has moves to play.
                changeActivePlayer();

                // Gets the other player's possible moves.
                i_CurrentPossibleMoves = PossibleOthelloMoves();

                // Returns false if the other player also has no moves - the game is over.
                m_IsGameActive = i_CurrentPossibleMoves.Count > 0;
            }
        }

        private void changeActivePlayer()
        {
            if (m_ActivePlayer.Equals(m_Player1))
            {
                m_ActivePlayer = m_Player2;
            }
            else
            {
                m_ActivePlayer = m_Player1;
            }
        }
    }
}