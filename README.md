# ViRtuGlove 0.9
This goal of this project to create a haptic feedback glove that can be used with consumer virtual reality (VR) systems. Presently, the project employs the following core features:
* 3 D0F rotational acceleration tracking for each digit (and for the hand to serve as a reference)
* anti-contractile haptic feedback for each digit
* assessment of contractile resistance against the haptic feedback system

Currently, the project does not have full integration with a VR system, but does include functioning (albeit in need of minor debugging) systems for exchanging sensor and actuator signals with a host computer. Please see the /src folder for my Arduino sketch and a companion script for Unity3D. These scripts provide a foundation for integration with a variety of VR interfaces.

![alt text][pic1]

[pic1]: https://github.com/kanyukaz/ViRtuGlove/blob/master/img/prototype.jpg "VR Glove"

Repository Contents
============
Here's where you'll provide a map of all the top-level contents (where applicable):

* **/documentation** - documentation for this project and its predecessor
* **/hardware** - models for the unused part prototypes and circuit schematics to summarize the electronic boards that I have built
* **/img** - images for the readme, including a product photo and circuit schematic diagrams.
* **/libraries** - the libraries for presently used versions of Adafruit sensor libraries (keeps the code functional in case current versions are not available in the future, and future versions are not backwards compatible).
* **/scr** - the Arduino sketch, the companion script for Unity3D, and code used for debugging

* **LICENSE** - The license file.
* **README.md** - The file you're reading now! :-D

Requirements and Materials
============
Dependencies:
* Adafruit Unified Sensor library V1.0.3
* Adafruit L3GD20 U Sensor library V1.01

Both of these are available through Arduino IDE's built-in library manager, but backups are included in the /libraries section

Bill of Materials:
* 1 x Arduino MEGA
* 1 x VR-capable computer
* 5 x Adafruit L3GD20H gyroscope breakouts
* 1 x Adafruit 9D0F sensor breakouts
* 5 x TOWERPRO SG-5010 servomotors
* 5 x Adafruit INA169 current sensor breakouts
* 1 x TCA9548A I2C multiplexer
* 1 x 180 cm derailleur cable
* 1 x tight-fitting elastic glove 
* L x Velcro
* L x cushion foam
* L x hot glue 
* L x flexible perfboard
* L x ribbon cable
* L x jump-wire
* L x header pins, male and female

L - liberal amounts of; exact amount depends on the assembler's skill in assembling the project and individual design variations. 

Bill of Tools:
* wire stripper
* wire cutter
* hot glue gun
* soldering iron 

Build Instructions
==================
Below, I have described the way I have build the project. Variant access to resources, time, and alternative materials leaves flexibility in many of the methods used. Please note that most of the build procedure is an excerpt from the term project write-up paper which is included in the /documentation directory. 

**Overall structure** 

The glove's construction consists of three main units: the microcontroller unit, which is mounted on the user's upper arm; the servomotor unit, which is mounted on the user's lower arm; and the hand unit, which is mounted on the user's hand. The microcontroller unit, held to the user's arm with an elastic armband, includes the Arduino MEGA microcontroller with a custom shield that facilitates a one-cable link to the electronic components of the other two units. The servomotor unit, held to the user's arm by a short sleeve includes the servomotor control board which powers and controls the servomotors and assesses the current drawn by each. Additionally, the servomotor unit has attachment points for each servomotor. Lastly, the hand unit, held to the user's hand by a tight-fitting work glove includes a gyroscope management board, a haptic feedback application bracket, and a haptic feedback harness. Below, I will describe each of these units, the connectivity between them, and the servomotor subunits in more detail.

I built the electronic boards described on the base of consumable flexible perfboard material. I used standard male and female header pins to build cable connectors. All electronic connections other than the cable connectors were reinforced with solder. All structural connections were reinforced with hot glue.

**The servomotor subunits** 

The haptic feedback mechanism employed by the prototype uses servomotors to pull the user's digits via metal cables opposite to the digits' contractile direction. The system is powered by a set of five TOWERPRO SG-5010 servomotors, one for each digit. To harness the power of the servomotors, a cable was attached to the far end of each servo's arm, and channelled through one of the built-in mounting holes. A 180 degree rotation of the servo arm results in a 4 cm range of motion for the cable. To allow flexibility in mounting the servos, the force is conveyed through a 30 cm derailleur cable. To summarize, a servo subunit consists of a servomotor, a derailleur cable with one end loose and the other end attached using hot glue to the mounting hole that serves as a cable channel, and a cable that is attached to the arm and passes through the derailleur cable. In the assembled prototype, the loose end of the derailleur cable is glued to the eyelets of the haptic feedback application bracket on the hand unit, and the servos are flexibly mounted at the servomotor unit using Velcro. 

**Inter-unit connectivity and power**

The microcontroller unit serves as the bridge between the transducers, the power source, and the host. The Arduino MEGA is connected to the host computer using a USB type A/B cable, and it is connected to the power adapter through the barrel-shaped power jack.

The electronic components of the three units are connected using two propitiatory ribbon cables. The servomotor subunit is connected to the microcontroller subunit using a 15-pin male-male ribbon cable, which I will refer to as cable A for the rest of the paper. Cable A has 5 PWM lanes (used for servo control), 5 analogue lanes (used for ammeter readings), SCL and SDA lanes for I2C communication (for gyros), a single free digital lane for future expansion, a Vin lane, and a ground lane. The connector for cable A is L-shaped to avoid the possibility of accidentally plugging it in the wrong orientation. The microcontroller subunit and servomotor subunits have corresponding female connectors for cable A. The custom shield for the Arduino MEGA serves as the microcontroller's adapter for cable A. The electronic components of the hand unit are connected to those on the servomotor unit (and by extension, through cable A to the microcontroller unit) via a 5-pin male-male ribbon cable which I will refer to as cable B. Cable B has SCL and SDA lanes for I2C communication, a single free digital lane for future expansion, a Vin lane, and a ground lane. Like cable A, it is L-shaped and has corresponding female connectors on the units that it links. The servomotor unit serves in part as a pass-through for the microcontroller's connection to the gyros. I formed the connectors using ribbon cable, male and female headers (where appropriate). Then I reinforced and insulated the male connectors using hot glue. It should also be noted that the Vin and ground connections make up system-wide power and ground rails. 

**Hand unit**

The hand unit is held to the user's hand through an elastic work glove. I chose a tight-fitting glove for the project to ensure close adherence to a wide range of hand sizes. The tips of each fingers are reinforced with a thimble fashioned out of wire and hot glue. Cables attached to the thimbles are threaded through the glove along the outer edge of each digit so they could be attached to the servomotor subunits' cables at the haptic feedback bracket. This local cable system makes up the haptic feedback application harness. Velcro mounting points for gyroscopes are placed on middle phalanges of each finger, and the proximal phalanx of the thumb. Positioning the gyroscopes on these joints best allows for extrapolation of finger conformation with a single sensor per digit. The Velcro haptic feedback bracket attachment point spans the back of the glove. The extensive use of Velcro throughout the arm  hand unit allows easy serviceability and inter-user adjustment. 

The haptic feedback bracket is a metal structure with a flat base parallel to the user's metacarpals and eyelets that serve as attachment points for the loose ends servomotor subunits' derailleur cables. The end of the bracket closest to the phalanges has five functional eyelets through which the distal ends of the servo subunits' derailleur cables are threaded. Here,  adjustable links between the servo-attached cables and harness-attached cables is made by twisting together the ends of each. Thus, the servos are mechanically linked to the tips of the user's fingers, completing the haptic feedback system. 
	
Early design of the project relied on the possibility of 3D printing structural pieces for the hand unit. These include gyro mounts with lobes when one can drill pass-throughs for haptic feedback harness cable for each digit. Additionally, I planned to 3D print a lightweight, plastic haptic feedback bracket with a shape moulded by the user's hand. When printing initial prototypes for the gyro mounts revealed the limited availability of 3D printers, the design was changed to what can be seen in the final product, and no further part prototypes were made.
	
I used hot glue to mount the gyroscope communication board atop the haptic feedback bracket. The schematic diagram for the board can be found below. The board consists of five Adafruit L3GD20 3DOF gyroscope breakout boards permanently connected to the communication board through 4-line ribbon cables. Each such cable has SCL and SDA lanes for I2C communication, a Vin lane, and a ground lane. Additionally, an Adafruit 9DOF sensor breakout board which includes a L3GD20 gyroscope has been mounted directly on the gyroscope communication board. It can serve as a reference point for the other gyroscopes.  Its additional accelerometers have not been implemented in this project, but remain an open for future development. The electronic components implemented on the gyroscope communication board use I2C for communicating with the microcontroller. Because all L3GD20H gyroscopes used share the same I2C address, they are managed with an Adafruit TCA9548A I2C multiplexer digital breakout board. Each sensor's I2C pins are linked to respective corresponding pins on the multiplexer. All of the breakout boards use the shared Vin and ground rails. As previously mentioned, this unit connects to the other units through cable B.

![alt text][pic2]

[pic2]: https://github.com/kanyukaz/ViRtuGlove/blob/master/img/GYRO.png "Hand unit board circuit schematic"

**Servomotor unit**

To facilitate mounting on the lower arm, a short sleeve fashioned out of another elastic work glove is used as the structural base for the servomotor unit. I mounted the servomotor control board on a piece of cushion foam with a base that is shaped to follow the curvature of the user's arm. This strategy enhances structural stability when mounting the large board to a user's body. Throughout the circumference of the sleeve, I placed Velcro attachment points for the servomotors. When the unit is assembled, the servomotors are connected to the appropriate headers on the servomotor control board.
	
The servomotor control board integrates two key functions of the prototype - management of haptic feedback and assessment of resistive contractile strength. The schematic diagram for the board can be found below. The board has five three-pin male headers common among shields made for servomotors. Each header has ground pin, a Vin pin, and a PWM communication pin. The five INA169 current sensors serve as a proxy between the servo headers' Vin pins and the global Vin power rail. This allows them to monitor the current drawn by the servomotors. The current sensors have Vin, ground, and analog signal pins, as well as the pins between which the current is measured and travels to the servos headers. The servo header PWM pins and the current sensors' analogue pins make a direct and exclusive connection with the the corresponding pins on the microcontroller through the cable A link. The board additionally serves as a pass-through to the hand unit, to which it is connected via cable B.

![alt text][pic3]

[pic3]: https://github.com/kanyukaz/ViRtuGlove/blob/master/img/SERVO.png "Servomotor unit board circuit schematic"

**Microcontroller unit**

The Arduino MEGA microcontroller board is mounted to the user's upper arm with an adjustable elastic armband. The outward surface of the custom-made shield consists solely of the female connector for cable A. The schematic diagram for the shield can be found below. On the flip-side, the female connector's pins are connected as follows: five analogue lines used for current sensors are connected to analogue pins 0-4; five PWM lines used for servo control and the expansion digital pin are connected to digital pins 2-6 and 7 respectively; and the Vin rail and ground rail lines are connected to Vin and ground pins respectively. As explained before, this unit is used for handling communication to the host via USB, managing the supply of power, and mediating the transducers. I have included further information on the firmware used by the microcontroller in the sections below. 
  
![alt text][pic4]

[pic4]: https://github.com/kanyukaz/ViRtuGlove/blob/master/img/MICRO.png "Microcontroller unit shield circuit schematic"

Usage
=====
**Assembly**

Thread your right hand and arm through the microcontroller unit's armband, followed by the servomotor unit's sleeve, and the hand unit's glove. Adjust the armband and the sleeve for a snug, yet comfortable fit. Then, with the digits fully contracted and the haptic feedback cables in a fully extended position, adjust the position of the haptic feedback bracket until the cables are lightly tensioned. When disconnected from any power source, the servo arms can be manually adjusted to make this possible. If necessary, adjust the lengths of the cables at the link between the servo subunit's and haptic feedback harness cables. After this, reposition the gyroscopes so they are in the middle of the middle phalanges of each finger, and the proximal phalanx of the thumb. Finally, ensure that cables A and B are firmly connected and the microcontroller is connected to the host and the power source. Depending on the setup, USB and mains extension cords may be desirable. To implement the glove with a VR system, use a Velcro strap to tether a VR controller anywhere on the lower arm where it will be visible to the tracking systems.  The project becomes active and starts communicating with the host once the serial port to the host is open.

**Unity3D interface script**

The Unity3D companion script offers a rudimentary graphic user interface, allowing the user to view the numeric signals the host receives from the glove and control the servomotors via sliders. Variables that handle each of these are publicly available to other scripts, and the script already takes references to joints of an OpenVR hand model, making it ripe for further development of a VR interface. 

Team
=====
The build team consists of: 
* Zakhar Kanyuka -- I designed and built the project

A special thanks is to the University of Regina Department of Computer Science for providing me with the resources for the project and helping me extensively with troubleshooting. 

Credits
=======
* The i2c scanner found in /src/debugging was adapted from riyas-org; the original can be found at: https://gist.github.com/riyas-org/a394774cd605789964530612c3fb3dbc


