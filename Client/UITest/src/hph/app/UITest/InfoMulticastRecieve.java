package hph.app.UITest;

import java.net.DatagramPacket;
import java.net.InetAddress;
import java.net.MulticastSocket;

public class InfoMulticastRecieve extends MulticastRecieve {

	@Override
	public void Recieve(MulticastSocket socket) {
		try {
			// TODO Auto-generated method stub
			byte[] rev = new byte[512];  
			DatagramPacket packet;  
			
	        packet = new DatagramPacket(rev, rev.length);  
	        socket.receive(packet);  
	        //String addr = socket.getInetAddress().getHostAddress();
	        //socket.getReuseAddress()
	        //Log.v(TAG, "get data = " + new String(packet.getData()).trim());    //不加trim，则会打印出512个byte，后面是乱码  
	        String msgString= new String(packet.getData());
	        InetAddress address = packet.getAddress();
	        MulticastInfo info=new MulticastInfo();
	        info.messageString=msgString;
	        info.IP=address.getHostAddress();
	        recieveData=info;
		} catch (Exception e) {
			// TODO: handle exception
		}
		
       
	}

}
