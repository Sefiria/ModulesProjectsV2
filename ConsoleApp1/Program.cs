using SFX;

Sample s1 = new Sample([0x14F3], SampleRates.VeryLow, Instruments.Saw, 0.2);

await SFX.SFX.PlayAsync(s1);