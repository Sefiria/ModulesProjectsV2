using NAudio.Wave;
using SFX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project7
{
    public partial class Game1
    {
        public Sample SE_GIFT, SE_RABBII_GRIP, SE_RABBII_RELEASE;
        public Sample SE_QUEST_SUCCESS;
        public Sample SE_FLY_FLYING, SE_FLY_DYING;
        public List<Sample> SE_RABBII_JUMPS;

        private int activeSounds = 0;
        private static readonly object lockObject = new object();
        private int max_sfx = 4;
        private List<SFXTask> RepeatSFXTasks = new List<SFXTask>();

        void LoadSFX()
        {
            // 0xABCD : A=Pitch, B=Octave, C=Volume, D=Duration (from 0 (0) to F (15))
            SE_GIFT = new Sample([0x26A2, 0x46A2], SampleRates.MediumDown, Instruments.Triangle, 0.2, Name: "SE_GIFT");
            SE_RABBII_GRIP = new Sample([0x32F2, 0x62E2, 0x33E2, 0x63E2], SampleRates.MediumDown, Instruments.Triangle, 0.3, Name: "SE_RABBII_GRIP");
            SE_RABBII_RELEASE = new Sample([0x63D2, 0x33D2, 0x62D2, 0x32D2], SampleRates.MediumDown, Instruments.Triangle, 0.2, Name: "SE_RABBII_RELEASE");
            SE_RABBII_JUMPS = new List<Sample>()
            {
                new Sample([0x32E2, 0x42E2, 0x13E2], SampleRates.MediumDown, Instruments.Triangle, 0.2, Name: "SE_RABBII_JUMPS_0"),
                new Sample([0x32E2, 0x52E2, 0x23E2], SampleRates.MediumDown, Instruments.Triangle, 0.2, Name: "SE_RABBII_JUMPS_1"),
                new Sample([0x32E2, 0x62E2, 0x33E2], SampleRates.MediumDown, Instruments.Triangle, 0.2, Name: "SE_RABBII_JUMPS_2")
            };
            SE_QUEST_SUCCESS = new Sample([0x44B2, 0x84B2, 0x45B2, 0x85B2, 0x46B2, 0x27B2], SampleRates.MediumDown, Instruments.Triangle, 0.2, "SE_QUEST_SUCCESS");
            SE_FLY_FLYING = new Sample([0x523B], SampleRates.VeryLow, Instruments.Saw, 0.05, "SE_FLY_FLYING");
            SE_FLY_DYING = new Sample([0x32B2, 0x12B2, 0x51B2], SampleRates.MediumDown, Instruments.Triangle, 0.2, "SE_FLY_DYING");
        }
        public async Task PlaySoundAsync(Sample se)
        {
            lock (lockObject)
            {
                if (activeSounds >= max_sfx)
                    return;
                activeSounds++;
            }

            await SFX.SFX.PlayAndCacheAsync(se);

            lock (lockObject)
            {
                activeSounds--;
            }
        }
        public void StartRepeatSoundAsync(string name, Sample se, bool fade_occurence = true, long timeout = 0)
        {
            if (RepeatSFXTasks.FirstOrDefault(t => t.Name == name) == null)
            {
                RepeatSFXTasks.Add(new SFXTask(name, se, fade_occurence));
            }
        }
        public void StopRepeatSoundAsync(string name)
        {
            var task = RepeatSFXTasks.FirstOrDefault(t => t.Name == name);
            if (task != null)
            {
                task.Stop();
                RepeatSFXTasks.Remove(task);
            }
        }
    }

    public class SFXTask
    {
        public string Name { get; }
        private Sample Sample { get; }
        private CancellationTokenSource Cancellation { get; set; }
        private Task LoopTask { get; set; }
        private WaveOutEvent currentPlayer;
        private long StartTick;
        private bool fade_occurence;

        public SFXTask(string name, Sample sample, bool fade_occurence = true)
        {
            Name = name;
            Sample = sample;
            Cancellation = new CancellationTokenSource();
            LoopTask = Task.Run(() => LoopPlayAsync(Cancellation.Token));
            this.fade_occurence = fade_occurence;
        }
        private async Task LoopPlayAsync(CancellationToken token)
        {
            string filePath = $"{SFX.SFX.SFX_AssetsFolder}/{Sample.Name}.wav";
            if (!File.Exists(filePath)) SFX.SFX.CreateWaveFile(Sample);

            StartTick = Game1.Instance.Ticks;

            while (!token.IsCancellationRequested)
            {
                // Créer un nouveau lecteur et un nouveau player à chaque itération
                var audioFile = new AudioFileReader(filePath);
                var player = new WaveOutEvent();

                player.Init(audioFile);
                player.Play();

                // Ne pas stocker dans currentPlayer, car plusieurs peuvent jouer en même temps
                // Nettoyage automatique après lecture
                _ = Task.Run(async () =>
                {
                    await Task.Delay(audioFile.TotalTime);
                    player.Dispose();
                    audioFile.Dispose();
                });

                // Attendre avant de relancer une nouvelle lecture
                var delay = fade_occurence ? audioFile.TotalTime.Multiply(0.8) : audioFile.TotalTime - TimeSpan.FromMilliseconds(100);
                if (delay.TotalMilliseconds > 0)
                    await Task.Delay(delay, token);
            }
        }
        public void Stop()
        {
            Cancellation.Cancel();
            currentPlayer?.Stop();
        }
        public bool IsRunning => !LoopTask.IsCompleted;
    }
}