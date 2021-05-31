using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameObjects;

namespace MemoryGame
{
    public partial class GameForm : Form
    {
        private SettingsFormDesigner m_settingsForm = new SettingsFormDesigner();
        private MessageBox m_endGameForm;

        private Game m_Game;

        private Button[,] ButtonCards;
        private Image[] m_CardImages;

        private Label labelCurrentPlayer = new Label();
        private Label labelFirstPlayer = new Label();
        private Label labelSecondPlayer = new Label();
        
        private const string k_MemoryGameTitle = "Memory Game";
        private const int k_Space = 10;
        private readonly Size r_CardSize = new Size(80, 80);
        private readonly Image r_BackCard = Properties.Resources.BackCard;

        public GameForm()
        {
            m_settingsForm.ShowDialog();
            m_Game = new Game(m_settingsForm);
            initComponents();
            int boardWidth = m_Game.m_BoardGame.Width;
            this.ClientSize = new Size(boardWidth * (r_CardSize.Width + k_Space) + k_Space, labelSecondPlayer.Bottom + k_Space);
            this.Text = k_MemoryGameTitle;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            addNotifieres();
        }
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

  

        private void addNotifieres()
        {
            m_Game.cardFlipped += buttonCard_Flipped;
            m_Game.foundAMatch += gameUpdateFoundAMatch; ;
        }

        private void gameUpdateFoundAMatch(bool i_FoundAMatch)
        {
            if (i_FoundAMatch)
            {
                updatePlayerPoints(m_Game.m_PlayerATurn);
                if(m_Game.m_BoardGame.UnReavealedCards.Count == 0)
                {                    
                    endGame();
                }
            }
            else
            {
                changeTurn(m_Game.m_PlayerATurn);
                
            }
            if (!m_Game.m_PlayerATurn && !m_settingsForm.TextBoxSecondPlayer.Enabled)
            {
                m_Game.ComputerTurn();
            }

        }

        private void endGame()
        {
            Hide();
            m_endGameForm = new MessageBox(m_Game.m_PlayerA, m_Game.m_PlayerB, this);
            m_endGameForm.ShowDialog();            
        }
        internal void Rematch()
        {
            m_Game = new Game(m_settingsForm);
            Controls.Clear();
            initComponents();
            addNotifieres();
            Show();
        }

        private void changeTurn(bool i_PlayerATurn)
        {
            if (i_PlayerATurn)
            {
                labelCurrentPlayer.Text = "Current Player: " + m_Game.m_PlayerA.Name;
                labelCurrentPlayer.BackColor = labelFirstPlayer.BackColor;
            }
            else
            {
                labelCurrentPlayer.Text = "Current Player: " + m_Game.m_PlayerB.Name;
                labelCurrentPlayer.BackColor = labelSecondPlayer.BackColor;
                
            }

            labelCurrentPlayer.Update();
        }

        private void updatePlayerPoints(bool i_PlayerATurn)
        {
            if (i_PlayerATurn)
            {
                labelFirstPlayer.Text = m_Game.m_PlayerA.ToString();
            }
            else
            {
                labelSecondPlayer.Text = m_Game.m_PlayerB.ToString();
            }
        }

        private void initComponents()
        {
            WebClient w = new WebClient();
            m_CardImages = new Image[m_Game.m_BoardGame.PairsToReveal];
            for( int i = 0; i < m_CardImages.Length; i++)
            {
                m_CardImages[i] = Image.FromStream( new MemoryStream( w.DownloadData("https://picsum.photos/80")));
            }

            Size boardSize = m_settingsForm.GetBoardSize();
            ButtonCards = new Button[boardSize.Height, boardSize.Width];
            for (int i = 0; i < boardSize.Height; i++)
            {
                for (int j = 0; j < boardSize.Width; j++)
                {
                    ButtonCards[i, j] = new Button();
                    Button currentButtonCard = ButtonCards[i, j];
                    currentButtonCard.Size = r_CardSize;
                    currentButtonCard.Location = new Point(j * (r_CardSize.Height + k_Space) + k_Space, i * (r_CardSize.Width + k_Space) + k_Space);
                    currentButtonCard.Click += new EventHandler(buttonCard_Click);
                    currentButtonCard.BackgroundImage = r_BackCard;
                    Controls.Add(currentButtonCard);
                }
            }

            //Current Player Label
            labelCurrentPlayer.AutoSize = true;
            labelCurrentPlayer.Text = "Current Player: " + m_Game.m_PlayerA.Name;
            labelCurrentPlayer.Top = boardSize.Height * (k_Space + r_CardSize.Width) + k_Space;
            labelCurrentPlayer.Left = k_Space;
            Controls.Add(labelCurrentPlayer);

            //First Player Label
            labelFirstPlayer.AutoSize = true;
            labelFirstPlayer.Left = k_Space;
            labelFirstPlayer.Top = labelCurrentPlayer.Bottom + k_Space;
            labelFirstPlayer.BackColor = Color.Green;
            labelFirstPlayer.Text = m_Game.m_PlayerA.ToString();
            Controls.Add(labelFirstPlayer);

            //Second Player Label
            labelSecondPlayer.AutoSize = true;
            labelSecondPlayer.Left = labelFirstPlayer.Left;
            labelSecondPlayer.Top = labelFirstPlayer.Bottom + k_Space;
            labelSecondPlayer.BackColor = Color.Purple;
            labelSecondPlayer.Text = m_Game.m_PlayerB.ToString();
            Controls.Add(labelSecondPlayer);

            labelCurrentPlayer.BackColor = labelFirstPlayer.BackColor;
        }

        private void buttonCard_Click(object sender, EventArgs e)
        {
            m_Game.CardChosen(convertPointToCardLocation((sender as Button).Location));
        }

        private Point convertPointToCardLocation(Point i_CardLocation)
        {
            int width = i_CardLocation.X;
            int height = i_CardLocation.Y;
            int length = r_CardSize.Width;

            return new Point(height / (length + k_Space), width / (length + k_Space));
        }

        private void buttonCard_Flipped(Point i_CardToFlipLocation)
        {
            Button cardButton = ButtonCards[i_CardToFlipLocation.X, i_CardToFlipLocation.Y];
            MemoryCard currentMemoryCard = m_Game.m_BoardGame.GetCard(i_CardToFlipLocation);
            System.Threading.Thread.Sleep(800);

            if (currentMemoryCard.IsChosen)
            {
                int cardIndex = (int)(currentMemoryCard.Value) - 'A';
                cardButton.BackgroundImage = m_CardImages[cardIndex];
                cardButton.BackColor = labelCurrentPlayer.BackColor;
                cardButton.Enabled = false;
            }
            else
            {
                cardButton.BackColor = Control.DefaultBackColor;
                cardButton.BackgroundImage = r_BackCard;
                cardButton.Enabled = true;
            }
        }

    }


}