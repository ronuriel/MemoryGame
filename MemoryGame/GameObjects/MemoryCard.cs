using System;
using System.Drawing;

namespace GameObjects
{
    public struct CardLocation
    {
        private int m_CardRow;
        private int m_CardColumn;

        public CardLocation(int i_CardRow, int i_CardColumn)
        {
            m_CardRow = i_CardRow;
            m_CardColumn = i_CardColumn;
        }

        public int CardRow
        {
            get
            {
                return m_CardRow;
            }
            set
            {
                m_CardRow = value;
            }
        }

        public int CardColumn
        {
            get
            {
                return m_CardColumn;
            }
            set
            {
                m_CardColumn = value;
            }
        }

    }

    public class MemoryCard
    {
        private char m_Value;
        private bool m_IsChosen = false;
        private Point m_CardPosition;

        public MemoryCard(char i_Value)
        {
            m_Value = i_Value;
            m_IsChosen = false;
        }

        public Point CardPosition
        {
            get
            {
                return m_CardPosition;
            }
            set
            {
                m_CardPosition = value;
            }
        }

        public char Value
        {
            get
            {
                return m_Value;
            }
        }

        public bool IsChosen
        {
            get
            {
                return m_IsChosen;
            }
            set
            {
                m_IsChosen = value;
            }
        }
    }
}