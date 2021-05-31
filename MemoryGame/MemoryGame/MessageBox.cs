using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using GameObjects;

namespace MemoryGame
{
    public class MessageBox : Form
    {
        internal const int k_MarginSpace = 12;
        internal const int k_ControlsSpace = 5;
        private const string k_TieMessage = "it's a TIE!!!";
        private const string k_WinnerMessage = "The Winner is: ";
        private const string k_GameOverMessage = "Game Over!";
        private const string k_RematchQuestion = "Do you want to play another game?";
        private const string k_OK = "OK";
        private const string k_NO = "NO";
        private const string k_ByeMessage = "Bye Bye! See You Next Time.";
        private readonly Size r_ButtonSize = new Size(80, 25);

        Label labelGameOverMessage = new Label();
        Label labelFirstPlayerScore = new Label();
        Label labelSecondPlayerScore = new Label();
        Label labelWinner = new Label();
        Label labelRematchQuestion = new Label();

        Button buttonOK = new Button();
        Button buttonNO = new Button();

        GameForm m_GameForm;
        

        public MessageBox(Player i_PlayerA, Player i_PlayerB, GameForm i_GameForm)
        {
            m_GameForm = i_GameForm;
            initComponents(i_PlayerA, i_PlayerB);
            this.ClientSize = new Size(buttonNO.Right + k_MarginSpace, buttonNO.Bottom + k_MarginSpace);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            labelGameOverMessage.Left = (ClientSize.Width - labelGameOverMessage.Size.Width) / 2;
            labelWinner.Left = (ClientSize.Width - labelWinner.Size.Width) / 2;
        }

        private void initComponents(Player i_PlayerA, Player i_PlayerB)
        {
            // Game Over Label
            this.labelGameOverMessage.AutoSize = true;
            this.labelGameOverMessage.Text = k_GameOverMessage;
            this.labelGameOverMessage.Top = k_MarginSpace;
            this.labelGameOverMessage.Font = new Font(labelGameOverMessage.Font, FontStyle.Bold);
            this.Controls.Add(labelGameOverMessage);

            // First Player Score Label
            this.labelFirstPlayerScore.AutoSize = true;
            this.labelFirstPlayerScore.Text = i_PlayerA.ToString();
            this.labelFirstPlayerScore.Left = k_MarginSpace;
            this.labelFirstPlayerScore.Top = this.labelGameOverMessage.Bottom + k_MarginSpace;
            this.Controls.Add(this.labelFirstPlayerScore);

            // Second Player Score Label
            this.labelSecondPlayerScore.AutoSize = true;
            this.labelSecondPlayerScore.Text = i_PlayerB.ToString();
            this.labelSecondPlayerScore.Left = k_MarginSpace;
            this.labelSecondPlayerScore.Top = this.labelFirstPlayerScore.Bottom + k_MarginSpace;
            this.Controls.Add(this.labelSecondPlayerScore);

            // Winner Label
            this.labelWinner.AutoSize = true;
            this.labelWinner.Text = winnerMessage(i_PlayerA, i_PlayerB);
            this.labelWinner.Top = this.labelSecondPlayerScore.Bottom + k_MarginSpace;
            this.labelWinner.Font = new Font(labelWinner.Font, FontStyle.Bold);
            this.Controls.Add(this.labelWinner);

            // Rematch Question Label
            this.labelRematchQuestion.AutoSize = true;
            this.labelRematchQuestion.Text = k_RematchQuestion;
            this.labelRematchQuestion.Left = k_MarginSpace;
            this.labelRematchQuestion.TextAlign = ContentAlignment.MiddleCenter;
            this.labelRematchQuestion.Top = this.labelWinner.Bottom + k_MarginSpace;
            this.Controls.Add(labelRematchQuestion);

            //Ok Button
            this.buttonOK.Size = r_ButtonSize;
            this.buttonOK.Left = k_MarginSpace;
            this.buttonOK.Top = this.labelRematchQuestion.Bottom + k_MarginSpace;
            this.buttonOK.Text = k_OK;
            this.buttonOK.Click += new EventHandler(buttonOK_Click);
            this.Controls.Add(buttonOK);

            //NO Button
            this.buttonNO.Size = r_ButtonSize;
            this.buttonNO.Left = k_MarginSpace + this.buttonOK.Right;
            this.buttonNO.Top = this.buttonOK.Top;
            this.buttonNO.Text = k_NO;
            this.buttonNO.Click += new EventHandler(buttonNO_Click);
            this.Controls.Add(buttonNO);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                closeAppliction();
            }
        }

        private void buttonNO_Click(object sender, EventArgs e)
        {
            closeAppliction();
        }

        private void closeAppliction()
        {
            System.Windows.Forms.MessageBox.Show(k_ByeMessage);
            Environment.Exit(0);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            m_GameForm.Rematch();
            Hide();
        }

        private string winnerMessage(Player i_PlayerA, Player i_PlayerB)
        {
            string winnerString;

            if (i_PlayerA.Points == i_PlayerB.Points)
            {
                winnerString = k_TieMessage;
            }
            else
            {
                string winnerName;

                if (i_PlayerA.Points > i_PlayerB.Points)
                {
                    winnerName = i_PlayerA.Name;
                }
                else
                {
                    winnerName = i_PlayerB.Name;
                }

                winnerString = string.Format("{0}{1}!!!", k_WinnerMessage, winnerName);
            }

            return winnerString;
        }


    }
}
