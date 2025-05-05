using ILGPU;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILGPU.Runtime.Cuda;
using ILGPU.Runtime.OpenCL;
using System.Windows.Media.Media3D;

namespace Tooling
{
    public class GPU : IDisposable
    {
        static Context context;
        static Accelerator accelerator;
        static GPU()
        {
            // Initialize ILGPU
            context = Context.CreateDefault();
            accelerator = context.GetPreferredDevice(preferCPU: false).CreateAccelerator(context);
        }

        public static void Run(int[] _input, int[] _output, Func<int, int[], int> _func)
        {
            // Load the data.
            var deviceData = accelerator.Allocate1D(_input);
            var deviceOutput = accelerator.Allocate1D(_output);

            // load / compile the kernel
            var loadedKernel = accelerator.LoadAutoGroupedStreamKernel(
            (Index1D i, ArrayView<int> data, ArrayView<int> output) =>
            {
                output[i] = _func(i, _input);
            });

            // tell the accelerator to start computing the kernel
            loadedKernel((int)deviceOutput.Length, deviceData.View, deviceOutput.View);

            // wait for the accelerator to be finished with whatever it's doing
            // in this case it just waits for the kernel to finish
            accelerator.Synchronize();
        }

        public static void RunXY(int[,] _input, byte[] h_bitmapData, int[] _output, Func<int, int[], int> _func)
        {
            //int width = _input.GetLength(0);
            //int height = _input.GetLength(1)

            //// Load the data
            //MemoryBuffer2D<int, Stride2D.DenseY> canvasData = accelerator.Allocate2DDenseY<int>(new Index2D(width, height));
            //MemoryBuffer1D<byte, Stride1D.Dense> d_bitmapData = accelerator.Allocate1D<byte>(width * height * 3);

            //// load / compile the kernel
            //var loadedKernel = accelerator.LoadAutoGroupedStreamKernel(
            //(Index1D i, ArrayView<int> data, ArrayView<int> output) =>
            //{
            //    output[i] = _func(i, _input);
            //});

            //// tell the accelerator to start computing the kernel
            //loadedKernel((int)deviceOutput.Length, deviceData.View, deviceOutput.View);

            //// wait for the accelerator to be finished with whatever it's doing
            //// in this case it just waits for the kernel to finish
            //accelerator.Synchronize();

            //canvasData.Dispose();
            //d_bitmapData.Dispose();
        }

        public void Dispose()
        {
            accelerator.Dispose();
            context.Dispose();
        }
    }
}
