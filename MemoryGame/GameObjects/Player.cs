using System;
using System.Text;

namespace GameObjects
{
    public class Player
    {
        private string m_Name;
        private int m_Point = 0;

        public Player(string i_NameOfPlayer)
        {
            m_Name = i_NameOfPlayer;
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public int Points
        {
            get
            {
                return m_Point;
            }
            set
            {
                m_Point = value;
            }
        }

        public void UpdatePoints(int i_NewPoints)
        {
            m_Point = i_NewPoints;
        }

        public string ToString()
        {
            StringBuilder playerDisplay = new StringBuilder();
            playerDisplay.AppendFormat("Player {0} has {1} points",m_Name ,m_Point);
            return playerDisplay.ToString();
        }
    }
}
