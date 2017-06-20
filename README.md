# HololensSR

Forked from https://github.com/Parrishoot/HololensSR.git

----------------------
#### Update (06/20/2017)
- Add WinSock to OverlappedNPServer
- Seems we need to construct a Hololens app that can handle multiple client request at once
	- a multi-threaded WinSock Server for UWP???
- Add `.gitignore` file so the repository is more clean now

----------------------

#### Update (06/05/2017)
- The previous NPServer and NPClient programs did not work very well when we tested them today. The problem seems to be originated from multiple instances of NPClient using the same named pipe sending data to the same NPServer at the same time. Apparently, named pipe is designed to be used for one-to-one connection and there should only be one client sending data at a time.
- In this update, the original NPCLient and NPServer are changed to Overlapped version. Instead of having only one instance of pipe, it uses an array of pipe instances that can be used by multiple instances of clients concurrently. If the pipes are busy, the client will wait until any of the pipe finishes its previous work and reconnected.
- The testing program works (great!)
- But I need to use task manager to kill it (Ctrl-C doesn't work, I don't know why yet)
- The testing program creates 10 threads and runs infinite loop on each thread (continuously calling the client to send data)
- Still, need to do the socket connection thing


----------------------

#### Update (05/31/2017)

- Add Named Pipe Server and Client for Inter-processes communication (IPC) so that it only use one socket for the whole process
- Add Spike2 script for acquiring data from Power1401
- TODO:
	- Construct socket connection from NPServer to Hololens
	- Arm Control and reconsrtuction
	- Data Analysis inside Spike2 Scripts
