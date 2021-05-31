using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;

namespace GameObjects
{
    public class Board
    {
        private MemoryCard[,] m_Board;
        private int m_BoardWidth;
        private int m_BoardHeight;
        private int m_PairsToReveal;
        private List<int> m_UnReavealedCards;
        private List<MemoryCard> m_OpenedCardsPool;


        public Board(int i_Width, int i_Height)
        {
            m_BoardHeight = i_Height;
            m_BoardWidth = i_Width;
            m_PairsToReveal = (m_BoardWidth * m_BoardHeight) / 2;
            m_OpenedCardsPool = new List<MemoryCard>();
            m_Board = new MemoryCard[m_BoardHeight, m_BoardWidth];
            m_UnReavealedCards = new List<int>();
            List<int> placementCard = new List<int>();            
            int numOfCardsValue = m_PairsToReveal;

            for (int i = 0; i < m_BoardWidth * m_BoardHeight; i++)
            {
                m_UnReavealedCards.Add(i);
                placementCard.Add(i);

            }

            for (int i = 0; i < numOfCardsValue; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    MemoryCard cardToPlace = new MemoryCard((char)(i + 65));
                    cardPlacement(placementCard, cardToPlace);
                }
            }
        }

        public List<int> UnReavealedCards
        {
            get
            {
                return m_UnReavealedCards;
            }
        }

        public int PairsToReveal
        {
            get
            {
                return m_PairsToReveal;
            }
            set
            {
                m_PairsToReveal = value;
            }
        }

        public int Width
        {
            get
            {
                return m_BoardWidth;
            }
            set
            {
                m_BoardWidth = value;
            }
        }

        public int Height
        {
            get
            {
                return m_BoardHeight;
            }
            set
            {
                m_BoardHeight = value;
            }
        }

        private void cardPlacement(List<int> i_Locations, MemoryCard io_CardToPlace)
        {
            Random randomLocation = new Random();
            int randomIndexFromList = randomLocation.Next(i_Locations.Count);
            int locationFromList = i_Locations[randomIndexFromList];
            int cardLocationX = locationFromList / m_BoardWidth;
            int cardLocationY = locationFromList % m_BoardWidth;

            i_Locations.RemoveAt(randomIndexFromList);            
            io_CardToPlace.CardPosition = new Point(cardLocationX, cardLocationY);
            m_Board[cardLocationX, cardLocationY] = io_CardToPlace;
        }

        public bool CheckIfPairAlreadyRevealed(ref Point o_FirstCard, ref Point o_SecondCard)
        {
            bool pairIsFound = false;
            MemoryCard cardToCheck;
            MemoryCard cardExist;
            int openedCardsPoolCount = m_OpenedCardsPool.Count;

            for (int i = 0; i < openedCardsPoolCount; i++)
            {
                cardToCheck = m_OpenedCardsPool[i];
                m_OpenedCardsPool.RemoveAt(i);

                for (int j = i; j < openedCardsPoolCount - 1; j++)
                {
                    cardExist = m_OpenedCardsPool[j];
                    if (cardExist.Value == cardToCheck.Value)
                    {
                        o_FirstCard = cardToCheck.CardPosition;
                        o_SecondCard = cardExist.CardPosition;
                        m_OpenedCardsPool.RemoveAt(j);
                        pairIsFound = true;
                        break;
                    }
                }

                if (!pairIsFound)
                {
                    m_OpenedCardsPool.Insert(i, cardToCheck);
                }
                else
                {
                    break;
                }
            }

            return pairIsFound;
        }

        public bool CheckIfPairAlreadyRevealed(Point i_FirstCardLocation, ref Point o_SecondCardLocation)
        {
            bool foundAPair = false;
            MemoryCard firstCard = GetCard(i_FirstCardLocation);
            m_OpenedCardsPool.Remove(firstCard);
            MemoryCard cardToCheck;

            for (int i = 0; i < m_OpenedCardsPool.Count; i++)
            {
                cardToCheck = m_OpenedCardsPool[i];
                if (cardToCheck.Value == firstCard.Value)
                {
                    m_OpenedCardsPool.Remove(cardToCheck);
                    o_SecondCardLocation = cardToCheck.CardPosition;
                    foundAPair = true;
                    break;
                }

            }

            if (!foundAPair)
            {
                m_OpenedCardsPool.Add(firstCard);
            }

            return foundAPair;
        }

        public void Flip(Point i_CardToFlipPosition)
        {
            int row = i_CardToFlipPosition.X;
            int column = i_CardToFlipPosition.Y;
            int CardLocationInBoard = row * m_BoardWidth + column;
            MemoryCard cardToFlip;

            if (m_Board[row, column] != null)
            {
                m_Board[row, column].IsChosen = !m_Board[row, column].IsChosen;
                if (!m_Board[row, column].IsChosen)
                {
                    m_UnReavealedCards.Add(CardLocationInBoard);
                }
                else
                {
                    cardToFlip = GetCard(i_CardToFlipPosition);
                    if (!m_OpenedCardsPool.Contains(cardToFlip))
                    {                        
                        m_OpenedCardsPool.Add(cardToFlip);
                    }

                    m_UnReavealedCards.Remove(CardLocationInBoard);
                }
            }
        }

        public MemoryCard GetCard(Point i_CardLocation)
        {
            return m_Board[i_CardLocation.X, i_CardLocation.Y];
        }

        public bool CheckIfMatch(Point i_FirstCard, Point i_SecondCard)
        {
            MemoryCard firsrCard = GetCard(i_FirstCard);
            MemoryCard secondCard = GetCard(i_SecondCard);

            bool thereIsMatch = false;
            if (firsrCard.Value == secondCard.Value)
            {
                m_OpenedCardsPool.Remove(firsrCard);
                m_OpenedCardsPool.Remove(secondCard);
                thereIsMatch = true;
            }

            return thereIsMatch;
        }

        private StringBuilder firstLine()
        {
            StringBuilder board = new StringBuilder();

            board.Append("   ");
            for (int i = 0; i < m_Board.GetLength(1); i++)
            {
                board.AppendFormat("  {0} ", Convert.ToChar(i + 65));
            }

            return board;
        }

        private StringBuilder separateLine()
        {
            StringBuilder board = new StringBuilder();

            board.AppendFormat("{0}   ", Environment.NewLine);
            for (int i = 0; i < m_Board.GetLength(1); i++)
            {
                board.Append("====");
            }

            board.AppendFormat("={0}", Environment.NewLine);

            return board;
        }

        public string ToString()
        {
            StringBuilder board = new StringBuilder();
            char cardPosition;

            board.AppendFormat("{0}{1}", firstLine(), separateLine());            
            for (int i = 0; i < m_Board.GetLength(0); i++)
            {
                board.AppendFormat("{0}  ", i + 1);
                for (int j = 0; j < m_Board.GetLength(1); j++)
                {
                    if (m_Board[i, j].IsChosen)
                    {
                        cardPosition = m_Board[i, j].Value;
                    }
                    else
                    {
                        cardPosition = ' ';
                    }

                    board.AppendFormat("| {0} ", cardPosition);
                }

                board.AppendFormat("|{0}", separateLine());
            }

            return board.ToString();
        }
    }
}