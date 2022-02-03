# Instructions
I've used `$` to denote Raspberry Pi commands and `>` to indicate windows command prompt (cmd.exe) commands. 
Let me know if you spot any errors so that I can correct them. Make sure you start off in your home directory (in my case C:\Users\dave).
The instructions assume you've installed VS Code and .NET on your Windows PC.
## Install .NET 6 on Raspberry Pi
```
$wget -O - https://raw.githubusercontent.com/pjgpetecodes/dotnet6pi/master/install.sh | sudo bash
```
The above command/script was expertly pieced together by Pete Gallagher [^1]
## Install the .NET debugger on the Raspberry Pi
Documented at the Microsoft web site [^2]
```
$curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/vsdbg
```
## Let's create a VS Code project
```
> dotnet new console -o MyFirstApp
> code MyFirstApp
```
Click Yes to create "Required Assets"
## Update the `tasks.json` file. 
Variation on a theme by Pete Gallagher[^1] to use Samba instead. Open the tasks.json file and add the following two tasks below the “watch” task, including that first comma! 
Change the `P:` drive to your mapped drive!
```
,
        {
            "label": "RaspberryPiPublish",
            "command": "sh",
            "type": "shell",
            "dependsOn": "build",
            "windows": {
                "command": "cmd",
                "args": [
                    "/c",
                    "\"dotnet publish -r linux-arm -o bin\\linux-arm\\publish --no-self-contained\""
                ],
                "problemMatcher": []
            }
            
        },
        {
            "label": "RaspberryPiDeploy",
            "type": "shell",
            "dependsOn": "RaspberryPiPublish",
            "presentation": {
                "reveal": "always",
                "panel": "new"
            },
            "windows": {
                "command": "copy bin\\linux-arm\\publish\\ -Destination P:${workspaceFolderBasename} -Recurse -Container:$false"
            },
            "problemMatcher": []
        }
```
## Update the `launch.json` file
Edit/Copy the following text, including the comma!
```
,
        {
            "name": ".NET Core Launch (remote)",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "RaspberryPiDeploy",
            "program": "dotnet",
            "args": ["/home/pi/${workspaceFolderBasename}/${workspaceFolderBasename}.dll"],
            "cwd": "/home/pi/${workspaceFolderBasename}",
            "stopAtEntry": false,
            "console": "internalConsole",
            "pipeTransport": {
                "pipeCwd": "${workspaceFolder}",
                "pipeProgram": "ssh",
                "pipeArgs": [
                    "pi@raspberrypi"
                ],
                "debuggerPath": "/home/pi/vsdbg/vsdbg"
            }
        }
```
Update `pipeArgs` with your user@host
## References:
[^1]: https://www.petecodes.co.uk/deploying-and-debugging-raspberry-pi-net-applications-using-vs-code/ (Pete Gallagher)
[^2]: https://docs.microsoft.com/en-us/dotnet/iot/debugging?tabs=self-contained&pivots=vscode
