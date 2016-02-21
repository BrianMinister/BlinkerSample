using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace Blinker
{
    //enum LED_SEGMENT : byte
    //{
    //    TOP_LEFT=0,
    //    CENTER=1,
    //    BOTTOM_LEFT=2,
    //    BOTTOM = 3,
    //    TOP = 4,
    //    TOP_RIGHT = 5,
    //    DECIMAL = 6,
    //    BOTTOM_RIGHT = 7
    //}

    public class Program
    {
        public static void Main()
        {
            // write your code here
            OutputPort[] led = null;
            OutputPort speaker;
            InputPort btn;
            SetEnvironment(out led, out btn, out speaker);

            CodeLoop(led, btn, speaker);

        }

        private static void SetEnvironment(
            out OutputPort[] led,
            out InputPort btn,
            out OutputPort speaker)
        {
            led = new OutputPort[] {
                new OutputPort(Pins.GPIO_PIN_D13, false),
                new OutputPort(Pins.GPIO_PIN_D12, false),
                new OutputPort(Pins.GPIO_PIN_D11, false),
                new OutputPort(Pins.GPIO_PIN_D10, false),
                new OutputPort(Pins.GPIO_PIN_D9, false),
                new OutputPort(Pins.GPIO_PIN_D8, false),
                new OutputPort(Pins.GPIO_PIN_D7, false),
                new OutputPort(Pins.GPIO_PIN_D6, false),
                new OutputPort(Pins.GPIO_PIN_D5, false),
                new OutputPort(Pins.GPIO_PIN_D4, false),
                new OutputPort(Pins.GPIO_PIN_D3, false),
                new OutputPort(Pins.GPIO_PIN_D2, false),
                new OutputPort(Pins.GPIO_PIN_D1, false),
                //new OutputPort(Pins.GPIO_PIN_D0, false),
                new OutputPort(Pins.ONBOARD_LED, false)
            };

            speaker = new OutputPort(Pins.GPIO_PIN_D0, false);

            btn = new InputPort(Pins.ONBOARD_BTN, false, ResistorModes.Disabled);
        }

        private static void CodeLoop(OutputPort[] led, InputPort btn, OutputPort speaker)
        {
            byte[][] charMap = InitChars();
            int x = 0;
            int dx = 1;

            beep(speaker);

            while (!btn.Read())
            {

                LedMgr(led, charMap[x]);

                Thread.Sleep(1000);
                
                x += dx;
                if (x == 0 || x == 15) {
                    dx = -dx;
                }
            }
            beep(speaker);

            Char_Clear(led);
        }

        private static void beep(OutputPort speaker)
        {
            for (int i = 300; i >= 0; i--)
            {
                speaker.Write(true);
                Thread.Sleep(1);
                speaker.Write(false);
            }
        }

        private static void Char_Clear(OutputPort[] led)
        {
            byte[] segs = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            LedMgr(led, segs);
        }

        private static void LedMgr(OutputPort[] led, byte[] segments)
        {
            for(int i = 0; i < segments.Length; i++)
            {
                led[i].Write(segments[i] == 0 ? false : true);
            }
        }

        private static byte[][] InitChars()
        {
            return new [] 
            {
                new byte[] { 1, 0, 1, 1, 1, 1, 0, 1, 0},
                new byte[] { 0, 0, 0, 0, 0, 1, 0, 1, 0 },
                new byte[] { 0, 1, 1, 1, 1, 1, 0, 0, 0 },
                new byte[] { 0, 1, 0, 1, 1, 1, 0, 1, 0 },
                new byte[] { 1, 1, 0, 0, 0, 1, 0, 1, 0 },
                new byte[] { 1, 1, 0, 1, 1, 0, 0, 1, 0 },
                new byte[] { 1, 1, 1, 1, 1, 0, 0, 1, 0 },
                new byte[] { 0, 0, 0, 0, 1, 1, 0, 1, 0 },
                new byte[] { 1, 1, 1, 1, 1, 1, 0, 1, 0 },
                new byte[] { 1, 1, 0, 1, 1, 1, 0, 1, 0 },
                new byte[] { 1, 1, 1, 0, 1, 1, 0, 1, 0 },
                new byte[] { 1, 1, 1, 1, 0, 0, 0, 1, 0 },
                new byte[] { 1, 0, 1, 1, 1, 0, 0, 0, 0 },
                new byte[] { 0, 1, 1, 1, 0, 1, 0, 1, 0 },
                new byte[] { 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                new byte[] { 1, 1, 1, 0, 1, 0, 0, 0, 0 },
                new byte[] { 0, 0, 0, 0, 0, 0, 1, 0, 0 }
            };
        }

    }
}
