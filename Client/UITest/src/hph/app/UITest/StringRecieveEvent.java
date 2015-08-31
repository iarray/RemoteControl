package hph.app.UITest;

import java.io.InputStream;
import java.net.Socket;

public class StringRecieveEvent extends SocketRecieveEvent {

	private String charSet;
	public String getCharSet() {
		return charSet;
	}
	public void setCharSet(String charSet) {
		this.charSet = charSet;
	}
	
	public StringRecieveEvent()
	{
		charSet="ASCII";
	}
	@Override
	public void EndRecieve(Socket client) {
		// TODO Auto-generated method stub
		try {
				InputStream inputStream=client.getInputStream();
				byte[] bytes=new byte[0];
				bytes=ByteToInputStream.input2byte(inputStream);
				parameters=new String(bytes,charSet);	
				inputStream.close();
				DisposeSocket(client);
		} catch (Exception e) {
			// TODO: handle exception
		}
		
	}

}
