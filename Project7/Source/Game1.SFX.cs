using SFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7
{
    public partial class Game1
    {
        public Sample SE_GIFT, SE_RABBII_GRIP, SE_RABBII_RELEASE;
        public List<Sample> SE_RABBII_JUMPS;

        private int activeSounds = 0;
        private static readonly object lockObject = new object();
        private int max_sfx = 4;

        void LoadSFX()
        {
            SE_GIFT = new Sample([0x26A1, 0x46A1], SampleRates.MediumDown, Instruments.Triangle, 0.2);
            SE_RABBII_GRIP = new Sample([0x32F1, 0x62E1, 0x33E1, 0x63E1], SampleRates.MediumDown, Instruments.Triangle, 0.3);
            SE_RABBII_RELEASE = new Sample([0x63D1, 0x33D1, 0x62D1, 0x32D1], SampleRates.MediumDown, Instruments.Triangle, 0.2);
            SE_RABBII_JUMPS = new List<Sample>()
            {
                new Sample([0x32E1, 0x42E1, 0x13E1], SampleRates.MediumDown, Instruments.Triangle, 0.2),
                new Sample([0x32E1, 0x52E1, 0x23E1], SampleRates.MediumDown, Instruments.Triangle, 0.2),
                new Sample([0x32E1, 0x62E1, 0x33E1], SampleRates.MediumDown, Instruments.Triangle, 0.2)
            };
        }
        public async Task PlaySoundAsync(Sample se)
        {
            lock (lockObject)
            {
                if (activeSounds >= max_sfx)
                    return;
                activeSounds++;
            }

            await SFX.SFX.PlayAsync(se);

            lock (lockObject)
            {
                activeSounds--;
            }
        }
    }
}
