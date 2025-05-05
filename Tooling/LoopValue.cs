using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooling
{
    public class LoopValue
    {
        public int min, max;

        int m_value = default;
        public int Value
        {
            get => m_value;
            set
            {
                m_value = value;
                while (m_value < min) m_value += max - min + 1;
                while (m_value > max) m_value -= max - min + 1;
            }
        }
        public LoopValue()
        {
        }
        public LoopValue(int v, int min, int max)
        {
            this.min = min;
            this.max = max;
            Value = v;
        }
    }

    public class LoopValueF
    {
        public float min, max;

        float m_value = default;
        public float Value
        {
            get => m_value;
            set
            {
                m_value = value;
                while (m_value < min) m_value += max - min;
                while (m_value >= max) m_value -= max - min;
            }
        }
        public LoopValueF()
        {
        }
        public LoopValueF(float v, float min, float max)
        {
            this.min = min;
            this.max = max;
            Value = v;
        }
    }
}
