using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFX
{
    public class SFXTask
    {
        public string Name { get; }
        private Sample Sample { get; }
        private CancellationTokenSource Cancellation { get; set; }
        private Task LoopTask { get; set; }
        private WaveOutEvent currentPlayer;
        private long StartTick;
        private bool fade_occurence;

        public SFXTask(string name, Sample sample, long current_ticks, bool fade_occurence = true)
        {
            Name = name;
            Sample = sample;
            Cancellation = new CancellationTokenSource();
            LoopTask = Task.Run(() => LoopPlayAsync(Cancellation.Token, current_ticks));
            this.fade_occurence = fade_occurence;
        }
        private async Task LoopPlayAsync(CancellationToken token, long current_ticks)
        {
            string filePath = $"{SFX.SFX_AssetsFolder}/{Sample.Name}.wav";
            if (!File.Exists(filePath)) SFX.CreateWaveFile(Sample);

            StartTick = current_ticks;

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
