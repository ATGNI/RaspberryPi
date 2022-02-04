# Instructions
I've used `$` to denote Raspberry Pi commands and `>` to indicate windows command prompt (cmd.exe) commands. 
Let me know if you spot any errors so that I can correct them. Make sure you start off in your home directory (in my case C:\Users\dave).
## Download/install the Raspberry Pi Imager here:
https://downloads.raspberrypi.org/imager/imager_latest.exe
## Run the Raspberry Pi Imager
```
Operating System: Raspberry Pi OS Lite (32-bit) - No Desktop
Storage: Select your Micro SD Card
Select "WRITE"
```
Windows "Eject" the Micro SD Card (if still mounted) and re-insert
## Create a `wpa_supplicant.conf` file 
Use your favourite Windows editor. Update with your Wifi details. 
```
ctrl_interface=DIR=/var/run/wpa_supplicant GROUP=netdev
country=GB
update_config=1

network={
 scan_ssid=1
 ssid="your_wifi_name"
 psk="your_password"
}
```
## Copy the file to the Micro SD-CARD
```
> COPY wpa_supplicant.conf H:
> H:
```
##  Create an empty SSH file to enable SSH after first boot
```
> COPY NUL SSH
> DIR
```
Should see the two files at the bottom\
Windows `Eject` the Micro SD Card \
Insert SD Card in Raspberry Pi and power on and wait for green light to quiesce
## Generate Private/Public keys
```
> MKDIR .SSH
> CD .SSH
> SSH-KEYGEN 
(select defaults)
> DIR
> TYPE ID_RSA.PUB
highlight/select the cryptic looking text. This is the public 'key'
Hit Return (this will do the equivalent of an Edit/Copy)
```
## Login to the raspberry pi
```
> SSH pi@raspberrypi.local
```
In response to fingerprint message.. type `Yes`\
Password is `raspberry`\
You're in! 
## Configure SSH to eliminate future password entry
```
$mkdir .ssh
$cd .ssh
$cat >authorized_keys
System Menu (top/Left), Edit/Paste to paste the previously copy'd "public key"
Enter/Return
^D     (control-D)
$exit
```
## Determine your Windows workgroup name
```
> SYSTEMINFO
```
Note the `Domain:` line for your PC's Workgroup name. Usually `WORKGROUP`
## SSH back into the Raspberry Pi and check no password
```
> SSH pi@raspberrypi.local
```
## Upgrade Pi to latest software
```
$sudo apt-get update
$sudo apt-get upgrade
```
## Install Samba/SMB (Windows File sharing)
```
$sudo apt-get install samba
```
## Configure SMB for the share

```
$sudo nano /etc/samba/smb.conf
```
On the first screen you should see the Workgroup name. Make sure it matches your Windows setting\
Add the following to the end of the file. \
Can get to end of file quickly with repeating `PAGE-DOWN`'s
```
[pishare]
   path=/home/pi
   writeable=yes
   create mask=0777
   directory mask=0777
   public=no
```
^O (control-o) To O-utput the file. Confirm the default filename\
^X (control-x) To eXit the editor
## Create a Samba/SMB user of Pi and set password
```
$sudo smbpasswd -a pi
set a password of "raspberry" (keep things simple)
$sudo reboot
```
Check SSH'ing back into Raspberry Pi. Note, no `.local` because joined Windows WORKGROUP
```
> SSH pi@raspberrypi
```
Windows thinks it's a different host, so Fingerprint message will appear again. Dismiss with `Yes`
## Map Network Drive
```
> NET USE P: \\raspberrypi\pishare /USER:pi raspberry /PERSISTENT:YES
```
## Unmap Network Drive (if you need to)
```
> NET USE P: /DELETE
```
