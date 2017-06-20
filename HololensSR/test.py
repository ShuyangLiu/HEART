
# A testing program for simulating multi threaded environment

import threading 
from subprocess import call

def callingClient(num):
	while 1:
		call ([".\\OverlappedNPClient\\Debug\\OverlappedNPClient.exe", num])


for x in range(1,10):
	t = threading.Thread(target=callingClient, args=str(x))
	t.start()

