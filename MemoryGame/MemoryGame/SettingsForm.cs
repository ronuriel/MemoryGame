using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MemoryGame
{

    public class SettingsFormDesigner : Form
    {

        internal Label LabelFirstPlayer = new Label();
        internal Label LabelSecondPlayer = new Label();
        internal Label LabelBoardSize = new Label();

        internal Button ButtonStart = new Button();
        internal Button ButtonMultiPlayer = new Button();
        internal Button ButtonBoardSize = new Button();

        internal TextBox TextBoxFirstPlayer = new TextBox();
        internal TextBox TextBoxSecondPlayer = new TextBox();

        internal readonly string[] r_boardSizeList = { "4X4", "4X5", "4X6", "5X4", "5X6", "6X4", "6X5", "6X6" };
        internal int m_CurrentBoardSize = 0;
        private string k_Computer = "- computer -";
        private string k_AgainstFriend = "Against a Friend";
        private string k_Start = "Start!";
        private string k_AgainstComputer = "Against Computer";
        private const string k_SecondPlayerName = "Second Player Name:";
        private const string k_BoardSize = "Board Size:";
        private const string k_FirstPlayerName = "First Player Name:";
        private const string k_SettingsTitle = "Memory Game - Settings";
        internal const int k_MarginSpace = 12;
        internal const int k_ControlsSpace = 5;
        internal const string k_TextBoxSecondPlayerString = "- computer -";

        public SettingsFormDesigner()
        {
            initComponents();
            this.Text = k_SettingsTitle;
            this.ClientSize = new Size(ButtonMultiPlayer.Right + k_MarginSpace, ButtonBoardSize.Bottom + k_MarginSpace);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

        }


        private void initComponents()
        {
            // First Player Label
            this.LabelFirstPlayer.Size = new Size(120, 20);
            this.LabelFirstPlayer.Text = k_FirstPlayerName;
            this.LabelFirstPlayer.Left = k_MarginSpace;
            this.LabelFirstPlayer.Top = k_MarginSpace;
            this.Controls.Add(LabelFirstPlayer);


            // Second Player Label
            this.LabelSecondPlayer.Size = this.LabelFirstPlayer.Size;
            this.LabelSecondPlayer.Text = k_SecondPlayerName;
            this.LabelSecondPlayer.Left = this.LabelFirstPlayer.Left;
            this.LabelSecondPlayer.Top = LabelFirstPlayer.Bottom + k_ControlsSpace;
            this.Controls.Add(LabelSecondPlayer);


            // Board Size Label
            LabelBoardSize.Size = this.LabelFirstPlayer.Size;
            this.LabelBoardSize.Text = k_BoardSize;
            this.LabelBoardSize.Left = this.LabelFirstPlayer.Left;
            this.LabelBoardSize.Top = LabelSecondPlayer.Bottom + k_ControlsSpace;
            this.Controls.Add(LabelBoardSize);

            // First Player Text Box
            TextBoxFirstPlayer.Size = this.LabelFirstPlayer.Size;
            this.TextBoxFirstPlayer.Left = this.LabelFirstPlayer.Right + k_ControlsSpace;
            this.TextBoxFirstPlayer.Top = this.LabelFirstPlayer.Top;
            this.Controls.Add(TextBoxFirstPlayer);

            // Second Player Text Box
            TextBoxSecondPlayer.Size = this.LabelFirstPlayer.Size;
            this.TextBoxSecondPlayer.Left = this.TextBoxFirstPlayer.Left;
            this.TextBoxSecondPlayer.Top = this.TextBoxFirstPlayer.Bottom + k_ControlsSpace;
            this.TextBoxSecondPlayer.Text = k_Computer;
            this.TextBoxSecondPlayer.Enabled = false;
            this.Controls.Add(TextBoxSecondPlayer);

            //MultiPlayer Button
            this.ButtonMultiPlayer.Size = this.LabelFirstPlayer.Size;
            this.ButtonMultiPlayer.Left = this.TextBoxSecondPlayer.Right + k_ControlsSpace;
            this.ButtonMultiPlayer.Top = this.TextBoxSecondPlayer.Top;
            this.ButtonMultiPlayer.Text = k_AgainstFriend;
            this.ButtonMultiPlayer.Click += new EventHandler(buttonMultiPlayer_Click);
            this.Controls.Add(ButtonMultiPlayer);

            //Board Size Button
            this.ButtonBoardSize.Size = new Size(100, 70);
            this.ButtonBoardSize.Left = this.LabelFirstPlayer.Left;
            this.ButtonBoardSize.Top = this.LabelBoardSize.Bottom;
            this.ButtonBoardSize.BackColor = Color.Purple;
            this.ButtonBoardSize.Text = r_boardSizeList[m_CurrentBoardSize];
            this.ButtonBoardSize.Click += new EventHandler(buttonBoardSize_Click);
            this.Controls.Add(ButtonBoardSize);

            //Start Button
            this.ButtonStart.Size = new Size(75, 20);
            this.ButtonStart.Left = this.ButtonMultiPlayer.Left + (ButtonMultiPlayer.Width - ButtonStart.Width);
            this.ButtonStart.Top = this.ButtonBoardSize.Bottom - this.ButtonStart.Height;
            this.ButtonStart.BackColor = Color.Green;
            this.ButtonStart.Text = k_Start;
            this.ButtonStart.Click += new EventHandler(buttonStart_Click);
            this.Controls.Add(ButtonStart);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Close();
        }

        internal Size GetBoardSize()
        {
            int height = int.Parse(ButtonBoardSize.Text[0].ToString());
            int width = int.Parse(ButtonBoardSize.Text[2].ToString());

            return new Size(width, height);
        }

        private void buttonMultiPlayer_Click(object sender, EventArgs e)
        {
            if (!TextBoxSecondPlayer.Enabled)
            {
                TextBoxSecondPlayer.Text = string.Empty;
                ButtonMultiPlayer.Text = k_AgainstComputer;
            }
            else
            {
                TextBoxSecondPlayer.Text = k_TextBoxSecondPlayerString;
                ButtonMultiPlayer.Text = k_AgainstFriend;
            }

            TextBoxSecondPlayer.Enabled = !TextBoxSecondPlayer.Enabled;
        }

        private void buttonBoardSize_Click(object sender, EventArgs e)
        {
            if (m_CurrentBoardSize == r_boardSizeList.Length - 1)
            {
                m_CurrentBoardSize = 0;
            }
            else
            {
                m_CurrentBoardSize++;
            }

            this.ButtonBoardSize.Text = r_boardSizeList[m_CurrentBoardSize];
        }
    }
}