package hph.app.UITest;

import java.io.File;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.net.Socket;

import android.R.integer;
import android.R.string;
import android.app.ProgressDialog;
import android.content.Context;
import android.os.Handler;
import android.os.Message;
import android.widget.Toast;



public class FileRecieveEvent extends SocketRecieveEvent {
	
	private String saveFilePath;
	private InputStream inputStream;
	private FileOutputStream outputStream=null;
	private Socket socket;
	private int maxSize;
	private Context context;
	private String showString;
	public FileRecieveEvent(Context context)
	{
		maxSize=100;
		this.context=context;
	}
	
	public int getMaxSize() {
		return maxSize;
	}
	public void setMaxSize(int maxSize) {
		this.maxSize = maxSize;
	}
	public String getSaveFilePath() {
		return saveFilePath;
	}
	//private Thread work;
	public void setSaveFilePath(String saveFilePath) {
		this.saveFilePath = saveFilePath;
	}
	private ProgressDialog downloadProgressDialog;
	public ProgressDialog getDownloadProgressDialog() {
		return downloadProgressDialog;
	}
	public void setDownloadProgressDialog(ProgressDialog downloadProgressDialog) {
		this.downloadProgressDialog = downloadProgressDialog;
	}
	
	@Override
	public void EndRecieve(Socket client) {
		// TODO Auto-generated method stub
		try {
				this.socket=client;
				inputStream=client.getInputStream();
				outputStream=new FileOutputStream(new File(saveFilePath));
				if (downloadProgressDialog!=null) {
					downloadProgressDialog.setMax(maxSize);
					downloadProgressDialog.show();
			}
			new Thread(new Runnable() {  
				  
		        @Override  
		        public void run(){  
		        	try 
		        	{
						
			        	byte[] buffer=new byte[1024];
						int size = -1;
			        	while ((size = inputStream.read(buffer)) != -1) {
		        			outputStream.write(buffer,0,size);
							if (downloadProgressDialog!=null) {
								downloadProgressDialog.incrementProgressBy(1); 
							}
						}
			            // 在进度条走完时删除Dialog  
			        	downloadProgressDialog.dismiss(); 
			        	showToast("下载成功");
			            inputStream.close();
						if (outputStream!=null) {
							outputStream.close();
						}
			            DisposeSocket(socket);
		        	} 
		        	catch (Exception e) {
						// TODO: handle exception
		        		try {
							downloadProgressDialog.dismiss(); 
				            inputStream.close();
							if (outputStream!=null) {
								outputStream.close();
							}
				            DisposeSocket(socket);
				            showToast(e.getMessage()); 
						} catch (Exception e2) {
							// TODO: handle exception
						}
		 
					}
		        }  
		    }).start();  
            
		} catch (Exception e) {
			// TODO: handle exception
		}
	}
	private void showToast(String content) {
		showString=content;
        Message msg = msgHandler.obtainMessage();
        msg.arg1 = R.string.show_toast;  
        msgHandler.sendMessage(msg);  
	}
	
	private final Handler msgHandler = new Handler(){
        public void handleMessage(Message msg) {
                switch (msg.arg1) {
                case R.string.show_toast:
                	Toast.makeText(context,showString,Toast.LENGTH_SHORT).show();
                        break;
                default:
                        break;
                }
        }
};

}
