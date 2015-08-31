package hph.app.UITest;

import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

public class SimpleSocketHelper {

	public static void sendString(String host,int port,String dataString) {
		sendString(host, port, dataString,null);
	}
	public static void sendString(String host,int port,String dataString,SocketRecieveEvent event)
    {
    	try {
			Socket client=new Socket(host,port);
			client.setSoTimeout(500);
    		//PrintWriter out = new PrintWriter(client.getOutputStream(),true);
    		OutputStream outputStream=client.getOutputStream();
    		byte[] data=dataString.getBytes("utf-8");
    		outputStream.write(data);
    		//InputStream inputStream=client.getInputStream();
    		outputStream.flush();
    		client.shutdownOutput();
    	  	if (event!=null) {
				event.EndRecieve(client);
			}
    	  	if (client!=null&& !client.isConnected() && !client.isClosed()) {
    	  		outputStream.close();
        	  	//inputStream.close();
        	  	client.close();
			}
		} catch (Exception e) {
			// TODO: handle exception
		}
    }
}
