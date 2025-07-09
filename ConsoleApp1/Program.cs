using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

class Program
{
    static OutputDevice outputDevice;
    static Timer timer;
    static int currentStep = 0;
    static readonly object midiLock = new();
    static CancellationTokenSource cancellationTokenSource = new();
    static List<Thread> noteThreads = new();

    // Ajout de la durée (en ms) à chaque note
    static Dictionary<int, List<(int note, int instrument, int duration)>> sequence = new()
    {
        { 0, new List<(int, int, int)> { (60, 29, 50) } },
        { 1, new List<(int, int, int)> { (62, 29, 50) } },
        { 2, new List<(int, int, int)> { (64, 29, 50) } },
        { 3, new List<(int, int, int)> { (65, 29, 500) } },
    };

    static HashSet<int> initializedChannels = new();

    static void Main()
    {
        try
        {
            outputDevice = OutputDevice.GetByName("Microsoft GS Wavetable Synth");
            outputDevice.PrepareForEventsSending();

            double bpm = 120;
            double intervalMs = 60000.0 / bpm / 4;

            timer = new Timer(intervalMs);
            timer.Elapsed += OnStep;
            timer.AutoReset = true;
            timer.Start();

            Console.WriteLine("Lecture en cours... Appuyez sur une touche pour quitter.");
            Console.ReadKey();
        }
        catch (MidiDeviceException ex)
        {
            Console.WriteLine("Erreur à l'ouverture du périphérique MIDI : " + ex.Message);
        }
        finally
        {
            cancellationTokenSource.Cancel();

            foreach (var thread in noteThreads)
            {
                if (thread.IsAlive)
                    thread.Join();
            }

            timer?.Stop();
            timer?.Dispose();
            outputDevice?.Dispose();
        }
    }

    static void OnStep(object sender, ElapsedEventArgs e)
    {
        if (sequence.TryGetValue(currentStep, out var notes))
        {
            foreach (var (noteNumber, instrument, duration) in notes)
            {
                try
                {
                    int channelIndex = instrument % 16;
                    var channel = (FourBitNumber)channelIndex;

                    lock (midiLock)
                    {
                        if (!initializedChannels.Contains(channelIndex))
                        {
                            outputDevice.SendEvent(new ProgramChangeEvent((SevenBitNumber)instrument) { Channel = channel });
                            initializedChannels.Add(channelIndex);
                        }

                        //outputDevice.SendEvent(new NoteOnEvent((SevenBitNumber)noteNumber, (SevenBitNumber)100) { Channel = channel });
                        outputDevice.SendEvent(new NoteOnEvent((SevenBitNumber)60, (SevenBitNumber)100) { Channel = channel });
                    }

                    var thread = new Thread(() =>
                    {
                        try
                        {
                            Thread.Sleep(duration);
                            if (!cancellationTokenSource.IsCancellationRequested)
                            {
                                lock (midiLock)
                                {
                                    outputDevice.SendEvent(new NoteOffEvent((SevenBitNumber)noteNumber, (SevenBitNumber)0) { Channel = channel });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erreur NoteOff : " + ex.Message);
                        }
                    });

                    noteThreads.Add(thread);
                    thread.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur NoteOn : " + ex.Message);
                }
            }
        }

        currentStep = (currentStep + 1) % 16;
    }
}
