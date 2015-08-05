using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ex05.OthelloLogic;

namespace Ex05.OthelloWindowsUI
{
    public partial class OthelloMainGameForm : Form
    {
        private const int k_ButtonSize = 35;
        private const int k_FormMargin = 15;

        private static int s_Player1Victories = 0;
        private static int s_Player2Victories = 0;

        private readonly Color r_Player1_BackColor = Color.Black;
        private readonly Color r_Player1_TextColor = Color.White;
        private readonly eSlotType r_Player1_SlotType = eSlotType.Black;
        private readonly Color r_Player2_BackColor = Color.White;
        private readonly Color r_Player2_TextColor = Color.Black;
        private readonly eSlotType r_Player2_SlotType = eSlotType.White;
        private readonly string r_ClickedButtonText = "O";
        private readonly Color r_PossibleMove_BackColor = Color.LimeGreen;
        private readonly int r_BoardSize;
        private readonly bool r_VsHuman;

        private OthelloGameLogic m_MainOthelloLogic;
        private List<Button> m_AllButtons;

        // Constructor - the game form which runs a whole single game and computes the final score.
        public OthelloMainGameForm(bool i_VsHuman, int i_BoardSize)
        {
            r_BoardSize = i_BoardSize;
            r_VsHuman = i_VsHuman;
            m_MainOthelloLogic = new OthelloGameLogic(r_BoardSize, r_VsHuman);
            initMainOthelloForm();
            m_MainOthelloLogic.InitializeDefualtPoints();
            updateBoardButtons();
            this.ShowDialog();
        }

        ///// TODO - Consider changing this method to change only "suspicious" slots
        // Displaying the current status of the game.
        private void updateBoardButtons()
        {
            // List of all the buttons/slots which the user can click/play in his current turn. 
            List<OthelloSlot> buttonsToMark = m_MainOthelloLogic.PossibleOthelloMoves();
            
            // Assigning to each button his current state in the game.
            for (int i = 0; i < m_AllButtons.Count; i++)
            {
                Button button = m_AllButtons[i];
                OthelloSlot slotToCheck = m_MainOthelloLogic.GameBoard[i / r_BoardSize, i % r_BoardSize];
                if (slotToCheck.SlotType != eSlotType.Empty)
                {
                    button.Text = r_ClickedButtonText;
                }

                if (slotToCheck.SlotType == r_Player1_SlotType)
                {
                    button.BackColor = r_Player1_BackColor;
                    button.ForeColor = r_Player1_TextColor;
                }
                else if (slotToCheck.SlotType == r_Player2_SlotType)
                {
                    button.BackColor = r_Player2_BackColor;
                    button.ForeColor = r_Player2_TextColor;
                }
                else if (buttonsToMark.Contains(slotToCheck))
                {
                    button.BackColor = r_PossibleMove_BackColor;
                    button.Enabled = true;
                }
                else
                {
                    // A button that was not pressed yet by any player and not a possible move at the moment.
                    button.BackColor = default(Color);
                    button.Enabled = false;
                }
            }
        }

        // Initializes the game form.
        private void initMainOthelloForm()
        {
            m_AllButtons = new List<Button>();
            int numOfSlots = (int)Math.Pow(r_BoardSize, 2);
            for (int i = 0; i < numOfSlots; i++)
            {
                addButton(i);
            }

            this.Text = m_MainOthelloLogic.getActivePlayersName();
            this.Size = new Size(1, 1);
            this.AutoSize = true;
            this.Height += k_FormMargin;
            this.Width += k_FormMargin;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            changeDefaultButtonsStatus();  
        }

        // Creates the start point of the game (2 slots to each player are assigned).
        private void changeDefaultButtonsStatus()
        {
            int median = (r_BoardSize / 2) - 1;
            int leftTopIndex = (median * r_BoardSize) + 1;
            m_AllButtons[leftTopIndex].Click -= slotButton_Click;
            m_AllButtons[leftTopIndex + 1].Click -= slotButton_Click;
            m_AllButtons[leftTopIndex + r_BoardSize].Click -= slotButton_Click;
            m_AllButtons[leftTopIndex + r_BoardSize + 1].Click -= slotButton_Click;

            m_AllButtons[leftTopIndex].Enabled = true;
            m_AllButtons[leftTopIndex + 1].Enabled = true;
            m_AllButtons[leftTopIndex + r_BoardSize].Enabled = true;
            m_AllButtons[leftTopIndex + r_BoardSize + 1].Enabled = true;
        }

        // The button's event handler.
        private void slotButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            clickedButton.Text = r_ClickedButtonText;
            clickedButton.Click -= slotButton_Click;
            int buttonIndex = m_AllButtons.IndexOf(clickedButton);
            m_MainOthelloLogic.DoStep(m_MainOthelloLogic.GameBoard[buttonIndex / r_BoardSize, buttonIndex % r_BoardSize]);
            updateBoardButtons();
            this.Text = m_MainOthelloLogic.getActivePlayersName();
            if (!m_MainOthelloLogic.IsGameActive)
            {
                checkWinner();
                currentGameSummary();
            }
        }

        // check each game who won and increase his total wins.
        private void checkWinner()
        {
            // list of 2 numbers - the amount of coins of each player in the end of a game.
            List<int> scores = m_MainOthelloLogic.GetScore();
            if (scores[0] > scores[1])
            {
                s_Player1Victories++;
            }
            else if (scores[0] < scores[1])
            {
                s_Player2Victories++;
            }
        }

        // Displaying a summary message of the competition and an option of playing another round.
        private void currentGameSummary()
        {
            List<int> scores = m_MainOthelloLogic.GetScore();
            string summary = string.Empty;
            if (scores[0] != scores[1])
            {
                summary = string.Format(
                        "{0} Won!! ({1}/{2}) ({3}/{4}) {5}Would you like another round?",
                        m_MainOthelloLogic.GetWinner(),
                        scores[0],
                        scores[1],
                        s_Player1Victories,
                        s_Player2Victories,
                        Environment.NewLine);
            }
            else
            {
                // In case of a tie in the current game.
                summary = string.Format(
                        "Draw!! ({0}/{1}) ({2}/{3}) {4}Would you like another round?",
                        scores[0],
                        scores[1],
                        s_Player1Victories,
                        s_Player2Victories,
                        Environment.NewLine);
            }

            DialogResult messageResult = MessageBox.Show(summary, "Othello", MessageBoxButtons.YesNo);
            if (messageResult == DialogResult.Yes)
            {
                this.Hide();
                OthelloMainGameForm newGame = new OthelloMainGameForm(r_VsHuman, r_BoardSize);
            }

            System.Environment.Exit(0);
        }

        // Adds buttons to the form according to the Othello game format.
        private void addButton(int i_Index)
        {
            m_AllButtons.Add(new Button());
            m_AllButtons[i_Index].Size = new Size(k_ButtonSize, k_ButtonSize);
            m_AllButtons[i_Index].Enabled = false;
            m_AllButtons[i_Index].FlatStyle = FlatStyle.Popup;
            if (i_Index % r_BoardSize == 0)
            {
                m_AllButtons[i_Index].Left = m_AllButtons[0].Left;
                if (i_Index == 0)
                {
                    m_AllButtons[0].Top = this.Top + k_FormMargin;
                    m_AllButtons[0].Left = this.Left + k_FormMargin;
                }
                else
                {
                    m_AllButtons[i_Index].Top = m_AllButtons[i_Index - 1].Bottom + 1;
                }
            }
            else
            {
                m_AllButtons[i_Index].Left = m_AllButtons[i_Index - 1].Right + 1;
                m_AllButtons[i_Index].Top = m_AllButtons[i_Index - 1].Top;
            }

            this.Controls.Add(m_AllButtons[i_Index]);
            m_AllButtons[i_Index].Click += new EventHandler(slotButton_Click);
        }
    }
}
