# Raspberry Pi + .NET + Digital (GPIO) Input Pin
## Introduction
In this episode we'll look at using the Raspberry Pi to write some C# code to read real-time consumption data from a dumb power/electricity meter, effectively turning it into a smart meter.
## Recap
In previous episodes, our development tool was Visual Studio Code, a tool that can run on a variety of opertaing systems, although in my/our case we ran it on Windows. During the build we published to the Raspberry Pi platform (`linux-arm`), copying the files to the Raspberry Pi over a network share and then executing/debugging remotely. On reflection, this is a klunky method so I've been looking for alternatives. Options included;
### Visual Studio Code - Running on Raspberry Pi
This doesn't quite fulfill the criteria, as it means that the development is solely on the Raspberry Pi. It also means that you need to load up and install the Desktop UI just for this purpose. My preference is to have a bare bones Pi that is headless.
### Visual Studio Code - Remote Development
In this case you can load up the `Remote - SSH` extension and configure to run your development over an SSH connection to your Raspberry Pi. Effectively using Visual Studio Code as a `thin client` to a backend running on the Raspberry Pi. The challenge here is the SSH configuration, ports, etc. 
### Visual Studio Code - Remote Tunnel
In December 2022, Microsoft brought out Version 1.74 of Visual Studio Code and also made the `remote-tunnel` extension available. Again, this introduces the concept of using Visual Studio Code on your Windows system as a `thin-client` communicating over a `tunnel` to something called the VS Code Server. You can think of the VS Code Server as a Command Line Interface (CLI) version of Visual Studio Code. Functionally, the development environment will reside on the target machine (the Raspberry Pi), including extensions, and all that is required on the Windows machine is the `remote-tunnel` extension. There is a caveat to using the tunnel and that is, you need a GitHub account, however, a couple of benefits ensue; 1) you no longer need network connectivity between your Windows and Raspberry Pi, the two can be seperated, for example, maybe you're travelling, and 2) you can use a web based version of Visual Studio Code. In summary, this is what remote tunneling looks like
![picture](https://code.visualstudio.com/assets/docs/remote/vscode-server/server-arch-latest.png)

