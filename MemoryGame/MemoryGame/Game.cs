using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GameObjects;

namespace MemoryGame
{
    public delegate void CardButtonFlippedNotifier(Point i_cardButtonFlippedLocation);
    public delegate void FoundAMatchNotifier(bool i_FoundAMatch);

    public class Game
    {

        internal const int k_AgainstFriend = 0;
        internal const int k_AgainstComputer = 1;
        internal const string k_Computer = "COMPUTER";

        internal bool m_PlayerATurn = true;
        internal Player m_PlayerA;
        internal Player m_PlayerB;
        private int m_GameMode;
        internal Board m_BoardGame;
        private int m_NumOfCardsChosen = 0;

        private MemoryCard m_FirstCard;
        private MemoryCard m_SecondCard;
        private readonly Random sr_RandomLocation = new Random();

        public event CardButtonFlippedNotifier cardFlipped;
        public event FoundAMatchNotifier foundAMatch;


        public Game(SettingsFormDesigner i_SettingsForm)
        {
            m_PlayerA = new Player(i_SettingsForm.TextBoxFirstPlayer.Text);
            if (i_SettingsForm.TextBoxSecondPlayer.Enabled)
            {
                m_GameMode = k_AgainstFriend;
                m_PlayerB = new Player(i_SettingsForm.TextBoxSecondPlayer.Text);
            }
            else
            {
                m_GameMode = k_AgainstComputer;
                m_PlayerB = new Player(k_Computer);
            }
            Size boardSize = i_SettingsForm.GetBoardSize();
            m_BoardGame = new Board(boardSize.Width, boardSize.Height);
        }

        public void CardChosen(Point i_CardLocation)
        {
            m_NumOfCardsChosen++;

            if (m_NumOfCardsChosen == 1)
            {
                m_FirstCard = m_BoardGame.GetCard(i_CardLocation);
                flip(m_FirstCard.CardPosition);
            }
            else
            {
                m_SecondCard = m_BoardGame.GetCard(i_CardLocation);
                flip(m_SecondCard.CardPosition);

                endTurn();
                
            }

        }

        private void endTurn()
        {
            bool foundMatch = m_BoardGame.CheckIfMatch(m_FirstCard.CardPosition, m_SecondCard.CardPosition);
            if (foundMatch)
            {
                reduceOnePairFromTotal();
                updateCurrentPlayerPoints();
            }
            else
            {
                flip(m_FirstCard.CardPosition);
                flip(m_SecondCard.CardPosition);
                passTurnToOtherPlayer();
            }

            m_NumOfCardsChosen = 0;
            foundAMatch(foundMatch);
        }

        private void flip(Point i_CardToFlip)
        {
            m_BoardGame.Flip(i_CardToFlip);
            cardFlipped(i_CardToFlip);
        }

        private void updateCurrentPlayerPoints()
        {
            Player currentPlayer;
            if (m_PlayerATurn)
            {
                currentPlayer = m_PlayerA;
            }
            else
            {
                currentPlayer = m_PlayerB;
            }
            currentPlayer.UpdatePoints(currentPlayer.Points + 1);
        }

        internal void ComputerTurn()
        {
            int itemList;
            int boardWidth = m_BoardGame.Width;
            for (int i = 0; i < 2; i++)
            {
                Point computerChoiceLocation = new Point();
                int randomItemFromList = sr_RandomLocation.Next(m_BoardGame.UnReavealedCards.Count);

                itemList = m_BoardGame.UnReavealedCards[randomItemFromList];
                computerChoiceLocation.X = itemList / boardWidth;
                computerChoiceLocation.Y = itemList % boardWidth;
                CardChosen(computerChoiceLocation);
            }
        }
        private void reduceOnePairFromTotal()
        {
            m_BoardGame.PairsToReveal--;
        }

        private void passTurnToOtherPlayer()
        {
            m_PlayerATurn = !m_PlayerATurn;
        }

        private void resetPoints()
        {
            m_PlayerA.Points = 0;
            m_PlayerB.Points = 0;
        }
    }
}