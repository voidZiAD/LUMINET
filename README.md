# ![LUMINET](https://media.discordapp.net/attachments/1167902642815643678/1186243414979784794/LUMINET.png?ex=65928a45&is=65801545&hm=a50fefaa7d73282b5e71745e3817281957b6901b0b73573002833578370f09eb&=&width=200&height=200)
LUMINET: An (unfinished) open-source automated VPN serving Windows Users (and possibly Linux on Wine) using the WireGuard Protocol.

## How it works:

This is an *automated* VPN that uses **CefSharp** (to bot servers) on Windows Forms (which you don't see on the VPN) to create temporary VPN servers for the following locations:

![image-removebg-preview](https://github.com/voidZiAD/LUMINET/assets/84229419/8b9a67f2-a8d2-48a5-b6d2-49a706fab8db)

Every server that is created using the bot lasts **4 days**, and the VPN will save that information in case you try to connect to the same location after 4 days (which by then the server would have expired, and the VPN would have to re-create a server). The VPN only creates servers when you don't already have a server (which is active) saved on your computer (via a .conf file in your **%AppData%**).

### When attempting to connect, the following process occurs:

if .conf file doesn't exist -> Create server -> Download .conf file -> ~~connect~~ **(CONNECTION NOT IMPLEMENTED YET. [Learn more](https://github.com/voidZiAD/LUMINET/blob/main/WhyIsWireGuardNotImplementedYet.md))**\
if .conf file exists -> Check how many days ago the server was created -> if server is still valid -> ~~connect~~ **(CONNECTION NOT IMPLEMENTED YET. [Learn more](https://github.com/voidZiAD/LUMINET/blob/main/WhyIsWireGuardNotImplementedYet.md))**



## What is finished/not finished:

- ✅ Automation (using [sshs8 servers](https://sshs8.com/) && [CefSharp](https://cefsharp.github.io))
- ✅ UI (finished for the current version but can have a few updates (e.g. Settings page))
- ❌ WireGuard integration (it's literally impossible to find a WireGuard library for C#)
- ✅ Back-end processes/systems

## Current UI:

![image-removebg-preview (1)](https://github.com/voidZiAD/LUMINET/assets/84229419/73ff0615-7735-4fc8-b796-dbc5765e23bb)

## Benefits of using LUMINET (after it's release 😅):

- 🎥 Netflix Support
- ⚡ 1GB/s Speed (from server + WireGuard's speed itself 😮‍💨)
- ✅ Rapid reconnection
- 🔐 High security

## Future Location Additions:

- 🔓 No Captcha:
  - United States (USA) from [sshOcean](https://sshocean.com/)
- 🔐 With Captcha (all from [vpnjantit](https://www.vpnjantit.com/)):
  - Asia Locations:
      - Azerbaijan
      - Bahrain
      - Bangladesh
      - Cambodia
      - Hong Kong
      - India
      - Indonesia
      - Japan
      - Kazakhstan
      - Malaysia
      - Pakistan
      - Philippines
      - Saudi Arabia
      - Singapore
      - South Korea
      - Thailand
      - Turkey
      - United Arab Emirates
      - Vietnam
  - Europe Locations:
      - Belgium
      - Bulgaria
      - Czech Republic
      - Estonia
      - Finland
      - France
      - Germany
      - Hungary
      - Ireland
      - Italy
      - Luxembourg
      - Moldova
      - Netherlands
      - Poland
      - Portugal
      - Romania
      - Russia
      - Spain
      - Sweden
      - Switzerland
      - Ukraine
      - United Kingdom
  - America Locations:
      - Argentina
      - Brazil
      - Canada
      - Chile
      - Mexico
      - United States
  - Other:
      - Australia

All those locations will be available, hopefully for **free** depending on if this VPN is still cost-free to finish on our side or not. I will be working to assure that we can bypass captcha on the WireGuard servers site in order to allow more locations to be accessed on the VPN. However, my main focus right now is to integrate WireGuard with C#, as that's currently the hardest thing to do on this project considering there are no reference Libraries or code anywhere on the internet that allows WireGuard to be integrated with C#.

# Contribution

I would greatly appreciate any form of contribution if it benefits the project. This is a non-profit project created to make VPNs easier to use, cost-free, fast, secure, and automated.

