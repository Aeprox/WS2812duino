using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LedControl
{
    enum LedModes
    {
        SOLID,
        RUNNING,
        RUNNING_RAINBOW
    }
    class LedEffects
    {
        private static byte numLeds = 60;
        private byte[] ledArray = new byte[numLeds*3];
        private SerialCom s;
        private Thread effectThread;


        public LedEffects(){}

        public void startEffect(LedModes mode, byte[] options) {
            this.effectThread = new Thread(
                () => fxThread(mode, options));
            effectThread.Start();
        }

        private void fxThread(LedModes mode, byte[] options)
        {
            switch (mode)
            {
                case LedModes.SOLID:
                    for (byte i = 0; i < numLeds; i++){
                        ledArray[i*3] = 255;
                        ledArray[i*3+1] = 180;
                        ledArray[i*3+2] = 70;
                    }
                    s.Send(ledArray);
                    break;
                case LedModes.RUNNING_RAINBOW:
                    for (; ; )
                    {
                        byte k;
                        for (byte j = 0; j < 255; j++)
                        {
                            // fill array with color wheel
                            k = j;
                            for (byte i = 0; i < numLeds; i++)
                            {
                                k++;
                                Array.Copy(Wheel(k), 0, ledArray, i*3, 3);
                            }
                            s.Send(ledArray);

                            Thread.Sleep(20);
                        }
                    }
                case LedModes.RUNNING:
                    for (;;)
                    {
                        for (byte i = 0; i < numLeds; i++){
                            Array.Clear(ledArray, 0, ledArray.Length);
                            Array.Copy(new byte[] { 255,0,0 }, 0, ledArray, i * 3, 3);
                            
                            s.Send(ledArray);
                            Thread.Sleep(20);
                        }  
                    }
                default:
                    throw new IndexOutOfRangeException("No such mode declared in LedEffects class"); ;

            }
        }


        // Input a value 0 to 255 to get a color value.
        // The colours are a transition r - g - b - back to r.
        private byte[] Wheel(byte WheelPos)
        {
            WheelPos = (byte)(255 - WheelPos);
            if (WheelPos < 85)
            {
                return new byte[3]{(byte)(255 - WheelPos * 3), 0, (byte)(WheelPos * 3)};
            }
            else if (WheelPos < 170)
            {
                WheelPos -= 85;
                return new byte[3]{0, (byte)(WheelPos * 3), (byte)(255 - WheelPos * 3)};
            }
            else
            {
                WheelPos -= 170;
                return new byte[3]{(byte)(WheelPos * 3), (byte)(255 - WheelPos * 3), 0};
            }
        }

        private void setAll(byte r, byte g, byte b)
        {
            for (byte i = 0; i < numLeds; i++)
            {
                ledArray[i * 3] = r;
                ledArray[i * 3 + 1] = g;
                ledArray[i * 3 + 2] = b;
            }
        }

        internal void startSerialCom(string comPort, int baudrate)
        {
            this.s = new SerialCom(comPort, baudrate);
        }

        internal void turnOff()
        {
            this.effectThread.Abort();
        }
    }
}
