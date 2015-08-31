package hph.app.UITest;

import java.io.InputStream;
import java.net.Socket;

public abstract class SocketRecieveEvent {
	protected Object parameters;
	
	public abstract void EndRecieve(Socket client);
	public Object getData() {
		return parameters;
	}
	public void DisposeSocket(Socket socket){
		try {
			if (socket!=null) {
				socket.close();
			}
		} catch (Exception e) {
			// TODO: handle exception
		}
	}
}
