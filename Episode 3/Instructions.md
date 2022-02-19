# Create and run a C# .NET application
We will enhance the application we created in `Part 2` and add some additional code that will allow you to read the state of a switch and flash the LED when it is push/depressed. If you create a new project, then you must remember to make all the modifications to the `launch.json` and `tasks.json` files that were described in `Part 2`. It demonstrates the principles of using both an `input` GPIO pin and an `output` GPIO pin. Within these instructions, I've used `$` to denote Raspberry Pi commands and `>` to indicate windows command prompt (cmd.exe) commands. 
Let me know if you spot any errors so that I can correct them. Make sure you start off in your home directory (in my case C:\Users\dave).
The instructions assume you've completed the previous two steps (`Part 1` and `Part 2`).
## Load up the Visual Code project
```
> code MyFirstApp
```
Select the Program.cs file and you should see something simple like this
```
1    // See https://aka.ms/new-console-template for more information
2    Console.WriteLine("Hello, World!");
```
## Configure the application for GPIO
We now need to add some of the GPIO code, but before we can do that we need to add the `package` that contains the GPIO library. To do this, click on the `Terminal` tab in the bottom window and type
```
dotnet add package System.Device.Gpio
```
You should see something like this
```
Determining projects to restore...
  Writing C:\Users\dave\AppData\Local\Temp\tmp6661.tmp
info : Adding PackageReference for package 'System.Device.Gpio' into project 'C:\Users\dave\MyFirstApp\MyFirstApp.csproj'.
info :   GET https://api.nuget.org/v3/registration5-gz-semver2/system.device.gpio/index.json
info :   OK https://api.nuget.org/v3/registration5-gz-semver2/system.device.gpio/index.json 401ms
info : Restoring packages for C:\Users\dave\MyFirstApp\MyFirstApp.csproj...
info : Package 'System.Device.Gpio' is compatible with all the specified frameworks in project 'C:\Users\dave\MyFirstApp\MyFirstApp.csproj'.
info : PackageReference for package 'System.Device.Gpio' version '2.0.0' added to file 'C:\Users\dave\MyFirstApp\MyFirstApp.csproj'.
info : Committing restore...
info : Generating MSBuild file C:\Users\dave\MyFirstApp\obj\MyFirstApp.csproj.nuget.g.targets.
info : Writing assets file to disk. Path: C:\Users\dave\MyFirstApp\obj\project.assets.json
log  : Restored C:\Users\dave\MyFirstApp\MyFirstApp.csproj (in 93 ms).
```
Excellent, we've now added the GPIO library so it will understand when we reference our GPIO pins. Alongside the `Terminal` tab, is `Debug Console` and `Output`. Click on `Output` which is the default. 
## Add GPIO code to the application
Insert a line at the top of the file to say we will be `using` the library
```C#
using System.Device.Gpio;

```
We now need to add some addition lines, so copy the following code and paste it at the end of the `Program.cs` file.
```C#

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
            Task.Delay (100);
            myController.Write (pinLED, PinValue.Low);      // Switch off LED
            Task.Delay (400);
        }
    }
    Task.Delay (100);   // Wait 100ms and check the switch
}
```

