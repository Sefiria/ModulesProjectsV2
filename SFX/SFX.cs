using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace SFX
{
    public delegate double Instrument(double frequency, double volume, double time);

    public class SFX
    {
        public static readonly string SFX_AssetsFolder = $"{Directory.GetCurrentDirectory()}/Assets/SFX";

        public static async Task PlayAsync(Sample sample) => await PlayAsync(sample.Notes, sample.SampleRate, sample.Instrument, sample.GlobalVolume);
        public static async Task PlayAsync(int[] notes, int sampleRate, Instrument instrument, double global_volume)
        {
            double[] frequencies = ConvertNotesToFrequencies(notes);
            int[] durations = ConvertNotesToDurations(notes);
            double[] volumes = ConvertNotesToVolumes(notes);

            WaveFormat waveFormat = new WaveFormat(sampleRate, 1);
            using (MemoryStream memoryStream = new MemoryStream())
            using (WaveFileWriter waveFileWriter = new WaveFileWriter(memoryStream, waveFormat))
            {
                for (int i = 0; i < notes.Length; i++)
                {
                    int totalSamples = sampleRate * durations[i] / 1000;
                    int attackSamples = totalSamples / 10; // 10% de la durée pour l'attaque
                    int releaseSamples = totalSamples / 10; // 10% de la durée pour la relâche

                    for (int n = 0; n < totalSamples; n++)
                    {
                        double amplitude = frequencies[i] == 0 ? 0 : instrument(frequencies[i], volumes[i] * global_volume, n / (double)sampleRate);

                        // Appliquer l'enveloppe d'attaque
                        if (n < attackSamples)
                        {
                            amplitude *= (double)n / attackSamples;
                        }
                        // Appliquer l'enveloppe de relâche
                        else if (n > totalSamples - releaseSamples)
                        {
                            amplitude *= (double)(totalSamples - n) / releaseSamples;
                        }

                        waveFileWriter.WriteSample((float)amplitude);
                    }
                }

                waveFileWriter.Flush();
                memoryStream.Position = 0;

                using (WaveOutEvent waveOut = new WaveOutEvent())
                using (WaveFileReader waveFileReader = new WaveFileReader(memoryStream))
                {
                    waveOut.Init(waveFileReader);
                    waveOut.Play();
                    await Task.Delay(durations.Sum()); // Attendre la fin de la lecture
                }
            }
        }

        public static async Task PlayAndCacheAsync(Sample sample) => await PlayAndCacheAsync(sample.Name, sample.Notes, sample.SampleRate, sample.Instrument, sample.GlobalVolume);
        public static async Task PlayAndCacheAsync(string name, int[] notes, int sampleRate, Instrument instrument, double global_volume)
        {
            CreateWaveFile(name, notes, sampleRate, instrument, global_volume);

            string filename = $"{SFX_AssetsFolder}/{name}.wav";
            await PlayWavFileAsync(filename);
        }

        public static void CreateWaveFile(Sample sample) => CreateWaveFile(sample.Name, sample.Notes, sample.SampleRate, sample.Instrument, sample.GlobalVolume);
        public static void CreateWaveFile(string name, int[] notes, int sampleRate, Instrument instrument, double global_volume)
        {
            string filename = $"{SFX_AssetsFolder}/{name}.wav";
            if (!File.Exists(filename))
            {
                double[] frequencies = ConvertNotesToFrequencies(notes);
                int[] durations = ConvertNotesToDurations(notes);
                double[] volumes = ConvertNotesToVolumes(notes);

                WaveFormat waveFormat = new WaveFormat(sampleRate, 1);
                using (MemoryStream memoryStream = new MemoryStream())
                using (WaveFileWriter waveFileWriter = new WaveFileWriter(memoryStream, waveFormat))
                {
                    for (int i = 0; i < notes.Length; i++)
                    {
                        int totalSamples = sampleRate * durations[i] / 1000;
                        int attackSamples = totalSamples / 10; // 10% de la durée pour l'attaque
                        int releaseSamples = totalSamples / 10; // 10% de la durée pour la relâche

                        for (int n = 0; n < totalSamples; n++)
                        {
                            double time = n / (double)sampleRate;
                            double amplitude = instrument(frequencies[i], volumes[i] * global_volume, time);

                            // Appliquer l'enveloppe d'attaque
                            if (n < attackSamples)
                            {
                                amplitude *= (double)n / attackSamples;
                            }
                            // Appliquer l'enveloppe de relâche
                            else if (n > totalSamples - releaseSamples)
                            {
                                amplitude *= (double)(totalSamples - n) / releaseSamples;
                            }

                            waveFileWriter.WriteSample((float)amplitude);
                        }
                    }

                    waveFileWriter.Flush();
                    memoryStream.Position = 0;

                    File.WriteAllBytes(filename, memoryStream.ToArray());
                }
            }
        }

        public static async Task PlayWavFileAsync(string filePath)
        {
            using (var audioFile = new AudioFileReader(filePath))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                await Task.Delay(audioFile.TotalTime);
            }
        }

        static double[] ConvertNotesToFrequencies(int[] notes)
        {
            //double[] baseFrequencies = { 16.35, 18.35, 20.60, 21.83, 24.50, 27.50, 30.87 };// 440Hz
            double[] baseFrequencies = { 15.99, 17.98, 20.20, 21.41, 24.05, 26.92, 30.24 };// 432Hz
            double[] frequencies = new double[notes.Length];

            for (int i = 0; i < notes.Length; i++)
            {
                int note = (notes[i] >> 12) & 0x0F; // Récupérer la hauteur de note (I)
                int octave = (notes[i] >> 8) & 0x0F; // Récupérer l'octave (J)

                if (note > 6) note = 6; // Limiter I à 6 (SI)

                frequencies[i] = (notes[i] & 0xFF00) == 0x0000 ? 0 : baseFrequencies[note] * Math.Pow(2, octave); // Si note == 0x00xy, fréquence = 0 (silence)
            }

            return frequencies;
        }

        static int[] ConvertNotesToDurations(int[] notes)
        {
            int[] durationMapping = { 10, 25, 50, 75, 100, 150, 200, 250, 333, 500, 1000, 1500, 3333, 5000, 10000, 60000 };
            int[] durations = new int[notes.Length];

            for (int i = 0; i < notes.Length; i++)
            {
                int durationIndex = notes[i] & 0x0F; // Récupérer la durée (L)
                durations[i] = durationMapping[durationIndex] / 2;
            }

            return durations;
        }

        static double[] ConvertNotesToVolumes(int[] notes)
        {
            double[] volumes = new double[notes.Length];

            for (int i = 0; i < notes.Length; i++)
            {
                int volumeIndex = (notes[i] >> 4) & 0x0F; // Récupérer le volume (K)
                volumes[i] = volumeIndex / 15.0; // Volume entre 0 et 1
            }

            return volumes;
        }
    }

    public static class Instruments
    {
        static Random random = new Random((int)DateTime.UtcNow.Ticks);

        public static double Sinus(double frequency, double volume, double time)
        {
            return volume * Math.Sin(2 * Math.PI * frequency * time);
        }
        public static double Saw(double frequency, double volume, double time)
        {
            double period = 1.0 / frequency;
            double t = time % period;
            return volume * (2.0 * (t / period) - 1.0);
        }
        public static double Square(double frequency, double volume, double time)
        {
            double period = 1.0 / frequency;
            double t = time % period;
            return volume * (t < period / 2.0 ? 1.0 : -1.0);
        }
        public static double Triangle(double frequency, double volume, double time)
        {
            double period = 1.0 / frequency;
            double t = time % period;
            return volume * (4.0 * Math.Abs(t / period - 0.5) - 1.0);
        }
        public static double Noise(double frequency, double volume, double time)
        {
            return volume * (2.0 * random.NextDouble() - 1.0);
        }
        public static double HybridTriangleGlissando(double frequency, double volume, double time)
        {
            double duration = 0.5;
            double startFrequency = frequency;
            double endFrequency = frequency;
            double currentFrequency = startFrequency + (endFrequency - startFrequency) * (time / duration);
            double triangle = 4.0 * Math.Abs((time * currentFrequency) % 1.0 - 0.5) - 1.0;
            double sine = Math.Sin(2 * Math.PI * currentFrequency * time);
            double noise = volume * (2.0 * random.NextDouble() - 1.0);
            return volume * (0.4 * triangle + 0.4 * sine + 0.2 * noise);
        }
    }
    public static class SampleRates
    {
        public static int High = 44100;
        public static int MediumUp = 22050;
        public static int MediumDown = 16000;
        public static int Low = 11025;
        public static int VeryLow = 8000;
    }
}
