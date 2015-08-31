package hph.app.UITest;

import java.net.MulticastSocket;

public abstract class MulticastRecieve {
	public Object recieveData;
	public abstract void Recieve(MulticastSocket socket);
}
