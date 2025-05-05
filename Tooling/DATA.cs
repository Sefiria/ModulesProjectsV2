using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using Tooling;

namespace Tooling
{
    public class DATA
    {
        public byte[] data;
        public int LAYERS, W, H;
        public int convert(int layer, int x, int y) => layer * (H+1) * (W+1) + y * (W+1) + x;
        public int convert(int layer, Point xy) => convert(layer, xy.X, xy.Y);
        public byte this[int layer, int x, int y] { get => data[convert(layer, x, y)]; set => data[convert(layer, x, y)] = value; }
        public byte this[int layer, Point xy] { get => data[convert(layer, xy)]; set => data[convert(layer, xy)] = value; }
        public DATA(int LAYERS, int W, int H)
        {
            this.LAYERS = LAYERS;
            this.W = W;
            this.H = H;
            data = new byte[(LAYERS+1) * (H+1) * (W+1)];
        }
        public bool check_fail(int x, int y) => x < 0 || y < 0 || x > W || y > H;
        public bool check_pass(int x, int y) => !check_fail(x, y);
        public (int layer, int x, int y, int index) PointedData(int x, int y)
        {
            if (check_fail(x,y))
                return (0,x,y,0);
            int l = LAYERS - 1;
            while(l > 0)
            {
                if (this[l, x, y] > 0)
                    return (l, x, y, this[l, x, y]);
                l--;
            }
            return (0,x,y,0);
        }
        public (int layer, int x, int y, int index) PointedData(Point xy) => PointedData(xy.X, xy.Y);
        public int PointedLayer(int x, int y) => PointedData(x, y).layer;
        public int PointedLayer(Point xy) => PointedLayer(xy.X, xy.Y);
        public int Pointedindex(int x, int y) => PointedData(x, y).index;
        public int Pointedindex(Point xy) => Pointedindex(xy.X, xy.Y);
        public void Set(int l, int x, int y, int v) => this[l, x, y] = (byte)Maths.Range(0, byte.MaxValue, v);
        public void SetSquareWithLayer(int l, int x, int y, int v, int size, int layer_at_first_press, ref Queue<Point> draw_refresh_queue, bool force = true)
        {
            if (size == 1)
            {
                set_single_bylayer(l, x, y, v, layer_at_first_press, ref draw_refresh_queue, force);
                return;
            }

            for (int j= -size/2; j<size/2; j++)
                for (int i = -size / 2; i < size / 2; i++)
                    if(check_pass(x - i, y - j))
                        set_single_bylayer(l, x-i, y-j, v, layer_at_first_press, ref draw_refresh_queue, force);
        }
        public void SetCircleWithLayer(int l, int x, int y, int v, int diameter, int layer_at_first_press, ref Queue<Point> draw_refresh_queue, bool force = true)
        {
            if (diameter == 1)
            {
                set_single_bylayer(l, x, y, v, layer_at_first_press, ref draw_refresh_queue, force);
                return;
            }

            int radius = diameter / 2;
            for (int j = -radius; j < radius; j++)
                for (int i = -radius; i < radius; i++)
                    if (check_pass(x - i, y - j) && Maths.Distance((i, j).P()) <= radius)
                        set_single_bylayer(l, x-i, y-j, v, layer_at_first_press, ref draw_refresh_queue, force);
        }
        public void FillWithLayer(int l, int x, int y, int v, int layer_at_first_press, ref Queue<Point> draw_refresh_queue, bool force = true)
        {
            var pointed = PointedData(x, y);
            if (pointed.layer == v)
                return;
            List<Point> done = new List<Point>();
            int fake_stack = 0;
            bool opti_unstack_please = false;
            int opti_max_stack = 1000;
            int opti_stack_release = 100;
            Queue<Point> temp_draw_refresh_queue = new Queue<Point>();
            void set(int _x, int _y, int @case)
            {
                fake_stack++;
                if (done.Contains(new Point(_x, _y)))
                        goto back;
                done.Add(new Point(_x, _y));

                if (_x < 0 || _y < 0 || _x > W || _y > H)
                        goto back;
                var dt = set_single_bylayer(l, _x, _y, v, layer_at_first_press, ref temp_draw_refresh_queue, force);
                if (((force || v == 0) && pointed.layer != dt.layer) || (dt.layer == l && dt.index == v))
                        goto back;

                if (opti_unstack_please)
                {
                    if (fake_stack < opti_stack_release)
                        opti_unstack_please = false;
                    else
                        goto back;
                }
                else
                {
                    if (fake_stack > opti_max_stack)
                        opti_unstack_please = true;
                    if (opti_unstack_please)
                        goto back;
                }

                switch(@case)
                {
                    case 0: set(_x - 1, _y, @case); set(_x, _y - 1, @case); set(_x, _y + 1, @case); break;
                    case 1: set(_x + 1, _y, @case); set(_x, _y - 1, @case); set(_x, _y + 1, @case); break;
                }

            back:
                fake_stack--;
                Application.DoEvents();
                return;
            }
            set(x, y, 0);
            fake_stack = 0; done.Clear();
            set_single_bylayer(l, x, y, -1, layer_at_first_press, ref draw_refresh_queue, force);
            set(x, y, 1);
            foreach (var pt in temp_draw_refresh_queue)
                draw_refresh_queue.Enqueue(pt);
        }
        public void SetSquareWithDirection(int direction, int x, int y, int v, int size, int layer_at_first_press, ref Queue<Point> draw_refresh_queue)
        {
            if(size == 1)
            {
                set_single_bydir(direction, x, y, v, layer_at_first_press, ref draw_refresh_queue);
                return;
            }

            for (int j = -size / 2; j < size / 2; j++)
                for (int i = -size / 2; i < size / 2; i++)
                    if (check_pass(x - i, y - j))
                        set_single_bydir(direction, x - i, y - j, v, layer_at_first_press, ref draw_refresh_queue);
        }
        public void SetCircleWithDirection(int direction, int x, int y, int v, int diameter, int layer_at_first_press, ref Queue<Point> draw_refresh_queue)
        {
            if (diameter == 1)
            {
                set_single_bydir(direction, x, y, v, layer_at_first_press, ref draw_refresh_queue);
                return;
            }

            int radius = diameter / 2;
            for (int j = -radius; j < radius; j++)
                for (int i = -radius; i < radius; i++)
                    if (check_pass(x - i, y - j) && Maths.Distance((i, j).P()) <= radius)
                        set_single_bydir(direction, x - i, y - j, v, layer_at_first_press, ref draw_refresh_queue);
        }
        public (int old, int @new) calc_layer(int direction, int x, int y)
        {
            int old, layer;
            old = layer = PointedLayer(x, y);
            layer = layer == 0 ? LAYERS / 2 : new RangeValue(layer + direction, 0, LAYERS - 1).Value;
            return (old, (int)Maths.Range(0, LAYERS-1, layer));
        }
        public void set_single_bydir(int direction, int x, int y, int v, int layer_at_first_press, ref Queue<Point> draw_refresh_queue)
        {
            (int old, int @new) = calc_layer(direction, x, y);
            if (old != layer_at_first_press)
                return;
            this[old, x, y] = 0;
            this[@new, x, y] = (byte)v;
            draw_refresh_queue.Enqueue((x, y).iP());
        }
        public (int layer, int x, int y, int index) set_single_bylayer(int @new, int x, int y, int v, int layer_at_first_press, ref Queue<Point> draw_refresh_queue, bool force = true)
        {
            var dt = PointedData(x, y);
            var old = dt.layer;
            if (force && old != layer_at_first_press)
                return dt;
            if(force)
                this[old, x, y] = 0;
            this[@new, x, y] = (byte)v;
            draw_refresh_queue.Enqueue((x, y).iP());
            return dt;
        }
        public void Clear()
        {
            for (int L = 0; L < LAYERS; L++)
                for (int X = 0; X < W; X++)
                    for (int Y = 0; Y < H; Y++)
                        Set(L, X, Y, 0);
        }
    }
}
