package hph.app.UITest;


import hph.app.UITest.AppConfig.ConnectModel;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.Gravity;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.Window;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.PopupWindow;
import android.widget.Toast;

public class UITestActivity extends Activity implements OnClickListener{
	private AppConfig config;
	private Button connectButton;
	/** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        this.requestWindowFeature(Window.FEATURE_NO_TITLE);//去掉标题栏
        setContentView(R.layout.main);
        config=AppConfig.getAppConfig();
        AppConfig.loadOptions(UITestActivity.this);
        //findHost();
        //config.setServerHost("10.0.2.2");
        //config.setServerPort(9999);
//        if (config.getServerHost()!="") {
//        	 Toast.makeText(this,config.getServerHost()+":"+config.getServerPort(),Toast.LENGTH_SHORT).show();	
//		}
        ImageButton desktopButton=(ImageButton)findViewById(R.id.btnDesktop);
        ImageButton fileButton=(ImageButton)findViewById(R.id.btnFileManager);
        ImageButton optionsButton=(ImageButton)findViewById(R.id.btnOpstions);
        ImageButton pluginButton=(ImageButton)findViewById(R.id.btnPlugins);
        connectButton=(Button)findViewById(R.id.btnConnetc);
        desktopButton.setOnClickListener(this);
        fileButton.setOnClickListener(this);
        optionsButton.setOnClickListener(this);
        pluginButton.setOnClickListener(this);
        connectButton.setOnClickListener(this);
    }
    
    private long exitTime = 0; 
    @Override 
    public boolean onKeyDown(int keyCode, KeyEvent event) { 
	    if(keyCode == KeyEvent.KEYCODE_BACK && event.getAction() == KeyEvent.ACTION_DOWN){ 
	    if((System.currentTimeMillis()-exitTime) > 2000){ 
		    Toast.makeText(getApplicationContext(), "再按一次退出程序", Toast.LENGTH_SHORT).show(); 
		    exitTime = System.currentTimeMillis(); 
	    } else { 
	    	finish(); 
	    	System.exit(0); 
	    } 
	    return true; 
	    } 
	    return super.onKeyDown(keyCode, event); 
    } 
    
    @Override
    public void onClick(View v)
    {
    	
    	switch (v.getId()) {
		case R.id.btnDesktop:
			if (!config.isConnected() || config.getServerHost()=="") {
	    		Toast.makeText(getApplicationContext(), "请先连接电脑...", Toast.LENGTH_SHORT).show();
	    		//findHost();
			}
	    	else {
	    		Intent intent= new Intent();
	        	intent.setClass(UITestActivity.this, DeskTopSocketWithControlerActivity.class);
	        	startActivity(intent);	
			}
			break;
		case R.id.btnFileManager:
			if (!config.isConnected() || config.getServerHost()=="") {
	    		Toast.makeText(getApplicationContext(), "请先连接电脑...", Toast.LENGTH_SHORT).show();
	    		//findHost();
			}
	    	else {
				Intent intent= new Intent();
	        	intent.setClass(UITestActivity.this, FileManagerActivity.class);
	        	startActivity(intent);
	    	}
			break;
		case R.id.btnOpstions:
			Intent intent= new Intent();
        	intent.setClass(UITestActivity.this, OptionsActivity.class);
        	startActivity(intent);
			break;
		case R.id.btnConnetc:
			if (!config.isConnected()) {
				findHost();
			}
			break;
		case R.id.btnPlugins:
			Intent pIntent= new Intent();
			pIntent.setClass(UITestActivity.this, PluginManagerActivity.class);
        	startActivity(pIntent);
			break;
		default:
			break;
		}
    	
    }
    
    
    private boolean findHost()
    {
    	try {
    		if (config.getConnectModel()==ConnectModel.LocalNetwork) 
    		{
    			//局域网配对方式
	    		 MulticastInfo info = ServerMutual.GetServerAddressInfo();
		   		 if (info!=null&&info.messageString!=null&&info.IP!=null) {
		   			 String recv= info.messageString.trim();
		   			//Toast.makeText(getApplicationContext(), recv, Toast.LENGTH_SHORT).show();
		   			 if (recv.length()>=2) {
			   				config.setServerHost(info.IP);
		   		         if (recv.equals("ok")) {
		   		        	connectButton.setText("成功连接电脑");
	   						config.setConnected(true);
							return true;
						 }
		   		         else if (recv.equals("key")) {
		   		        	showPopupWindow(UITestActivity.this, connectButton);
						 }
		   			}
		   		 }
		   		 else {
						Toast.makeText(getApplicationContext(), "没有找到目标主机,正在尝试寻找...\n请稍后再试", Toast.LENGTH_SHORT).show();
				}
    		}
    		else {
				//外网配对方式
    			StringRecieveEvent sRecieveEvent=new StringRecieveEvent();
    			SimpleSocketHelper.sendString(config.getServerHost(), config.getServerPort(), "connect",sRecieveEvent);
    			String rstr=sRecieveEvent.parameters.toString();
    			if (rstr.equals("ok")) {
   		        	connectButton.setText("成功连接电脑");
					config.setConnected(true);
				}
    			else if (rstr.equals("key")) {
   		        	showPopupWindow(UITestActivity.this, connectButton);
    			}
    			else {
    				connectButton.setText("点击连接电脑");
					config.setConnected(false);
				}
			}
	   		return false;
		} catch (Exception e) {
			// TODO: handle exception
			Toast.makeText(getApplicationContext(), e.getMessage(), Toast.LENGTH_SHORT).show();
			return false;
		}
    	
    }
    
    
    public void showPopupWindow(Context context,View parent){  
        LayoutInflater inflater = (LayoutInflater)     
           context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);     
        final View vPopupWindow=inflater.inflate(R.layout.authenticationwindow, null, false);  
        final PopupWindow pw= new PopupWindow(vPopupWindow,400,300,true);
        pw.setOutsideTouchable(false);
        //OK按钮及其处理事件   
        Button btnOK=(Button)vPopupWindow.findViewById(R.id.btnOK);  
        btnOK.setOnClickListener(new OnClickListener(){  
            @Override  
            public void onClick(View v) {  
                //设置文本框内容   
                EditText keyEditText=(EditText)vPopupWindow.findViewById(R.id.etAtKey);  
                String keyString = keyEditText.getText().toString();
                if (keyString.length()>0) {
			 		 config.setKeyString(keyString);
			 		 if (config.getConnectModel()==ConnectModel.LocalNetwork) {
				 		 MulticastInfo info2 = ServerMutual.SendAuthenticationString(config.getKeyString());
		   		        	if (info2!=null && info2.messageString.trim().equals("ok")) {
		   		        		connectButton.setText("成功连接电脑");
		   						config.setConnected(true);
							}
		   		        	else {
		   		        		Toast.makeText(getApplicationContext(), "配对钥匙错误,配对失败", Toast.LENGTH_SHORT).show();
		   		        		connectButton.setText("点击连接电脑");
		   		        		config.setConnected(false);
							}
				 		pw.dismiss();
			 		 }
			 		 else {
			 			StringRecieveEvent sRecieveEvent=new StringRecieveEvent();
						SimpleSocketHelper.sendString(config.getServerHost(), config.getServerPort(),Commands.connectString + config.getKeyString(),sRecieveEvent);
						if (sRecieveEvent.parameters.toString().equals("ok")) {
							connectButton.setText("成功连接电脑");
	   						config.setConnected(true);
						}
						else {
	   		        		Toast.makeText(getApplicationContext(), "配对钥匙错误,配对失败", Toast.LENGTH_SHORT).show();
	   		        		connectButton.setText("点击连接电脑");
	   		        		config.setConnected(false);
						}
				 		pw.dismiss();
					}
				}
				else {
					Toast.makeText(getApplicationContext(), "配对钥匙不能为空!", Toast.LENGTH_SHORT).show();
				}
            }  
        });  
          
        //显示popupWindow对话框   
        pw.showAtLocation(parent, Gravity.CENTER, 0, 0);  
    }  
      
}  