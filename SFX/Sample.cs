using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFX
{
    public class Sample
    {
        public int[] Notes = [];
        public int SampleRate = 0;
        public Instrument Instrument = Instruments.Noise;
        public double GlobalVolume = 0F;
        public Sample()
        {
        }
        public Sample(int[] Notes, int SampleRate, Instrument Instrument, double GlobalVolume)
        {
            this.Notes = Notes;
            this.SampleRate = SampleRate;
            this.Instrument = Instrument;
            this.GlobalVolume = GlobalVolume;
        }
    }
}
