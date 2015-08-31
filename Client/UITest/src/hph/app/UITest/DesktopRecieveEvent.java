package hph.app.UITest;

import java.io.InputStream;
import java.net.Socket;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;

public class DesktopRecieveEvent extends SocketRecieveEvent{


	@Override
	public void EndRecieve(Socket client) {
		// TODO Auto-generated method stub
		try {
			InputStream inputStream=client.getInputStream();
			//InputStream inputStream=socket.getInputStream();
			byte[] bytes=new byte[0];
    		bytes=ByteToInputStream.input2byte(inputStream);
    		inputStream.close();
    		BitmapFactory.Options bmpFactoryOptions = AppConfig.getAppConfig().getBmpFactoryOptions();
    		Bitmap bitMap = BitmapFactory.decodeByteArray(bytes, 0, bytes.length,bmpFactoryOptions);
    		parameters=(Object)bitMap;
    		inputStream.close();
    		DisposeSocket(client);
		} catch (Exception e) {
			// TODO: handle exception
		}
		
	}
}
