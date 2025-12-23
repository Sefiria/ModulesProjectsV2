using SFX;
using System.Collections.Generic;

namespace Project8
{
    public partial class GameMain
    {
        public Sample SE_GIFT, SE_GRIP, SE_RELEASE;
        public Sample SE_QUEST_SUCCESS;
        public Sample SE_FLY_FLYING, SE_FLY_DYING;
        public List<Sample> SE_JUMPS;

        void LoadSFX()
        {
            // 0xABCD : A=Pitch, B=Octave, C=Volume, D=Duration (from 0 (0) to F (15))
            SE_GIFT = new Sample([0x26A2, 0x46A2], SampleRates.MediumDown, Instruments.Triangle, 0.2, Name: "SE_GIFT");
            SE_GRIP = new Sample([0x32F2, 0x62E2, 0x33E2, 0x63E2], SampleRates.MediumDown, Instruments.Triangle, 0.3, Name: "SE_GRIP");
            SE_RELEASE = new Sample([0x63D2, 0x33D2, 0x62D2, 0x32D2], SampleRates.MediumDown, Instruments.Triangle, 0.2, Name: "SE_RELEASE");
            SE_JUMPS = new List<Sample>()
            {
                new Sample([0x32E2, 0x42E2, 0x13E2], SampleRates.MediumDown, Instruments.Triangle, 0.2, Name: "SE_JUMPS_0"),
                new Sample([0x32E2, 0x52E2, 0x23E2], SampleRates.MediumDown, Instruments.Triangle, 0.2, Name: "SE_JUMPS_1"),
                new Sample([0x32E2, 0x62E2, 0x33E2], SampleRates.MediumDown, Instruments.Triangle, 0.2, Name: "SE_JUMPS_2")
            };
            SE_QUEST_SUCCESS = new Sample([0x44B2, 0x84B2, 0x45B2, 0x85B2, 0x46B2, 0x27B2], SampleRates.MediumDown, Instruments.Triangle, 0.2, "SE_QUEST_SUCCESS");
            SE_FLY_FLYING = new Sample([0x523B], SampleRates.VeryLow, Instruments.Saw, 0.02, "SE_FLY_FLYING");
            SE_FLY_DYING = new Sample([0x32B2, 0x12B2, 0x51B2], SampleRates.MediumDown, Instruments.Triangle, 0.2, "SE_FLY_DYING");
        }
    }
}