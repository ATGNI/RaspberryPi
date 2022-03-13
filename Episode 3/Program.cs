using System.Device.Gpio;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

int pinSwitch = 23;
int pinLED = 24;

GpioController myController = new GpioController (PinNumberingScheme.Logical);

// Set up the two GPIO pins, one as input (switch) and the other as output (LED)

myController.OpenPin (pinSwitch, PinMode.InputPullUp);
myController.OpenPin (pinLED, PinMode.Output, PinValue.Low);

while (true) {
    if (myController.Read (pinSwitch) == PinValue.Low) {    // Switch has been pushed
        for (int loop = 0;loop < 3;loop++) {
            myController.Write (pinLED, PinValue.High);     // Switch on LED
            Thread.Sleep (300);
            myController.Write (pinLED, PinValue.Low);      // Switch off LED
            Thread.Sleep (300);
        }
    }
    Thread.Sleep (100);   // Wait 100ms and check the switch
}