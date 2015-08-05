using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05.OthelloWindowsUI
{
    public class OthelloSettingsForm : Form
    {
        private const int k_MinBoardSize = 6;
        private Button m_IncreaseBoardSize;
        private Button m_VsComputer;
        private Button m_VsHuman;
        private OthelloMainGameForm m_OthelloMainForm;
        private int m_BoardSize;

        // CTOR - the first UI form which gets the size of the board and the type of contender to play against. 
        public OthelloSettingsForm()
        {
            initializeSettingsForm();
        }

        // This method initializes the settings form and also activates the initializtaion of its buttons
        private void initializeSettingsForm()
        {
            this.Text = "Othello - Game Settings";
            this.Size = new Size(450, 180);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            m_BoardSize = k_MinBoardSize;

            initializeSettingsButtons();
        }

        private void initializeSettingsButtons()
        {
            // Set the button that increases by 2 the board size by each press (max 12)
            string increaseBoardSizeText = string.Empty;
            m_IncreaseBoardSize = new Button();
            increaseBoardSizeText = getIncreaseButtonText();
            m_IncreaseBoardSize.Text = increaseBoardSizeText;
            m_IncreaseBoardSize.Location = new Point(this.Location.X + 12, this.Location.Y + 20);
            m_IncreaseBoardSize.Size = new Size(this.Size.Width - 40, this.Size.Height / 4);
            this.Controls.Add(m_IncreaseBoardSize);
            m_IncreaseBoardSize.Click += new EventHandler(m_IncreaseBoardSize_Click);

            // Set the button which will enable the one player mode + event handler
            m_VsComputer = new Button();
            m_VsComputer.Text = "Play against the computer";
            m_VsComputer.Location = new Point(m_IncreaseBoardSize.Location.X, m_IncreaseBoardSize.Bottom + 20);
            m_VsComputer.Size = new Size((m_IncreaseBoardSize.Size.Width / 2) - 10, m_IncreaseBoardSize.Size.Height);
            this.Controls.Add(m_VsComputer);
            m_VsComputer.Click += new EventHandler(m_VsComputer_Click);

            // Set the button which will enable the two player mode + event handler
            m_VsHuman = new Button();
            m_VsHuman.Text = "Play against your friend";
            m_VsHuman.Location = new Point(m_VsComputer.Right + 20, m_VsComputer.Location.Y);
            m_VsHuman.Size = m_VsComputer.Size;
            this.Controls.Add(m_VsHuman);
            m_VsHuman.Click += new EventHandler(m_VsHuman_Click);
        }

        // Handles button event - the user wants to play against a computer.
        private void m_VsComputer_Click(object sender, EventArgs e)
        {
            this.Hide();
            m_OthelloMainForm = new OthelloMainGameForm(false, m_BoardSize);
        }

        // Handles button event - the user wants to play against a friend.
        private void m_VsHuman_Click(object sender, EventArgs e)
        {
            this.Hide();
            m_OthelloMainForm = new OthelloMainGameForm(true, m_BoardSize);
        }

        // Handles button event - increases the size of the board by 2.
        private void m_IncreaseBoardSize_Click(object sender, EventArgs e)
        {
            if (m_BoardSize < 12)
            {
                m_BoardSize += 2;
                m_IncreaseBoardSize.Text = getIncreaseButtonText();
            }
        }

        // Sends the button's title in dynamic format in order to change the button's title in real time.
        private string getIncreaseButtonText()
        {
            return string.Format("Board Size: {0}X{0} (click to increase)", m_BoardSize);
        }
    }
}
